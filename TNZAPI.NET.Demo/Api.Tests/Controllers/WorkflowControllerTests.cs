using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TNZAPI.NET.Demo.Api.Tests.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Controllers;

// Workflow has no Status/Received/Actions endpoint — only Send (docs/workflow.md).
public class WorkflowControllerTests : DemoApiTestBase
{
    public WorkflowControllerTests(DemoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Send_OnSuccess_Returns200WithFullResult()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/workflow/send", new { WorkflowTemplateId = "tmpl-1", ToNumber = "+64211234567" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("abc-123", body.GetProperty("MessageID").GetString());
        Assert.Equal("https://api.tnz.co.nz/api/v3.00/workflow", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task Send_WithUnparseableSendTime_ReturnsBadRequestWithoutCallingSdk()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/workflow/send", new { WorkflowTemplateId = "tmpl-1", ToNumber = "+64211234567", SendTime = "not-a-date" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Send_WithContactIdsAndGroupIds_SplitsCommaSeparatedValues()
    {
        var handler = FakeResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"JobStatus\":\"Pending\"}");

        var response = await Client.PostAsJsonAsync("/api/workflow/send", new
        {
            WorkflowTemplateId = "tmpl-1",
            ContactIds = "contact-1, contact-2",
            GroupIds = "group-1",
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("contact-1", handler.LastRequestBody);
        Assert.Contains("contact-2", handler.LastRequestBody);
        Assert.Contains("group-1", handler.LastRequestBody);
    }

    [Fact]
    public async Task Send_OnUnauthorized_Returns400WithTrimmedResult()
    {
        FakeResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Access denied\"]}");

        var response = await Client.PostAsJsonAsync("/api/workflow/send", new { WorkflowTemplateId = "tmpl-1", ToNumber = "+64211234567" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Assert.Equal("Unauthorized", body.GetProperty("Result").GetString());
    }
}