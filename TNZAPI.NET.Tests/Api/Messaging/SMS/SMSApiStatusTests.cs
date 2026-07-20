using System.Net;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.SMS;

public class SMSApiStatusTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task StatusAsync_GetsCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"JobStatus\":\"Pending\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.StatusAsync(new MessageID("abc-123"));

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/sms/abc-123", fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Get, fakeHandler.LastRequest!.Method);
    }

    [Fact]
    public async Task StatusAsync_ParsesJobStatusAndRecipients()
    {
        var body = "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\",\"Recipients\":[{\"Type\":\"SMS\",\"Destination\":\"+64211234567\",\"Status\":\"Success\",\"Result\":\"Delivered\"}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.StatusAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal(Enums.JobStatus.Completed, result.JobStatus);
        Assert.Single(result.Recipients);
        Assert.Equal(Enums.RecipientChannelType.SMS, result.Recipients[0].Type);
        Assert.Equal(Enums.MessageStatus.Success, result.Recipients[0].Status);
    }

    [Fact]
    public async Task StatusAsync_ParsesNumericPrice()
    {
        // Price is a plain decimal? here — confirmed via the live API response to always arrive as
        // a JSON number, and decimal is the correct client-side type for money once it reaches the
        // public SDK surface. Regression test for the deserialization crash this
        // originally caused when Price was string? (System.Text.Json throwing "The JSON value
        // could not be converted to System.String").
        var body = "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\",\"Price\":0.1,\"Recipients\":[{\"Type\":\"SMS\",\"Destination\":\"+64211234567\",\"Status\":\"Success\",\"Result\":\"Delivered\",\"Price\":0.05}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.StatusAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal(0.1m, result.Price);
        Assert.Equal(0.05m, result.Recipients[0].Price);
    }

    [Fact]
    public async Task StatusAsync_ParsesTextRecipientTypeAsSMS()
    {
        // Confirmed via the live API response: SMS recipients come back on the wire as "Text", not
        // "SMS" — this reproduces the response shape that surfaced the bug (RecipientChannelType had
        // no matching member, so JsonStringEnumConverter threw). Field values below are synthetic.
        var body = "{\"MessageID\":\"ID123456\",\"JobStatus\":\"Completed\",\"JobNum\":\"12907969\",\"Account\":\"TNZ\",\"SubAccount\":\"Web Server\",\"Department\":\"\",\"Reference\":\"AdminV2 New DDI Request\",\"CreatedTimeLocal\":\"2022-03-20 09:49:41\",\"CreatedTimeUTC\":\"2022-03-19 21:49:41\",\"Timezone\":\"New Zealand\",\"Count\":0,\"Complete\":1,\"Success\":1,\"Failed\":0,\"Price\":0.1,\"TotalRecords\":1,\"RecordsPerPage\":100,\"PageCount\":1,\"Page\":1,\"Recipients\":[{\"Type\":\"Text\",\"DestSeq\":\"00F16FEE\",\"Destination\":\"+64211234567\",\"Status\":\"Success\",\"Result\":\"Sent OK\",\"SentTimeLocal\":\"2021-06-21 12:59:07\",\"SentTimeUTC\":\"2021-06-21 00:59:07\",\"RemoteID\":\"\",\"Price\":0.1}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.StatusAsync(new MessageID("ID123456"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Single(result.Recipients);
        Assert.Equal(Enums.RecipientChannelType.SMS, result.Recipients[0].Type);
        Assert.Equal("Sent OK", result.Recipients[0].Result);
    }

    [Fact]
    public async Task StatusAsync_On404_ReturnsRecordNotFound()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, "{\"Result\":\"RecordNotFound\",\"MessageID\":\"abc-123\",\"ErrorMessage\":[\"Not found\"]}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.StatusAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.RecordNotFound, result.Result);
    }

    [Fact]
    public async Task StatusAsync_WithEmptyMessageID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.StatusAsync(new MessageID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void Status_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"JobStatus\":\"Pending\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = sms.Status(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}