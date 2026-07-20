using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Workflow;

namespace TNZAPI.NET.Tests.Api.Messaging.Workflow.Dto;

public class WorkflowRequestBodyJsonTests
{
    [Fact]
    public void WorkflowDestinationBody_SerializesToNumberMainPhoneAndEmailAddressTogether()
    {
        var destination = new WorkflowDestinationBody
        {
            ToNumber = "+6421000001",
            MainPhone = "+6421000001",
            EmailAddress = "john.doe@example.com",
            FirstName = "John"
        };

        var json = JsonSerializer.Serialize(destination);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("+6421000001", doc.RootElement.GetProperty("ToNumber").GetString());
        Assert.Equal("+6421000001", doc.RootElement.GetProperty("MainPhone").GetString());
        Assert.Equal("john.doe@example.com", doc.RootElement.GetProperty("EmailAddress").GetString());
        Assert.Equal("John", doc.RootElement.GetProperty("FirstName").GetString());
    }

    [Fact]
    public void WorkflowRequestBody_SerializesWorkflowTemplateIDField()
    {
        var body = new WorkflowRequestBody
        {
            WorkflowTemplateID = "a1b2c3d4-e5f6-7890-1234-567890abcdef"
        };

        var json = JsonSerializer.Serialize(body);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("a1b2c3d4-e5f6-7890-1234-567890abcdef", doc.RootElement.GetProperty("WorkflowTemplateID").GetString());
    }
}