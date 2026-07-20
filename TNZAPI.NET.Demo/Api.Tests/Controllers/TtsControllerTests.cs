using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

public class TtsControllerTests : DemoApiTestBase
{
    public TtsControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Send_OnSuccess_Returns200WithFullResult()
    {
        FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/tts/send", new { ToNumber = "+6421000001", MessageToPeople = "Hello" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("abc-123", body.GetProperty("MessageID").GetString());
    }

    [Fact]
    public async Task Send_WithUnparseableSendTime_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/tts/send", new { ToNumber = "+6421000001", MessageToPeople = "Hello", SendTime = "not-a-date" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Send_WithKeypads_Returns200()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/tts/send", new
        {
            ToNumber = "+6421000001",
            MessageToPeople = "Press 1 for sales",
            Keypads = new[] { new { Tone = 1, Play = "sales.wav", RouteNumber = "+6495551234", PlaySection = "Main" } },
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("\"Keypads\"", handler.LastRequestBody);
    }

    [Fact]
    public async Task Send_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        var response = await Client.PostAsJsonAsync("/api/tts/send", new { ToNumber = "+6421000001", MessageToPeople = "Hello" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("Unauthorized", body.GetProperty("Result").GetString());
    }

    [Fact]
    public async Task Status_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\"}");

        var response = await Client.GetAsync("/api/tts/status/abc-123");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/tts/abc-123", handler.LastRequest!.RequestUri!.ToString());
    }
}