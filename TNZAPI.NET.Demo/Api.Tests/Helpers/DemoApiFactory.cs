using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace TNZAPI.NET.Demo.Api.Tests.Helpers;

// Pins the env-var-backed settings TNZApiConfig/HttpRequest read directly (TNZ_API_URL,
// TNZ_ALLOW_INSECURE_HTTP) before the host ever boots, so tests get a deterministic
// https://api.tnz.co.nz to assert against regardless of what a developer's local, gitignored
// appsettings.Development.json happens to contain (e.g. a docker-host URL for manual testing).
// Setting these directly — rather than via IConfiguration — sidesteps Program.cs's own
// SeedEnvVarFromConfig, which only seeds the env var when it's still empty: since these are set
// here first, that seeding step becomes a no-op and never reads the file-based config at all.
// TNZ_AUTH_TOKEN is still supplied via IConfiguration since SessionTokenProvider reads it through
// IConfiguration directly, not via an env var.
public class DemoApiFactory : WebApplicationFactory<Program>
{
    static DemoApiFactory()
    {
        Environment.SetEnvironmentVariable("TNZ_API_URL", "https://api.tnz.co.nz");
        Environment.SetEnvironmentVariable("TNZ_ALLOW_INSECURE_HTTP", "false");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["TNZ_AUTH_TOKEN"] = "test-token",
            });
        });
    }
}