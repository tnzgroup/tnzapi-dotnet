using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Email;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.Email;

public class EmailApiSendTests : IDisposable
{
    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
    }

    [Fact]
    public async Task SendMessageAsync_PostsToCorrectUrl()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(new EmailModel
        {
            MessagePlain = "hello",
            Recipients = new List<Recipient> { new Recipient { EmailAddress = "john.doe@example.com" } }
        });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/email", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsRecipientEmailAddressToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(new EmailModel
        {
            MessagePlain = "hello",
            Recipients = new List<Recipient> { new Recipient { EmailAddress = "john.doe@example.com" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("john.doe@example.com", destination.GetProperty("EmailAddress").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_ReturnsMessageIDFromResponse()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await email.SendMessageAsync(new EmailModel
        {
            MessagePlain = "hello",
            Recipients = new List<Recipient> { new Recipient { EmailAddress = "john.doe@example.com" } }
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
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await email.SendMessageAsync(new EmailModel { MessagePlain = "hello" });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task SendMessageAsync_WithNoMessageContent_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await email.SendMessageAsync(new EmailModel
        {
            Recipients = new List<Recipient> { new Recipient { EmailAddress = "john.doe@example.com" } }
        });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void SendMessage_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        var result = email.SendMessage(new EmailModel
        {
            MessagePlain = "hello",
            Recipients = new List<Recipient> { new Recipient { EmailAddress = "john.doe@example.com" } }
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
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await email.SendMessageAsync(
            messagePlain: "hello",
            destination: "john.doe@example.com",
            emailSubject: "Test",
            sendMode: Enums.SendModeType.Live
        );

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithEmailAddress_MapsToDestinationRecipient()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(
            messagePlain: "hello",
            emailAddress: "john.doe@example.com",
            emailSubject: "Test",
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("john.doe@example.com", destination.GetProperty("Recipient").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithContactIDAndGroupID_AddsBothToDestinations()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(
            messagePlain: "hello",
            emailSubject: "Test",
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
    public void SendMessage_WithCommaSeparatedEmailAddress_CreatesMultipleRecipients()
    {
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;

        email.SendMessage(messagePlain: "hello", emailAddress: "a@test.com,b@test.com", sendMode: Enums.SendModeType.Test);

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        Assert.Equal("a@test.com", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("b@test.com", destinations[1].GetProperty("Recipient").GetString());
    }

    [Fact]
    public void SendMessage_BulkSendViaCommaSeparatedEmailAddress_SucceedsEndToEnd()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        var response = email.SendMessage(
            emailAddress: "email1@example.com,email2@example.com",
            emailSubject: "Office closed today.",
            messagePlain: "The office is closed today for a public holiday.",
            sendMode: Enums.SendModeType.Test
        );

        Assert.Equal(Enums.ResultCode.Success, response.Result);
        Assert.NotNull(response.MessageID);
        Assert.Equal("abc-123", response.MessageID!.Value);

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        Assert.Equal("email1@example.com", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("email2@example.com", destinations[1].GetProperty("Recipient").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsAllNineCustomFieldsToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(new EmailModel
        {
            MessagePlain = "hello",
            Recipients = new List<Recipient>
            {
                new Recipient { EmailAddress = "a@test.com", Custom5 = "c5", Custom6 = "c6", Custom7 = "c7", Custom8 = "c8", Custom9 = "c9" }
            }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("c5", destination.GetProperty("Custom5").GetString());
        Assert.Equal("c6", destination.GetProperty("Custom6").GetString());
        Assert.Equal("c7", destination.GetProperty("Custom7").GetString());
        Assert.Equal("c8", destination.GetProperty("Custom8").GetString());
        Assert.Equal("c9", destination.GetProperty("Custom9").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_WithDestinations_SendsRecipientFieldOnWire()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(new EmailModel
        {
            MessagePlain = "hello",
            Destinations = new List<Destination> { new Destination { Recipient = "john.doe@example.com", FirstName = "John" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("john.doe@example.com", destination.GetProperty("Recipient").GetString());
        Assert.Equal("John", destination.GetProperty("FirstName").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_SetsNotificationTypeOnBuiltModel()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(
            messagePlain: "hello",
            destination: "john.doe@example.com",
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
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(
            messagePlain: "hello",
            destination: new Destination { Recipient = "john.doe@example.com", FirstName = "John" }
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(1, destinations.GetArrayLength());
        Assert.Equal("john.doe@example.com", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("John", destinations[0].GetProperty("FirstName").GetString());
        // Guards against double-delivery: a Destination-only send must not also populate Recipients.
        Assert.False(doc.RootElement.TryGetProperty("Recipients", out var recipients) && recipients.GetArrayLength() > 0);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithDestinationAndDestinationsTogether_MergesBothOnWire()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await email.SendMessageAsync(
            messagePlain: "hello",
            destination: new Destination { Recipient = "john.doe@example.com", FirstName = "John" },
            destinations: new List<Destination> { new Destination { Recipient = "jane.doe@example.com", FirstName = "Jane" } }
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        // Order is an intentional contract, not incidental: EmailApi.SendMessageAsync adds the
        // singular `destination` to destinationList before the plural `destinations` list, so
        // singular-first is the expected, stable merge order on the wire.
        Assert.Equal("john.doe@example.com", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("jane.doe@example.com", destinations[1].GetProperty("Recipient").GetString());
        // Guards against double-delivery: a merged destination+destinations send must not also
        // populate Recipients.
        Assert.False(doc.RootElement.TryGetProperty("Recipients", out var recipients) && recipients.GetArrayLength() > 0);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithUnsupportedDestinationType_Throws()
    {
        var email = new EmailApi(new TNZApiUser { AuthToken = "test-token" });

        await Assert.ThrowsAsync<ArgumentException>(() => email.SendMessageAsync(
            messagePlain: "hello",
            destination: 12345
        ));
    }
}