using System.Text.Json.Serialization;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// TNZApiConfig.Domain and the HTTPS-only guard in HttpRequest.cs read TNZ_API_URL /
// TNZ_ALLOW_INSECURE_HTTP via Environment.GetEnvironmentVariable directly — they never go through
// IConfiguration, so appsettings.json entries alone would be silently ignored. Seed the real
// process env var from config here, once, at startup — but only when it isn't already set, so a
// real environment variable (e.g. docker-compose.yml's `environment:` section) still wins over
// appsettings.json, matching the precedence ASP.NET Core's own config providers use.
SeedEnvVarFromConfig("TNZ_API_URL");
SeedEnvVarFromConfig("TNZ_ALLOW_INSECURE_HTTP");

void SeedEnvVarFromConfig(string key)
{
    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
    {
        return;
    }

    var value = builder.Configuration[key];
    if (!string.IsNullOrEmpty(value))
    {
        Environment.SetEnvironmentVariable(key, value);
    }
}

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // null means "use the C# property name verbatim" (PascalCase) instead of ASP.NET Core's
        // default camelCase policy — matches the TNZ REST API's own wire convention this Demo
        // showcases, and the SDK's own model property names, rather than translating between the two.
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<ITokenProvider, SessionTokenProvider>();
builder.Services.AddScoped(sp => new TNZApiClient(sp.GetRequiredService<ITokenProvider>().Resolve()));

var app = builder.Build();

// No AddCors/UseCors call, and no [Authorize] on any controller (SettingsController's write actions
// are the only ones gated, and only to ASPNETCORE_ENVIRONMENT=Development). Both are deliberate for a
// local single-developer demo, not oversights:
//  - CORS: default deny-all is safe here since the SPA only ever talks to this API same-origin, via
//    docker-compose.yml's Vite dev-server proxy (VITE_API_PROXY_TARGET) — the browser never sees a
//    cross-origin request to add a policy for. Don't add a wildcard AllowAnyOrigin to "fix" this.
//  - Auth: every messaging/addressbook/optout endpoint uses whatever AuthToken this process holds
//    (see SessionTokenProvider) with no caller identity check of its own. Note this isn't only a risk
//    if someone later "deploys" this somewhere — docker-compose.yml's default "5080:5080" port
//    publish already binds to all interfaces, so simply running `docker-compose up` on a machine
//    that isn't behind a restrictive firewall exposes this to the whole LAN/Wi-Fi. Anyone who can
//    reach the port can send messages and mutate records on the real TNZ account behind the
//    configured token. Restrict network exposure (firewall, host-only port binding, etc.) or add a
//    real auth gate before running this anywhere other than an isolated developer machine.
app.UseSession();
app.MapControllers();

app.Run();

// Top-level statements generate an internal Program class by default — this partial declaration
// makes it public so TNZAPI.NET.Demo.Api.Tests can target it with WebApplicationFactory<Program>.
public partial class Program { }