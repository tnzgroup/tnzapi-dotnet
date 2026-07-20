using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.SMS;

public class SMSApiActionsTests : IDisposable
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
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });
        var sendTime = new DateTime(2026, 8, 1, 10, 0, 0);

        await sms.RescheduleAsync(new MessageID("abc-123"), sendTime);

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/sms/abc-123/reschedule", fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Patch, fakeHandler.LastRequest!.Method);
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("2026-08-01T10:00:00", doc.RootElement.GetProperty("SendTime").GetString());
    }

    [Fact]
    public async Task AbortAsync_PatchesCorrectUrlWithNoBody()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Abort\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.AbortAsync(new MessageID("abc-123"));

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/sms/abc-123/abort", fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Patch, fakeHandler.LastRequest!.Method);
    }

    [Fact]
    public async Task RescheduleAsync_WithEmptyMessageID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.RescheduleAsync(new MessageID(""), DateTime.Now);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }
}