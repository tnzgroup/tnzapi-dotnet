using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Addressbook.Group.Contact;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Addressbook.Group.Contact;

public class GroupContactApiTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task ListAsync_GetsCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Contacts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        await groupContact.ListAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/group/223e4567-e89b-12d3-a456-426614175000/contact/list",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ListAsync_ParsesGroupAndContacts()
    {
        var body = "{\"Group\":{\"GroupName\":\"VIP\"},\"Contacts\":[{\"FirstName\":\"John\"}]}";
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, body);
        HttpRequest.MessageHandler = fakeHandler;
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await groupContact.ListAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(result.Group);
        Assert.Equal("VIP", result.Group.GroupName);
        Assert.Single(result.Contacts);
        Assert.Equal("John", result.Contacts[0].FirstName);
    }

    [Fact]
    public async Task ListAsync_WithEmptyGroupID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await groupContact.ListAsync(new GroupID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void List_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Contacts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = groupContact.List(new GroupID("223e4567-e89b-12d3-a456-426614175000"));

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task AddAsync_PatchesCorrectUrlWithContactIDBody()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Group\":{\"GroupName\":\"VIP\"},\"Contact\":{\"FirstName\":\"John\"}}");
        HttpRequest.MessageHandler = fakeHandler;
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await groupContact.AddAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"), new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/group/223e4567-e89b-12d3-a456-426614175000/contact",
            fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", doc.RootElement.GetProperty("ContactID").GetString());
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
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await groupContact.RemoveAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"), new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/group/223e4567-e89b-12d3-a456-426614175000/contact/123e4567-e89b-12d3-a456-426614174000",
            fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Delete, fakeHandler.LastRequest!.Method);
        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task DetailAsync_GetsCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Group\":{\"GroupName\":\"VIP\"},\"Contact\":{\"FirstName\":\"John\"}}");
        HttpRequest.MessageHandler = fakeHandler;
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await groupContact.DetailAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"), new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/group/223e4567-e89b-12d3-a456-426614175000/contact/123e4567-e89b-12d3-a456-426614174000",
            fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Get, fakeHandler.LastRequest!.Method);
        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(result.Group);
        Assert.Equal("VIP", result.Group.GroupName);
    }

    [Fact]
    public async Task AddAsync_WithEmptyGroupID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await groupContact.AddAsync(new GroupID(""), new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task AddAsync_WithEmptyContactID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var groupContact = new GroupContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await groupContact.AddAsync(new GroupID("223e4567-e89b-12d3-a456-426614175000"), new ContactID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }
}