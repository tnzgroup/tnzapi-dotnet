using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Configuration.OptOut;
using TNZAPI.NET.Api.Configuration.OptOut.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Configuration.OptOut;

public class OptOutApiCreateTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task CreateAsync_PostsToCorrectUrlWithModelFields()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"ID\":\"8000000a-f002-4007-b00a-d00000000001\",\"DestType\":\"SMS\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.CreateAsync(new OptOutModel { DestType = "SMS", Destination = "+6421003004" });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/optout", fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("SMS", doc.RootElement.GetProperty("DestType").GetString());
        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(result.ID);
        Assert.Equal("8000000a-f002-4007-b00a-d00000000001", result.ID);
    }

    [Fact]
    public async Task CreateAsync_WithNoDestType_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.CreateAsync(new OptOutModel { Destination = "+6421003004" });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task CreateAsync_WithNoDestinationOrContactID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.CreateAsync(new OptOutModel { DestType = "SMS" });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task CreateAsync_WithContactIDInsteadOfDestination_Succeeds()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"ID\":\"8000000a-f002-4007-b00a-d00000000001\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.CreateAsync(new OptOutModel
        {
            DestType = "SMS",
            ContactID = new TNZAPI.NET.Api.Messaging.Common.Dto.ContactID("123e4567-e89b-12d3-a456-426614174000")
        });

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task CreateBatchAsync_PostsToCorrectUrlWithDestinationsArray()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Data\":[{\"ID\":\"8000000a-f002-4007-b00a-d00000000001\"}]}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        await optOut.CreateBatchAsync(new OptOutBatchModel
        {
            DestType = "SMS,Email",
            Destinations = new List<string> { "+6421003004", "john.doe@example.com" }
        });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/optout/batch", fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal(2, doc.RootElement.GetProperty("Destinations").GetArrayLength());
    }

    [Fact]
    public async Task CreateBatchAsync_ParsesDataArrayIntoOptOuts()
    {
        var body = "{\"Data\":[{\"ID\":\"8000000a-f002-4007-b00a-d00000000001\",\"DestType\":\"Text\",\"Destination\":\"+6421003004\"},{\"ID\":\"9000000a-f002-4007-b00a-d00000000002\",\"DestType\":\"Text\",\"Destination\":\"+6421003005\"}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.CreateBatchAsync(new OptOutBatchModel
        {
            DestType = "SMS",
            Destinations = new List<string> { "+6421003004", "+6421003005" }
        });

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal(2, result.OptOuts.Count);
        var firstOptOut = result.OptOuts[0];
        Assert.NotNull(firstOptOut.ID);
        Assert.Equal("8000000a-f002-4007-b00a-d00000000001", firstOptOut.ID);
        Assert.Equal("+6421003004", firstOptOut.Destination);
        var secondOptOut = result.OptOuts[1];
        Assert.NotNull(secondOptOut.ID);
        Assert.Equal("9000000a-f002-4007-b00a-d00000000002", secondOptOut.ID);
    }

    [Fact]
    public async Task CreateBatchAsync_WithNoDestType_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await optOut.CreateBatchAsync(new OptOutBatchModel { Destination = "+6421003004" });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void Create_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"ID\":\"8000000a-f002-4007-b00a-d00000000001\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var optOut = new OptOutApi(new TNZApiUser { AuthToken = "test-token" });

        var result = optOut.Create(new OptOutModel { DestType = "SMS", Destination = "+6421003004" });

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}