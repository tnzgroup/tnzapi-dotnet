using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

public class ActionsControllerTests : DemoApiTestBase
{
    public ActionsControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    // --- Abort: all 7 channels with an Actions facade ---

    [Theory]
    [InlineData("sms", "sms")]
    [InlineData("email", "email")]
    [InlineData("fax", "fax")]
    [InlineData("tts", "tts")]
    [InlineData("voice", "voice")]
    [InlineData("whatsapp", "whatsapp")]
    [InlineData("rcs", "rcs")]
    public async Task Abort_DispatchesToCorrectChannelUrl(string channel, string urlSegment)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ActionResult\":\"OK\"}");

        var response = await Client.PostAsJsonAsync("/api/actions/abort", new { MessageID = "abc-123", Channel = channel });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal($"https://api.tnz.co.nz/api/v3.00/{urlSegment}/abc-123/abort", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Abort_WithUnknownChannel_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/actions/abort", new { MessageID = "abc-123", Channel = "workflow" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Contains("Unknown channel", body.GetProperty("ErrorMessage")[0].GetString());
    }

    [Fact]
    public async Task Abort_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        var response = await Client.PostAsJsonAsync("/api/actions/abort", new { MessageID = "abc-123", Channel = "sms" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("Unauthorized", body.GetProperty("Result").GetString());
    }

    // --- Reschedule: same 7 channels as Abort ---

    [Theory]
    [InlineData("sms", "sms")]
    [InlineData("email", "email")]
    [InlineData("fax", "fax")]
    [InlineData("tts", "tts")]
    [InlineData("voice", "voice")]
    [InlineData("whatsapp", "whatsapp")]
    [InlineData("rcs", "rcs")]
    public async Task Reschedule_DispatchesToCorrectChannelUrl(string channel, string urlSegment)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ActionResult\":\"OK\"}");

        var response = await Client.PostAsJsonAsync("/api/actions/reschedule", new { MessageID = "abc-123", Channel = channel, SendTime = "2026-08-01 09:00:00" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal($"https://api.tnz.co.nz/api/v3.00/{urlSegment}/abc-123/reschedule", handler.LastRequest!.RequestUri!.ToString());
        Assert.Contains("SendTime", handler.LastRequestBody);
    }

    [Fact]
    public async Task Reschedule_WithInvalidSendTime_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ActionResult\":\"OK\"}");

        var response = await Client.PostAsJsonAsync("/api/actions/reschedule", new { MessageID = "abc-123", Channel = "sms", SendTime = "not-a-date" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Contains("Invalid SendTime", body.GetProperty("ErrorMessage")[0].GetString());
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Reschedule_WithUnknownChannel_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/actions/reschedule", new { MessageID = "abc-123", Channel = "workflow", SendTime = "2026-08-01 09:00:00" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // --- Resubmit: only Email/Fax/TTS/Voice — no SMS/WhatsApp/RCS ---

    [Theory]
    [InlineData("email", "email")]
    [InlineData("fax", "fax")]
    [InlineData("tts", "tts")]
    [InlineData("voice", "voice")]
    public async Task Resubmit_DispatchesToCorrectChannelUrl(string channel, string urlSegment)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ActionResult\":\"OK\"}");

        var response = await Client.PostAsJsonAsync("/api/actions/resubmit", new { MessageID = "abc-123", Channel = channel, SendTime = "2026-08-01 09:00:00" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal($"https://api.tnz.co.nz/api/v3.00/{urlSegment}/abc-123/resubmit", handler.LastRequest!.RequestUri!.ToString());
    }

    [Theory]
    [InlineData("sms")]
    [InlineData("whatsapp")]
    [InlineData("rcs")]
    public async Task Resubmit_WithUnsupportedChannel_ReturnsBadRequest(string channel)
    {
        var response = await Client.PostAsJsonAsync("/api/actions/resubmit", new { MessageID = "abc-123", Channel = channel, SendTime = "2026-08-01 09:00:00" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Contains("Resubmit supports email, fax, tts, voice", body.GetProperty("ErrorMessage")[0].GetString());
    }

    [Fact]
    public async Task Resubmit_WithInvalidSendTime_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ActionResult\":\"OK\"}");

        var response = await Client.PostAsJsonAsync("/api/actions/resubmit", new { MessageID = "abc-123", Channel = "email", SendTime = "not-a-date" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    // --- Pacing: only TTS/Voice ---

    [Theory]
    [InlineData("tts", "tts")]
    [InlineData("voice", "voice")]
    public async Task Pacing_DispatchesToCorrectChannelUrl(string channel, string urlSegment)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ActionResult\":\"OK\"}");

        var response = await Client.PostAsJsonAsync("/api/actions/pacing", new { MessageID = "abc-123", Channel = channel, NumberOfOperators = 5 });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal($"https://api.tnz.co.nz/api/v3.00/{urlSegment}/abc-123/pacing", handler.LastRequest!.RequestUri!.ToString());
        Assert.Contains("\"NumberOfOperators\":5", handler.LastRequestBody);
    }

    [Theory]
    [InlineData("sms")]
    [InlineData("email")]
    public async Task Pacing_WithUnsupportedChannel_ReturnsBadRequest(string channel)
    {
        var response = await Client.PostAsJsonAsync("/api/actions/pacing", new { MessageID = "abc-123", Channel = channel, NumberOfOperators = 5 });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Contains("Pacing supports tts, voice only", body.GetProperty("ErrorMessage")[0].GetString());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Pacing_WithNonPositiveNumberOfOperators_ReturnsBadRequestWithoutCallingSdk(int numberOfOperators)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ActionResult\":\"OK\"}");

        var response = await Client.PostAsJsonAsync("/api/actions/pacing", new { MessageID = "abc-123", Channel = "tts", NumberOfOperators = numberOfOperators });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Contains("Invalid NumberOfOperators", body.GetProperty("ErrorMessage")[0].GetString());
    }

    [Fact]
    public async Task Pacing_WithNumberOfOperatorsOmitted_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ActionResult\":\"OK\"}");

        var response = await Client.PostAsJsonAsync("/api/actions/pacing", new { MessageID = "abc-123", Channel = "tts" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }
}