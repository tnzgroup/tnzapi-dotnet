using System.Net;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

public class HealthControllerTests : DemoApiTestBase
{
    public HealthControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_ReturnsOkWithStatusOk()
    {
        var response = await Client.GetAsync("/api/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("ok", body.RootElement.GetProperty("Status").GetString());
    }
}