using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Voice;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.Voice;

public class VoiceApiActionsTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task RescheduleAsync_PatchesCorrectUrlWithSendTime()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Reschedule\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var voice = new VoiceApi(new TNZApiUser { AuthToken = "test-token" });
        var sendTime = new DateTime(2026, 8, 1, 10, 0, 0);

        await voice.RescheduleAsync(new MessageID("abc-123"), sendTime);

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/voice/abc-123/reschedule", fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("2026-08-01T10:00:00", doc.RootElement.GetProperty("SendTime").GetString());
    }

    [Fact]
    public async Task AbortAsync_PatchesCorrectUrlWithNoBody()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Abort\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var voice = new VoiceApi(new TNZApiUser { AuthToken = "test-token" });

        await voice.AbortAsync(new MessageID("abc-123"));

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/voice/abc-123/abort", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ResubmitAsync_PatchesCorrectUrlWithSendTime()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Resubmit\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var voice = new VoiceApi(new TNZApiUser { AuthToken = "test-token" });
        var sendTime = new DateTime(2026, 8, 1, 10, 0, 0);

        var result = await voice.ResubmitAsync(new MessageID("abc-123"), sendTime);

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/voice/abc-123/resubmit", fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task PacingAsync_PatchesCorrectUrlWithNumberOfOperators()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Pacing\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var voice = new VoiceApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await voice.PacingAsync(new MessageID("abc-123"), 10);

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/voice/abc-123/pacing", fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal(10, doc.RootElement.GetProperty("NumberOfOperators").GetInt32());
        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task RescheduleAsync_WithEmptyMessageID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var voice = new VoiceApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await voice.RescheduleAsync(new MessageID(""), DateTime.Now);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }
}