using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

// Covers AddressbookGroupsController — Groups CRUD (client.Addressbook.Group).
public class AddressbookGroupsControllerTests : DemoApiTestBase
{
    public AddressbookGroupsControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task List_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"Groups\":[]}");

        var response = await Client.GetAsync("/api/addressbook/groups?page=1&recordsPerPage=50");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/group/list?page=1&recordsPerPage=50", handler.LastRequest!.RequestUri!.ToString());
    }

    [Theory]
    [InlineData("page=-1&recordsPerPage=50")]
    [InlineData("page=1&recordsPerPage=0")]
    [InlineData("page=1&recordsPerPage=1001")]
    public async Task List_WithInvalidPagination_ReturnsBadRequestWithoutCallingSdk(string query)
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"Groups\":[]}");

        var response = await Client.GetAsync($"/api/addressbook/groups?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Create_OnSuccess_Returns200WithFullResult()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"GroupID\":\"group-1\",\"GroupName\":\"Test Group\"}");

        var response = await Client.PostAsJsonAsync("/api/addressbook/groups", new { GroupName = "Test Group" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/group", handler.LastRequest!.RequestUri!.ToString());
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("group-1", body.GetProperty("GroupID").GetString());
    }

    [Fact]
    public async Task Details_GetsCorrectSdkUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"GroupID\":\"group-1\"}");

        var response = await Client.GetAsync("/api/addressbook/groups/group-1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/group/group-1", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Update_UsesCorrectUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"GroupID\":\"group-1\",\"GroupName\":\"Updated\"}");

        var response = await Client.PutAsJsonAsync("/api/addressbook/groups/group-1", new { GroupName = "Updated" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/addressbook/group/group-1", handler.LastRequest!.RequestUri!.ToString());
        Assert.Equal(HttpMethod.Patch, handler.LastRequest.Method);
    }

    [Fact]
    public async Task Update_WithFieldOmitted_PreservesExistingValueInsteadOfClearingIt()
    {
        // GroupModel has no way to represent "leave unchanged" on the wire (its string properties
        // are non-nullable), so Update must fetch the current record first and fall back to those
        // values for any field the caller omitted.
        var handler = new SequencedFakeHttpMessageHandler(
            (HttpStatusCode.OK, "{\"GroupID\":\"group-1\",\"GroupName\":\"Test Group\",\"Department\":\"Sales\"}"),
            (HttpStatusCode.OK, "{\"GroupID\":\"group-1\",\"GroupName\":\"Updated\",\"Department\":\"Sales\"}"));
        SetHandler(handler);

        var response = await Client.PutAsJsonAsync("/api/addressbook/groups/group-1", new { GroupName = "Updated" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Contains("\"Department\":\"Sales\"", handler.RequestBodies[1]);
    }

    [Fact]
    public async Task Update_WhenDetailsLookupFails_ReturnsFailureWithoutCallingUpdate()
    {
        var handler = new SequencedFakeHttpMessageHandler(
            (HttpStatusCode.NotFound, "{\"Result\":\"RecordNotFound\",\"ErrorMessage\":[\"Not found\"]}"));
        SetHandler(handler);

        var response = await Client.PutAsJsonAsync("/api/addressbook/groups/group-1", new { GroupName = "Updated" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Delete_UsesCorrectUrl()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"GroupID\":\"group-1\"}");

        var response = await Client.DeleteAsync("/api/addressbook/groups/group-1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpMethod.Delete, handler.LastRequest!.Method);
    }

    [Fact]
    public async Task Create_WithViewEditByEnumValue_PassesItThrough()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"GroupID\":\"group-1\"}");

        var response = await Client.PostAsJsonAsync("/api/addressbook/groups", new { GroupName = "Test Group", ViewEditBy = "Account" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("\"ViewEditBy\":\"Account\"", handler.LastRequestBody);
    }

    [Fact]
    public async Task Create_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        var response = await Client.PostAsJsonAsync("/api/addressbook/groups", new { GroupName = "Test Group" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}