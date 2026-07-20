using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Configuration.OptOut;
using TNZAPI.NET.Api.Configuration.OptOut.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Configuration.OptOut;

public class OptOutApiUpdateDeleteTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task UpdateAsync_PatchesCorrectUrlWithModelFields()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Notes\":\"Updated via API\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.UpdateAsync(
            new OptOutID("8000000a-f002-4007-b00a-d00000000001"),
            new OptOutModel { DestType = "SMS", Destination = "+6421003004", Notes = "Updated via API" });

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/optout/8000000a-f002-4007-b00a-d00000000001",
            fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("Updated via API", doc.RootElement.GetProperty("Notes").GetString());
        Assert.Equal("Updated via API", result.Notes);
    }

    [Fact]
    public async Task UpdateAsync_WithEmptyOptOutID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.UpdateAsync(new OptOutID(""), new OptOutModel { DestType = "SMS", Destination = "+6421003004" });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task DeleteAsync_DeletesCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"DestType\":\"SMS\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.DeleteAsync(new OptOutID("8000000a-f002-4007-b00a-d00000000001"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/optout/8000000a-f002-4007-b00a-d00000000001",
            fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Delete, fakeHandler.LastRequest!.Method);
        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task DeleteAsync_WithEmptyOptOutID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.DeleteAsync(new OptOutID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void Update_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Notes\":\"Updated\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = optOut.Update(new OptOutID("8000000a-f002-4007-b00a-d00000000001"), new OptOutModel { DestType = "SMS", Destination = "+6421003004" });

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}