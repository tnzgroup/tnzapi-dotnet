using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

public class AuthControllerTests : DemoApiTestBase
{
    public AuthControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SetToken_WithJwtShapedToken_ReturnsOk()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/token", new { Token = "header.payload.signature" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("ok", body.GetProperty("Status").GetString());
    }

    [Fact]
    public async Task SetToken_WithEmptyToken_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/token", new { Token = "" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SetToken_WithNonJwtShapedToken_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/token", new { Token = "not-a-jwt" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Contains("three dot-separated segments", body.GetProperty("Error").GetString());
    }
}