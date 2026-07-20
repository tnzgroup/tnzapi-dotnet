using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Workflow;
using TNZAPI.NET.Api.Messaging.Workflow.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.Workflow;

public class WorkflowApiSendTests : IDisposable
{
    private const string TestWorkflowTemplateID = "a1b2c3d4-e5f6-7890-1234-567890abcdef";

    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task SendMessageAsync_PostsToCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(new WorkflowModel
        {
            WorkflowTemplateID = TestWorkflowTemplateID,
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/workflow", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsRecipientToAllThreeDestinationFields()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(new WorkflowModel
        {
            WorkflowTemplateID = TestWorkflowTemplateID,
            Recipients = new List<Recipient>
            {
                new Recipient
                {
                    MobileNumber = "+64211234567",
                    PhoneNumber = "+6495006000",
                    EmailAddress = "john.doe@example.com"
                }
            }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+64211234567", destination.GetProperty("ToNumber").GetString());
        Assert.Equal("+6495006000", destination.GetProperty("MainPhone").GetString());
        Assert.Equal("john.doe@example.com", destination.GetProperty("EmailAddress").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_IncludesWorkflowTemplateIDInBody()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(new WorkflowModel
        {
            WorkflowTemplateID = TestWorkflowTemplateID,
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal(TestWorkflowTemplateID, doc.RootElement.GetProperty("WorkflowTemplateID").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_ReturnsMessageIDFromResponse()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await workflow.SendMessageAsync(new WorkflowModel
        {
            WorkflowTemplateID = TestWorkflowTemplateID,
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(result.MessageID);
        Assert.Equal("abc-123", result.MessageID);
    }

    [Fact]
    public async Task SendMessageAsync_WithEmptyRecipients_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await workflow.SendMessageAsync(new WorkflowModel { WorkflowTemplateID = TestWorkflowTemplateID });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task SendMessageAsync_WithNoWorkflowTemplateID_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await workflow.SendMessageAsync(new WorkflowModel
        {
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void SendMessage_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        var result = workflow.SendMessage(new WorkflowModel
        {
            WorkflowTemplateID = TestWorkflowTemplateID,
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(result.MessageID);
        Assert.Equal("abc-123", result.MessageID);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_BuildsCorrectModel()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            destination: "+64211234567",
            sendMode: Enums.SendModeType.Live
        );

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithToNumberMainPhoneAndEmailAddress_MapsAllThreeToSameDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            toNumber: "+64211234567",
            mainPhone: "+6495006000",
            emailAddress: "john.doe@example.com",
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(1, destinations.GetArrayLength());
        var destination = destinations[0];
        Assert.Equal("+64211234567", destination.GetProperty("ToNumber").GetString());
        Assert.Equal("+6495006000", destination.GetProperty("MainPhone").GetString());
        Assert.Equal("john.doe@example.com", destination.GetProperty("EmailAddress").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithMultiValuedToNumberAndSingleValuedMainPhone_BroadcastsMainPhoneToEveryDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            toNumber: "+64211111111,+64222222222",
            mainPhone: "+6493334444",
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        Assert.Equal("+64211111111", destinations[0].GetProperty("ToNumber").GetString());
        Assert.Equal("+6493334444", destinations[0].GetProperty("MainPhone").GetString());
        Assert.Equal("+64222222222", destinations[1].GetProperty("ToNumber").GetString());
        Assert.Equal("+6493334444", destinations[1].GetProperty("MainPhone").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithEqualLengthMultiValuedToNumberAndMainPhone_ZipsThemByPosition()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            toNumber: "+64211111111,+64222222222",
            mainPhone: "+6493334444,+6493335555",
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        Assert.Equal("+64211111111", destinations[0].GetProperty("ToNumber").GetString());
        Assert.Equal("+6493334444", destinations[0].GetProperty("MainPhone").GetString());
        Assert.Equal("+64222222222", destinations[1].GetProperty("ToNumber").GetString());
        Assert.Equal("+6493335555", destinations[1].GetProperty("MainPhone").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithMismatchedMultiValuedCounts_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            toNumber: "+64211111111,+64222222222,+64233333333",
            mainPhone: "+6493334444,+6493335555",
            sendMode: Enums.SendModeType.Live
        );

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void SendMessage_NamedParamsOverload_WithMismatchedMultiValuedCounts_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        var result = workflow.SendMessage(
            workflowTemplateId: TestWorkflowTemplateID,
            toNumber: "+64211111111,+64222222222,+64233333333",
            mainPhone: "+6493334444,+6493335555",
            sendMode: Enums.SendModeType.Live
        );

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithContactIDAndGroupID_AddsBothToDestinations()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            contactID: "CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD",
            groupID: "GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD",
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        Assert.Equal("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD", destinations[0].GetProperty("ContactID").GetString());
        Assert.Equal("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD", destinations[1].GetProperty("GroupID").GetString());
    }

    [Fact]
    public void SendMessage_WithCommaSeparatedToNumber_CreatesMultipleRecipients()
    {
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;

        workflow.SendMessage(workflowTemplateId: TestWorkflowTemplateID, toNumber: "+64211111111,+64222222222", sendMode: Enums.SendModeType.Test);

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal(2, doc.RootElement.GetProperty("Destinations").GetArrayLength());
    }

    [Fact]
    public async Task SendMessageAsync_MapsAllNineCustomFieldsToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(new WorkflowModel
        {
            WorkflowTemplateID = TestWorkflowTemplateID,
            Recipients = new List<Recipient> { new Recipient { MobileNumber = "+64211111111", Custom5 = "c5", Custom9 = "c9" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("c5", destination.GetProperty("Custom5").GetString());
        Assert.Equal("c9", destination.GetProperty("Custom9").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_WithDestinations_SendsRecipientFieldOnWire()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(new WorkflowModel
        {
            WorkflowTemplateID = TestWorkflowTemplateID,
            Destinations = new List<Destination> { new Destination { Recipient = "+64211234567", FirstName = "John" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+64211234567", destination.GetProperty("Recipient").GetString());
        Assert.Equal("John", destination.GetProperty("FirstName").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_SetsNotificationTypeOnBuiltModel()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            destination: "+64211111111",
            notificationType: Enums.NotificationType.Webhook
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("Webhook", doc.RootElement.GetProperty("NotificationType").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithDestinationObject_SendsRecipientFieldOnWire()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            destination: new Destination { Recipient = "+64211111111", FirstName = "John" }
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(1, destinations.GetArrayLength());
        Assert.Equal("+64211111111", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("John", destinations[0].GetProperty("FirstName").GetString());
        // Guards against double-delivery: a Destination-only send must not also populate Recipients.
        Assert.False(doc.RootElement.TryGetProperty("Recipients", out var recipients) && recipients.GetArrayLength() > 0);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithUnsupportedDestinationType_Throws()
    {
        var workflow = new WorkflowApi(new TNZApiUser { AuthToken = "test-token" });

        await Assert.ThrowsAsync<ArgumentException>(() => workflow.SendMessageAsync(
            workflowTemplateId: TestWorkflowTemplateID,
            destination: 12345
        ));
    }
}