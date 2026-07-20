using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

public class FaxControllerTests : DemoApiTestBase
{
    public FaxControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Send_OnSuccess_Returns200WithFullResult()
    {
        FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        // FaxApi's client-side validation requires either Files or TemplateID (fax has no
        // message-text field — the document to send is always an attachment or a pre-set template).
        var response = await Client.PostAsJsonAsync("/api/fax/send", new { ToNumber = "+6495006000", TemplateId = "tmpl-1" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("abc-123", body.GetProperty("MessageID").GetString());
    }

    [Fact]
    public async Task Send_WithUnparseableSendTime_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/fax/send", new { ToNumber = "+6495006000", TemplateId = "tmpl-1", SendTime = "not-a-date" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Send_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        // FaxApi's client-side validation requires either Files or TemplateID (fax has no
        // message-text field — the document to send is always an attachment or a pre-set template).
        var response = await Client.PostAsJsonAsync("/api/fax/send", new { ToNumber = "+6495006000", TemplateId = "tmpl-1" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("Unauthorized", body.GetProperty("Result").GetString());
        Assert.False(body.TryGetProperty("MessageID", out _));
    }

    [Fact]
    public async Task Status_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\"}");

        var response = await Client.GetAsync("/api/fax/status/abc-123");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/fax/abc-123", handler.LastRequest!.RequestUri!.ToString());
    }
}