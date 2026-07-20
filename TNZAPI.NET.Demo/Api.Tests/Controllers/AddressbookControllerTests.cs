using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

// Covers AddressbookController — Contacts CRUD (client.Addressbook.Contact).
public class AddressbookControllerTests : DemoApiTestBase
{
    public AddressbookControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task List_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"Contacts\":[]}");

        var response = await Client.GetAsync("/api/addressbook/contacts?page=2&recordsPerPage=25");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/list?page=2&recordsPerPage=25", handler.LastRequest!.RequestUri!.ToString());
    }

    [Theory]
    [InlineData("page=0&recordsPerPage=100")]
    [InlineData("page=1&recordsPerPage=0")]
    [InlineData("page=1&recordsPerPage=1001")]
    public async Task List_WithInvalidPagination_ReturnsBadRequestWithoutCallingSdk(string query)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"Contacts\":[]}");

        var response = await Client.GetAsync($"/api/addressbook/contacts?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Create_OnSuccess_Returns200WithFullResult()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ContactID\":\"contact-1\",\"FirstName\":\"Jane\"}");

        var response = await Client.PostAsJsonAsync("/api/addressbook/contacts", new { FirstName = "Jane", LastName = "Doe" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact", handler.LastRequest!.RequestUri!.ToString());
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("contact-1", body.GetProperty("ContactID").GetString());
    }

    [Fact]
    public async Task Details_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ContactID\":\"contact-1\"}");

        var response = await Client.GetAsync("/api/addressbook/contacts/contact-1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/contact-1", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Update_UsesPatchAndCorrectUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ContactID\":\"contact-1\",\"FirstName\":\"Updated\"}");

        var response = await Client.PutAsJsonAsync("/api/addressbook/contacts/contact-1", new { FirstName = "Updated" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/contact-1", handler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Patch, handler.LastRequest.Method);
    }

    [Fact]
    public async Task Update_WithFieldOmitted_PreservesExistingValueInsteadOfClearingIt()
    {
        // ContactModel has no way to represent "leave unchanged" on the wire (its string properties
        // are non-nullable), so Update must fetch the current record first and fall back to those
        // values for any field the caller omitted — otherwise an omitted field like City would be
        // sent as "" and silently clear it server-side.
        var handler = new SequencedFakeHttpMessageHandler(
            (HttpStatusCode.OK, "{\"ContactID\":\"contact-1\",\"FirstName\":\"Jane\",\"City\":\"Auckland\"}"),
            (HttpStatusCode.OK, "{\"ContactID\":\"contact-1\",\"FirstName\":\"Updated\",\"City\":\"Auckland\"}"));
        SetHandler(handler);

        var response = await Client.PutAsJsonAsync("/api/addressbook/contacts/contact-1", new { FirstName = "Updated" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Patch, handler.Requests[1].Method);
        Assert.Contains("\"City\":\"Auckland\"", handler.RequestBodies[1]);
    }

    [Fact]
    public async Task Update_WhenDetailsLookupFails_ReturnsFailureWithoutCallingUpdate()
    {
        var handler = new SequencedFakeHttpMessageHandler(
            (HttpStatusCode.NotFound, "{\"Result\":\"RecordNotFound\",\"ErrorMessage\":[\"Not found\"]}"));
        SetHandler(handler);

        var response = await Client.PutAsJsonAsync("/api/addressbook/contacts/contact-1", new { FirstName = "Updated" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
    }

    [Fact]
    public async Task Delete_UsesCorrectUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"ContactID\":\"contact-1\"}");

        var response = await Client.DeleteAsync("/api/addressbook/contacts/contact-1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/contact-1", handler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Delete, handler.LastRequest.Method);
    }

    [Fact]
    public async Task Create_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        var response = await Client.PostAsJsonAsync("/api/addressbook/contacts", new { FirstName = "Jane" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("Unauthorized", body.GetProperty("Result").GetString());
    }
}