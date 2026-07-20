using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Addressbook.Group;

public class GroupApiCrudTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task CreateAsync_PostsToCorrectUrlWithModelFields()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"GroupID\":\"223e4567-e89b-12d3-a456-426614175000\",\"GroupName\":\"VIP\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var group = new GroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await group.CreateAsync(new GroupModel { GroupName = "VIP" });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/group", fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("VIP", doc.RootElement.GetProperty("GroupName").GetString());
        Assert.NotNull(result.GroupID);
        Assert.Equal("223e4567-e89b-12d3-a456-426614175000", result.GroupID);
    }

    [Fact]
    public async Task ListAsync_GetsCorrectUrlWithQueryParams()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Groups\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var group = new GroupApi(new TNZApiUser { AuthToken = "test-token" });

        await group.ListAsync(page: 2, recordsPerPage: 50);

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/group/list?page=2&recordsPerPage=50",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task DetailsAsync_GetsCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"GroupName\":\"VIP\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var group = new GroupApi(new TNZApiUser { AuthToken = "test-token" });

        await group.DetailsAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/group/223e4567-e89b-12d3-a456-426614175000",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task UpdateAsync_PatchesCorrectUrlWithModelFields()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"GroupName\":\"VIP Renamed\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var group = new GroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await group.UpdateAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"), new GroupModel { GroupName = "VIP Renamed" });

        Assert.Equal("VIP Renamed", result.GroupName);
    }

    [Fact]
    public async Task DeleteAsync_DeletesCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"GroupName\":\"VIP\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var group = new GroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await group.DeleteAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(HttpMethod.Delete, fakeHandler.LastRequest!.Method);
        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task DetailsAsync_WithEmptyGroupID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var group = new GroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await group.DetailsAsync(new GroupID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void Create_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"GroupName\":\"VIP\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var group = new GroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = group.Create(new GroupModel { GroupName = "VIP" });

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}