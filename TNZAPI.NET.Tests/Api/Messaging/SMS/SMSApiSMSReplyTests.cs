using System.Net;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.SMS;

public class SMSApiSMSReplyTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task SMSReplyAsync_BuildsCorrectPaginatedURL()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SMSReplyAsync(new MessageID("abc-123"), recordsPerPage: 50, page: 2);

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/sms/abc-123?recordsPerPage=50&page=2", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task SMSReplyAsync_UsesDefaultsWhenNotSpecified()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SMSReplyAsync(new MessageID("abc-123"));

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/sms/abc-123?recordsPerPage=100&page=1", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task SMSReplyAsync_ParsesResponse()
    {
        var body = "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\",\"Recipients\":[{\"Type\":\"SMS\",\"Destination\":\"+64211234567\",\"SMSReplies\":[{\"MessageText\":\"hi\"}]}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.SMSReplyAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Single(result.Recipients);
        Assert.Single(result.Recipients[0].SMSReplies);
        Assert.Equal("hi", result.Recipients[0].SMSReplies[0].MessageText);
    }

    [Fact]
    public async Task SMSReplyAsync_WithEmptyMessageID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.SMSReplyAsync(new MessageID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task SMSReplyAsync_WithEmptyAuthToken_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "" });

        var result = await sms.SMSReplyAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void SMSReply_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = sms.SMSReply(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}