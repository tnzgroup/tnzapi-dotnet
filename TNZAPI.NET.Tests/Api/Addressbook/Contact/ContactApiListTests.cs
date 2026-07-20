using System.Net;
using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Addressbook.Contact;

public class ContactApiListTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task ListAsync_GetsCorrectUrlWithQueryParams()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Contacts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        await contact.ListAsync(page: 2, recordsPerPage: 50);

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/contact/list?page=2&recordsPerPage=50",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ListAsync_UsesDefaultsWhenNotSpecified()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Contacts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        await contact.ListAsync();

        Assert.Equal(
            "https://api.tnz.co.nz/api/v3.00/addressbook/contact/list?page=1&recordsPerPage=100",
            fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task ListAsync_ParsesContacts()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Contacts\":[{\"FirstName\":\"John\"}]}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await contact.ListAsync();

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Single(result.Contacts);
        Assert.Equal("John", result.Contacts[0].FirstName);
    }

    [Fact]
    public async Task SearchAsync_GetsCorrectUrlWithQueryParams()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Contacts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        await contact.SearchAsync(emailAddress: "john.doe@example.com", firstName: "John");

        var url = fakeHandler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("https://api.tnz.co.nz/api/v3.00/addressbook/contact/search?", url);
        Assert.Contains("EmailAddress=john.doe%40example.com", url);
        Assert.Contains("FirstName=John", url);
    }

    [Fact]
    public async Task SearchAsync_OmitsUnspecifiedParams()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Contacts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        await contact.SearchAsync(lastName: "Doe");

        var url = fakeHandler.LastRequest!.RequestUri!.ToString();
        Assert.Contains("LastName=Doe", url);
        Assert.DoesNotContain("EmailAddress", url);
        Assert.DoesNotContain("FirstName", url);
    }

    [Fact]
    public void List_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"Contacts\":[]}");
        HttpRequest.MessageHandler = fakeHandler;
        var contact = new ContactApi(new TNZApiUser { AuthToken = "test-token" });

        var result = contact.List();

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }
}