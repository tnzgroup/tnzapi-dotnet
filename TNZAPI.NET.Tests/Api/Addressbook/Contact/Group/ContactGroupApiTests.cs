using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Addressbook.Contact.Group;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Addressbook.Contact.Group;

public class ContactGroupApiTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task ListAsync_GetsCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Groups\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var contactGroup = new ContactGroupApi(new TNZApiUser { AuthToken = "test-token" });

        await contactGroup.ListAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/contact/123e4567-e89b-12d3-a456-426614174000/group/list",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ListAsync_ParsesContactAndGroups()
    {
        var body = "{\"Contact\":{\"FirstName\":\"John\"},\"Groups\":[{\"GroupName\":\"VIP\"}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var contactGroup = new ContactGroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contactGroup.ListAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(result.Contact);
        Assert.Equal("John", result.Contact.FirstName);
        Assert.Single(result.Groups);
        Assert.Equal("VIP", result.Groups[0].GroupName);
    }

    [Fact]
    public async Task AddAsync_PatchesCorrectUrlWithGroupIDBody()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Group\":{\"GroupName\":\"VIP\"},\"Contact\":{\"FirstName\":\"John\"}}");
        HttpRequest.MessageHandler = fakeHandler;
        var contactGroup = new ContactGroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contactGroup.AddAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"), new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/contact/123e4567-e89b-12d3-a456-426614174000/group",
            fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("223e4567-e89b-12d3-a456-426614175000", doc.RootElement.GetProperty("GroupID").GetString());
        Assert.NotNull(result.Group);
        Assert.Equal("VIP", result.Group.GroupName);
        Assert.NotNull(result.Contact);
        Assert.Equal("John", result.Contact.FirstName);
    }

    [Fact]
    public async Task RemoveAsync_DeletesCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Group\":{\"GroupName\":\"VIP\"},\"Contact\":{\"FirstName\":\"John\"}}");
        HttpRequest.MessageHandler = fakeHandler;
        var contactGroup = new ContactGroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contactGroup.RemoveAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"), new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/contact/123e4567-e89b-12d3-a456-426614174000/group/223e4567-e89b-12d3-a456-426614175000",
            fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Delete, fakeHandler.LastRequest!.Method);
        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task AddAsync_WithEmptyContactID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var contactGroup = new ContactGroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contactGroup.AddAsync(new ContactID(""), new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task DetailAsync_GetsCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Group\":{\"GroupName\":\"VIP\"},\"Contact\":{\"FirstName\":\"John\"}}");
        HttpRequest.MessageHandler = fakeHandler;
        var contactGroup = new ContactGroupApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contactGroup.DetailAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"), new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/contact/123e4567-e89b-12d3-a456-426614174000/group/223e4567-e89b-12d3-a456-426614175000",
            fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Get, fakeHandler.LastRequest!.Method);
        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(result.Group);
        Assert.Equal("VIP", result.Group.GroupName);
    }
}