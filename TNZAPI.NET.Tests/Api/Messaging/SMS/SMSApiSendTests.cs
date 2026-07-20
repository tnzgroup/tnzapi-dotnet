using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.SMS;

public class SMSApiSendTests : IDisposable
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
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(new SMSModel
        {
            Message = "hello",
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/sms", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsRecipientMobileNumberToDestinationToNumber()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(new SMSModel
        {
            Message = "hello",
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+64211234567", destination.GetProperty("ToNumber").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_ReturnsMessageIDFromResponse()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.SendMessageAsync(new SMSModel
        {
            Message = "hello",
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
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.SendMessageAsync(new SMSModel { Message = "hello" });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task SendMessageAsync_WithEmptyMessage_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.SendMessageAsync(new SMSModel
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
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = sms.SendMessage(new SMSModel
        {
            Message = "hello",
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
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await sms.SendMessageAsync(
            messageText: "hello",
            destination: "+64211234567",
            sendMode: Enums.SendModeType.Live
        );

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task SendMessageAsync_WithDestinations_SendsRecipientFieldOnWire()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(new SMSModel
        {
            Message = "hello",
            Destinations = new List<Destination> { new Destination { Recipient = "+64211234567", FirstName = "Jane" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+64211234567", destination.GetProperty("Recipient").GetString());
        Assert.Equal("Jane", destination.GetProperty("FirstName").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_WithBothRecipientsAndDestinations_MergesIntoOneDestinationsArray()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(new SMSModel
        {
            Message = "hello",
            Recipients = new List<Recipient> { new Recipient("+64211111111") },
            Destinations = new List<Destination> { new Destination("+64222222222") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        Assert.Equal("+64211111111", destinations[0].GetProperty("ToNumber").GetString());
        Assert.Equal("+64222222222", destinations[1].GetProperty("Recipient").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_SetsNotificationTypeOnBuiltModel()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(
            messageText: "hello",
            destination: "+64211234567",
            notificationType: Enums.NotificationType.Webhook
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("Webhook", doc.RootElement.GetProperty("NotificationType").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_TestMode_IncludesModeFieldInBody()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(
            messageText: "hello",
            destination: "+64211234567",
            sendMode: Enums.SendModeType.Test
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("Test", doc.RootElement.GetProperty("Mode").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_LiveMode_OmitsModeFieldFromBody()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(
            messageText: "hello",
            destination: "+64211234567",
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.False(doc.RootElement.TryGetProperty("Mode", out _));
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithToNumber_MapsToDestinationRecipient()
    {
        // The named-parameter overload routes everything (toNumber/contactID/groupID/recipients/
        // destination) through Destinations only, so it always sends the wire's primary "Recipient"
        // field — see the Destination-model-rollout memory.
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(
            messageText: "hello",
            toNumber: "+64211234567",
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+64211234567", destination.GetProperty("Recipient").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithContactIDAndGroupID_AddsBothToDestinations()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(
            messageText: "hello",
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
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;

        sms.SendMessage(messageText: "hello", toNumber: "+64211111111,+64222222222", sendMode: Enums.SendModeType.Test);

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        Assert.Equal("+64211111111", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("+64222222222", destinations[1].GetProperty("Recipient").GetString());
    }

    [Fact]
    public void SendMessage_BulkSendViaCommaSeparatedToNumber_SucceedsEndToEnd()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        var response = sms.SendMessage(
            toNumber: "+64211111111,+64222222222",
            messageText: "Office closed today.",
            sendMode: Enums.SendModeType.Test
        );

        Assert.Equal(Enums.ResultCode.Success, response.Result);
        Assert.NotNull(response.MessageID);
        Assert.Equal("abc-123", response.MessageID!.Value);

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
        Assert.Equal("+64211111111", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("+64222222222", destinations[1].GetProperty("Recipient").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsAllNineCustomFieldsToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(new SMSModel
        {
            Message = "hello",
            Recipients = new List<Recipient>
            {
                new Recipient
                {
                    MobileNumber = "+64211234567",
                    Custom1 = "c1", Custom2 = "c2", Custom3 = "c3", Custom4 = "c4",
                    Custom5 = "c5", Custom6 = "c6", Custom7 = "c7", Custom8 = "c8", Custom9 = "c9"
                }
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
    public async Task SendMessageAsync_SerializesFallbackModeAsCommaJoinedStringWithWhatsAppMappedToWAPP()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(new SMSModel
        {
            Message = "hello",
            FallbackMode = new List<Enums.SMSFallbackMode> { Enums.SMSFallbackMode.RCS, Enums.SMSFallbackMode.WhatsApp, Enums.SMSFallbackMode.Voice },
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("RCS, WAPP, Voice", doc.RootElement.GetProperty("FallbackMode").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithDestinationObject_SendsRecipientFieldOnWire()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await sms.SendMessageAsync(
            messageText: "hello",
            destination: new Destination { Recipient = "+64211234567", FirstName = "John" }
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(1, destinations.GetArrayLength());
        Assert.Equal("+64211234567", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("John", destinations[0].GetProperty("FirstName").GetString());
        // Guards against double-delivery: a Destination-only send must not also populate Recipients.
        Assert.False(doc.RootElement.TryGetProperty("Recipients", out var recipients) && recipients.GetArrayLength() > 0);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithUnsupportedDestinationType_Throws()
    {
        var sms = new SMSApi(new TNZApiUser { AuthToken = "test-token" });

        await Assert.ThrowsAsync<ArgumentException>(() => sms.SendMessageAsync(
            messageText: "hello",
            destination: 12345
        ));
    }
}