using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

// SetApiUrl/SetAllowInsecureHttp mutate process-wide environment variables (TNZ_API_URL,
// TNZ_ALLOW_INSECURE_HTTP) — every test that changes them away from DemoApiFactory's pinned
// defaults must restore them in OnDispose(), or the change leaks into every test class that runs
// afterward in this same process (tests run sequentially, not isolated — see AssemblyInfo.cs).
public class SettingsControllerTests : DemoApiTestBase
{
    public SettingsControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    protected override void OnDispose()
    {
        Environment.SetEnvironmentVariable("TNZ_API_URL", "https://api.tnz.co.nz");
        Environment.SetEnvironmentVariable("TNZ_ALLOW_INSECURE_HTTP", "false");
    }

    [Fact]
    public async Task GetApiUrl_ReturnsCurrentValue()
    {
        var response = await Client.GetAsync("/api/settings/api-url");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("https://api.tnz.co.nz", body.GetProperty("ApiUrl").GetString());
    }

    [Fact]
    public async Task SetApiUrl_InDevelopment_UpdatesValue()
    {
        // WebApplicationFactory defaults its hosting environment to Development, matching this
        // Demo's own Dockerfile (ENV ASPNETCORE_ENVIRONMENT=Development).
        var response = await Client.PostAsJsonAsync("/api/settings/api-url", new { ApiUrl = "https://staging.example.com" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://staging.example.com", Environment.GetEnvironmentVariable("TNZ_API_URL"));
    }

    [Fact]
    public async Task SetApiUrl_WithEmptyUrl_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/settings/api-url", new { ApiUrl = "" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetApiUrl_WithNonHttpScheme_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/settings/api-url", new { ApiUrl = "ftp://example.com" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetApiUrl_OutsideDevelopment_ReturnsForbidden()
    {
        await using var productionFactory = new DemoApiFactory().WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
        using var client = productionFactory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/settings/api-url", new { ApiUrl = "https://evil.example.com" });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        // Confirms the guard actually blocked the mutation, not just returned 403 after applying it.
        Assert.NotEqual("https://evil.example.com", Environment.GetEnvironmentVariable("TNZ_API_URL"));
    }

    [Fact]
    public async Task GetSslVerification_DefaultsToEnabled()
    {
        var response = await Client.GetAsync("/api/settings/ssl-verification");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.True(body.GetProperty("Enabled").GetBoolean());
    }

    [Fact]
    public async Task SetSslVerification_Disabled_ThenReportsDisabled()
    {
        var setResponse = await Client.PostAsJsonAsync("/api/settings/ssl-verification", new { Enabled = false });
        Assert.Equal(HttpStatusCode.OK, setResponse.StatusCode);

        var getResponse = await Client.GetAsync("/api/settings/ssl-verification");
        var body = JsonDocument.Parse(await getResponse.Content.ReadAsStringAsync()).RootElement;
        Assert.False(body.GetProperty("Enabled").GetBoolean());
    }

    [Fact]
    public async Task GetAllowInsecureHttp_DefaultsToDisabled()
    {
        var response = await Client.GetAsync("/api/settings/allow-insecure-http");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.False(body.GetProperty("Enabled").GetBoolean());
    }

    [Fact]
    public async Task SetAllowInsecureHttp_Enabled_ThenReportsEnabled()
    {
        var setResponse = await Client.PostAsJsonAsync("/api/settings/allow-insecure-http", new { Enabled = true });
        Assert.Equal(HttpStatusCode.OK, setResponse.StatusCode);

        var getResponse = await Client.GetAsync("/api/settings/allow-insecure-http");
        var body = JsonDocument.Parse(await getResponse.Content.ReadAsStringAsync()).RootElement;
        Assert.True(body.GetProperty("Enabled").GetBoolean());
    }
}