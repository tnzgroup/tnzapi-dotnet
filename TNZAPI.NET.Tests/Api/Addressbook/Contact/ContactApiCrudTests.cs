using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Addressbook.Contact;

public class ContactApiCrudTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task CreateAsync_PostsToCorrectUrlWithModelFields()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"ContactID\":\"123e4567-e89b-12d3-a456-426614174000\",\"FirstName\":\"John\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contact.CreateAsync(new ContactModel { FirstName = "John", EmailAddress = "john.doe@example.com" });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact", fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Post, fakeHandler.LastRequest!.Method);
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("John", doc.RootElement.GetProperty("FirstName").GetString());
        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(result.ContactID);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", result.ContactID);
    }

    [Fact]
    public async Task DetailsAsync_GetsCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"FirstName\":\"John\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        await contact.DetailsAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/123e4567-e89b-12d3-a456-426614174000", fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Get, fakeHandler.LastRequest!.Method);
    }

    [Fact]
    public async Task DetailsAsync_On404_ReturnsRecordNotFound()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, "{\"ErrorMessage\":[\"Not found\"]}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contact.DetailsAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal(Enums.ResultCode.RecordNotFound, result.Result);
    }

    [Fact]
    public async Task DetailsAsync_WithEmptyContactID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contact.DetailsAsync(new ContactID(""));

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task UpdateAsync_PatchesCorrectUrlWithModelFields()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"FirstName\":\"Jane\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contact.UpdateAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"), new ContactModel { FirstName = "Jane" });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/123e4567-e89b-12d3-a456-426614174000", fakeHandler.LastRequest!.RequestUri!.ToString());
        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("Jane", doc.RootElement.GetProperty("FirstName").GetString());
        Assert.Equal("Jane", result.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_DeletesCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"FirstName\":\"John\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contact.DeleteAsync(new ContactID("123e4567-e89b-12d3-a456-426614174000"));

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/123e4567-e89b-12d3-a456-426614174000", fakeHandler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Delete, fakeHandler.LastRequest!.Method);
        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public void Create_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"FirstName\":\"John\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = contact.Create(new ContactModel { FirstName = "John" });

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}