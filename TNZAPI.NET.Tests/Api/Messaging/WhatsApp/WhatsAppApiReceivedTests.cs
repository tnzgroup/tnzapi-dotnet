using System.Net;
using TNZAPI.NET.Api.Messaging.WhatsApp;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.WhatsApp;

public class WhatsAppApiReceivedTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task ReceivedAsync_GetsCorrectUrlWithQueryParams()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Messages\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var whatsApp = new WhatsAppApi(new TNZApiUser { AuthToken = "test-token" });

        await whatsApp.ReceivedAsync(timePeriod: 60, recordsPerPage: 50, page: 2);

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/whatsapp/received?timePeriod=60&recordsPerPage=50&page=2",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ReceivedAsync_UsesDefaultsWhenNotSpecified()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Messages\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var whatsApp = new WhatsAppApi(new TNZApiUser { AuthToken = "test-token" });

        await whatsApp.ReceivedAsync(timePeriod: 1440);

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/whatsapp/received?timePeriod=1440&recordsPerPage=100&page=1",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ReceivedAsync_ParsesMessages()
    {
        var body = "{\"Messages\":[{\"MessageID\":\"abc-123\",\"From\":\"+64211234567\",\"MessageText\":\"hi\"}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var whatsApp = new WhatsAppApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await whatsApp.ReceivedAsync(timePeriod: 60);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Single(result.Messages);
        Assert.Equal("+64211234567", result.Messages[0].From);
    }

    [Fact]
    public async Task ReceivedAsync_WithInvalidTimePeriod_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Messages\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var whatsApp = new WhatsAppApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await whatsApp.ReceivedAsync(timePeriod: 0);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void Received_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Messages\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var whatsApp = new WhatsAppApi(new TNZApiUser { AuthToken = "test-token" });

        var result = whatsApp.Received(timePeriod: 60);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task ReceivedAsync_WithDateFromAndDateTo_BuildsCorrectURL()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Messages\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var whatsApp = new WhatsAppApi(new TNZApiUser { AuthToken = "test-token" });

        await whatsApp.ReceivedAsync(dateFrom: new DateTime(2026, 1, 1), dateTo: new DateTime(2026, 1, 2));

        var url = fakeHandler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("dateFrom=", url);
        Assert.Contains("dateTo=", url);
        Assert.DoesNotContain("timePeriod=", url);
    }

    [Fact]
    public async Task ReceivedAsync_WithOnlyDateFrom_DefaultsDateToToNow()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Messages\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var whatsApp = new WhatsAppApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await whatsApp.ReceivedAsync(dateFrom: new DateTime(2026, 1, 1));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        var url = fakeHandler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("dateFrom=", url);
        Assert.Contains("dateTo=", url);
    }

    [Fact]
    public async Task ReceivedAsync_WithDateFromAfterDateTo_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Messages\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var whatsApp = new WhatsAppApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await whatsApp.ReceivedAsync(dateFrom: new DateTime(2026, 1, 2), dateTo: new DateTime(2026, 1, 1));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }
}