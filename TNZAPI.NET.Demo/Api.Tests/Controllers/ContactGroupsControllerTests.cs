using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

// Covers ContactGroupsController — client.Addressbook.Contact.Group (Add/List/Remove).
public class ContactGroupsControllerTests : DemoApiTestBase
{
    public ContactGroupsControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task List_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"Groups\":[]}");

        var response = await Client.GetAsync("/api/addressbook/contact-groups?contactID=contact-1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/contact-1/group/list", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Add_UsesCorrectUrlAndPatchVerb()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"Group\":{\"GroupID\":\"group-1\"}}");

        var response = await Client.PostAsJsonAsync("/api/addressbook/contact-groups", new { ContactID = "contact-1", GroupID = "group-1" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/contact-1/group", handler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Patch, handler.LastRequest.Method);
    }

    [Fact]
    public async Task Remove_UsesCorrectUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{}");

        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/addressbook/contact-groups")
        {
            Content = new StringContent(JsonSerializer.Serialize(new { ContactID = "contact-1", GroupID = "group-1" }), Encoding.UTF8, "application/json"),
        };
        var response = await Client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/contact/contact-1/group/group-1", handler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Delete, handler.LastRequest.Method);
    }

    [Fact]
    public async Task Add_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        var response = await Client.PostAsJsonAsync("/api/addressbook/contact-groups", new { ContactID = "contact-1", GroupID = "group-1" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}