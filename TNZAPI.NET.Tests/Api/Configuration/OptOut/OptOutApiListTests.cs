using System.Net;
using TNZAPI.NET.Api.Configuration.OptOut;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Configuration.OptOut;

public class OptOutApiListTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task ListAsync_GetsCorrectUrlWithQueryParams()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"OptOuts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        await optOut.ListAsync(timePeriod: 5, destType: "SMS", page: 2, recordsPerPage: 50);

        var url = fakeHandler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("https://api.tnz.co.nz/api/v3.00/optout/list?", url);
        Assert.Contains("timePeriod=5", url);
        Assert.Contains("destType=SMS", url);
        Assert.Contains("page=2", url);
        Assert.Contains("recordsPerPage=50", url);
    }

    [Fact]
    public async Task ListAsync_WithEnumDestType_GetsCorrectUrlWithQueryParams()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"OptOuts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        await optOut.ListAsync(Enums.OptOutDestType.SMS, timePeriod: 5, page: 2, recordsPerPage: 50);

        var url = fakeHandler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("timePeriod=5", url);
        Assert.Contains("destType=SMS", url);
        Assert.Contains("page=2", url);
        Assert.Contains("recordsPerPage=50", url);
    }

    [Fact]
    public void List_Sync_WithEnumDestType_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"OptOuts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = optOut.List(Enums.OptOutDestType.Email);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Contains("destType=Email", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ListAsync_UsesDefaultsWhenNotSpecified()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"OptOuts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        await optOut.ListAsync();

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/optout/list?page=1&recordsPerPage=100",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ListAsync_ParsesOptOuts()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"OptOuts\":[{\"DestType\":\"SMS\"}]}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.ListAsync();

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Single(result.OptOuts);
        Assert.Equal("SMS", result.OptOuts[0].DestType);
    }

    [Fact]
    public async Task DetailsAsync_GetsCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"DestType\":\"SMS\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        await optOut.DetailsAsync(new OptOutID("8000000a-f002-4007-b00a-d00000000001"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/optout/8000000a-f002-4007-b00a-d00000000001",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task DetailsAsync_On404_ReturnsRecordNotFound()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, "{\"ErrorMessage\":[\"Not found\"]}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.DetailsAsync(new OptOutID("8000000a-f002-4007-b00a-d00000000001"));

        Assert.Equal(Enums.ResultCode.RecordNotFound, result.Result);
    }

    [Fact]
    public async Task DetailsAsync_WithEmptyOptOutID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.DetailsAsync(new OptOutID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void List_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"OptOuts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = optOut.List();

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}