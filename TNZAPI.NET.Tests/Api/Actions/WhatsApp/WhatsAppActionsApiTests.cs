using System.Net;
using TNZAPI.NET.Api.Actions.WhatsApp;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Actions.WhatsApp;

public class WhatsAppActionsApiTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task RescheduleAsync_DelegatesToWhatsAppApiReschedule()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Reschedule\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var actions = new WhatsAppActionsApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await actions.RescheduleAsync(new MessageID("abc-123"), DateTime.Now);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/whatsapp/abc-123/reschedule", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task AbortAsync_DelegatesToWhatsAppApiAbort()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Abort\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var actions = new WhatsAppActionsApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await actions.AbortAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/whatsapp/abc-123/abort", fakeHandler.LastRequest!.RequestUri!.ToString());
    }
}