using System.Net;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.Fax;

public class FaxApiStatusTests : IDisposable
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
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.StatusAsync(new MessageID("abc-123"));

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/fax/abc-123", fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Get, fakeHandler.LastRequest!.Method);
    }

    [Fact]
    public async Task StatusAsync_ParsesJobStatusAndRecipients()
    {
        var body = "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\",\"Recipients\":[{\"Type\":\"Fax\",\"Destination\":\"+6495006000\",\"Status\":\"Success\",\"Result\":\"Sent OK\"}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await fax.StatusAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal(Enums.JobStatus.Completed, result.JobStatus);
        Assert.Single(result.Recipients);
        Assert.Equal(Enums.RecipientChannelType.Fax, result.Recipients[0].Type);
        Assert.Equal("Sent OK", result.Recipients[0].Result);
    }

    [Fact]
    public async Task StatusAsync_ParsesNumericPrice()
    {
        // Price is a plain decimal? here — confirmed via the live API response to always arrive as
        // a JSON number, and decimal is the correct client-side type for money once it reaches the
        // public SDK surface. Regression test for the deserialization crash this
        // originally caused when Price was string? (System.Text.Json throwing "The JSON value
        // could not be converted to System.String").
        var body = "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Completed\",\"Price\":0.1,\"Recipients\":[{\"Type\":\"Fax\",\"Destination\":\"+6495006000\",\"Status\":\"Success\",\"Result\":\"Sent OK\",\"Price\":0.05}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await fax.StatusAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal(0.1m, result.Price);
        Assert.Equal(0.05m, result.Recipients[0].Price);
    }

    [Fact]
    public async Task StatusAsync_On404_ReturnsRecordNotFound()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, "{\"Result\":\"RecordNotFound\",\"MessageID\":\"abc-123\",\"ErrorMessage\":[\"Not found\"]}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await fax.StatusAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.RecordNotFound, result.Result);
    }

    [Fact]
    public async Task StatusAsync_WithEmptyMessageID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await fax.StatusAsync(new MessageID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void Status_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"JobStatus\":\"Pending\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = fax.Status(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}