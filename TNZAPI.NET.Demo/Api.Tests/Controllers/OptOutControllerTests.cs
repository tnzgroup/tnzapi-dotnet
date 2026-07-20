using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

// Covers OptOutController — client.Configuration.OptOut (List/Create/Details/Delete).
public class OptOutControllerTests : DemoApiTestBase
{
    public OptOutControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task List_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"OptOuts\":[]}");

        var response = await Client.GetAsync("/api/optout?page=1&recordsPerPage=50");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/optout/list?page=1&recordsPerPage=50", handler.LastRequest!.RequestUri!.ToString());
    }

    [Theory]
    [InlineData("page=0&recordsPerPage=50")]
    [InlineData("page=1&recordsPerPage=0")]
    [InlineData("page=1&recordsPerPage=1001")]
    public async Task List_WithInvalidPagination_ReturnsBadRequestWithoutCallingSdk(string query)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"OptOuts\":[]}");

        var response = await Client.GetAsync($"/api/optout?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Create_OnSuccess_Returns200WithFullResult()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ID\":\"optout-1\",\"Destination\":\"+64211234567\"}");

        var response = await Client.PostAsJsonAsync("/api/optout", new { Destination = "+64211234567", DestType = "sms" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/optout", handler.LastRequest!.RequestUri!.ToString());
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("optout-1", body.GetProperty("ID").GetString());
    }

    [Fact]
    public async Task Details_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ID\":\"optout-1\"}");

        var response = await Client.GetAsync("/api/optout/optout-1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/optout/optout-1", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Delete_UsesCorrectUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ID\":\"optout-1\"}");

        var response = await Client.DeleteAsync("/api/optout/optout-1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/optout/optout-1", handler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Delete, handler.LastRequest.Method);
    }

    [Fact]
    public async Task Create_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        var response = await Client.PostAsJsonAsync("/api/optout", new { Destination = "+64211234567", DestType = "sms" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}