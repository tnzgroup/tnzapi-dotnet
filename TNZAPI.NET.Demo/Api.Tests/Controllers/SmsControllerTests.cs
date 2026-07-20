using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

// Demo API controllers are thin pass-throughs to the SDK — these tests confirm the wiring (request
// binds correctly, SDK gets called, response round-trips through RespondWithResult) rather than
// re-testing SDK behavior already covered by TNZAPI.NET.Tests.
public class SmsControllerTests : DemoApiTestBase
{
    public SmsControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Send_OnSuccess_Returns200WithFullResult()
    {
        FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/sms/send", new { ToNumber = "+64211234567", Message = "Hello" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("abc-123", body.GetProperty("MessageID").GetString());
    }

    [Fact]
    public async Task Send_WithUnparseableSendTime_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/sms/send", new { ToNumber = "+64211234567", Message = "Hello", SendTime = "not-a-date" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Send_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        var response = await Client.PostAsJsonAsync("/api/sms/send", new { ToNumber = "+64211234567", Message = "Hello" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("Unauthorized", body.GetProperty("Result").GetString());
        Assert.Equal("Access denied", body.GetProperty("ErrorMessage")[0].GetString());
        // RespondWithResult trims failure responses to {Result, ErrorMessage} only.
        Assert.False(body.TryGetProperty("MessageID", out _));
    }

    [Fact]
    public async Task Status_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\"}");

        var response = await Client.GetAsync("/api/sms/status/abc-123");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/sms/abc-123", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Reply_PassesRecordsPerPageAndPageThrough()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\"}");

        var response = await Client.GetAsync("/api/sms/reply/abc-123?recordsPerPage=25&page=2");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("recordsPerPage=25", handler.LastRequest!.RequestUri!.ToString());
        Assert.Contains("page=2", handler.LastRequest.RequestUri!.ToString());
    }

    [Fact]
    public async Task Received_WithTimePeriod_PassesItThrough()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"TotalRecords\":0}");

        var response = await Client.GetAsync("/api/sms/received?timePeriod=60&recordsPerPage=25&page=2");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("timePeriod=60", url);
        Assert.Contains("recordsPerPage=25", url);
        Assert.Contains("page=2", url);
        Assert.DoesNotContain("dateFrom", url);
    }

    [Fact]
    public async Task Received_WithDateFromDateTo_PassesThemThrough()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"TotalRecords\":0}");

        var response = await Client.GetAsync("/api/sms/received?dateFrom=2026-01-01&dateTo=2026-01-31");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("dateFrom", url);
        Assert.Contains("dateTo", url);
    }

    [Fact]
    public async Task Received_WithDateFromOnly_RoutesToDateFromOverload()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"TotalRecords\":0}");

        var response = await Client.GetAsync("/api/sms/received?dateFrom=2026-01-01");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var url = handler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("dateFrom", url);
        Assert.DoesNotContain("timePeriod", url);
    }

    [Fact]
    public async Task Received_WithUnparseableDateFrom_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"TotalRecords\":0}");

        var response = await Client.GetAsync("/api/sms/received?dateFrom=not-a-date");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Received_WithUnparseableDateTo_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"TotalRecords\":0}");

        var response = await Client.GetAsync("/api/sms/received?dateFrom=2026-01-01&dateTo=not-a-date");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Received_WithDateToOnly_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"TotalRecords\":0}");

        var response = await Client.GetAsync("/api/sms/received?dateTo=2026-01-31");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Received_WithDateFromAfterDateTo_ReturnsBadRequestFromSdkValidationWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"TotalRecords\":0}");

        var response = await Client.GetAsync("/api/sms/received?dateFrom=2026-01-31&dateTo=2026-01-01");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Contains("dateFrom must not be after dateTo", body.GetProperty("ErrorMessage")[0].GetString());
    }

    [Theory]
    [InlineData("recordsPerPage=0&page=1")]
    [InlineData("recordsPerPage=1001&page=1")]
    [InlineData("recordsPerPage=25&page=0")]
    public async Task Reply_WithInvalidPagination_ReturnsBadRequestWithoutCallingSdk(string query)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\"}");

        var response = await Client.GetAsync($"/api/sms/reply/abc-123?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Theory]
    [InlineData("recordsPerPage=0&page=1")]
    [InlineData("recordsPerPage=1001&page=1")]
    [InlineData("recordsPerPage=100&page=0")]
    public async Task Received_WithInvalidPagination_ReturnsBadRequestWithoutCallingSdk(string query)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"TotalRecords\":0}");

        var response = await Client.GetAsync($"/api/sms/received?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }
}