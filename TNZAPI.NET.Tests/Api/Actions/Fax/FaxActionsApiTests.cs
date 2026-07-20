using System.Net;
using TNZAPI.NET.Api.Actions.Fax;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Actions.Fax;

public class FaxActionsApiTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task RescheduleAsync_DelegatesToFaxApiReschedule()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Reschedule\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var actions = new FaxActionsApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await actions.RescheduleAsync(new MessageID("abc-123"), DateTime.Now);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/fax/abc-123/reschedule", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task AbortAsync_DelegatesToFaxApiAbort()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Abort\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var actions = new FaxActionsApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await actions.AbortAsync(new MessageID("abc-123"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/fax/abc-123/abort", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ResubmitAsync_DelegatesToFaxApiResubmit()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Action\":\"Resubmit\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var actions = new FaxActionsApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await actions.ResubmitAsync(new MessageID("abc-123"), DateTime.Now);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/fax/abc-123/resubmit", fakeHandler.LastRequest!.RequestUri!.ToString());
    }
}