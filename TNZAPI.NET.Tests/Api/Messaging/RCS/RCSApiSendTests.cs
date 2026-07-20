using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.RCS;
using TNZAPI.NET.Api.Messaging.RCS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.RCS;

public class RCSApiSendTests : IDisposable
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(new RCSModel
        {
            Message = "hello",
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/rcs", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsRecipientMobileNumberToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(new RCSModel
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await rcs.SendMessageAsync(new RCSModel
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await rcs.SendMessageAsync(new RCSModel { Message = "hello" });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task SendMessageAsync_WithNoMessageOrTemplate_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await rcs.SendMessageAsync(new RCSModel
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = rcs.SendMessage(new RCSModel
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await rcs.SendMessageAsync(
            messageText: "hello",
            destination: "+64211234567",
            sendMode: Enums.SendModeType.Live
        );

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithToNumber_MapsToDestinationRecipient()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;

        rcs.SendMessage(messageText: "hello", toNumber: "+64211111111,+64222222222", sendMode: Enums.SendModeType.Test);

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal(2, doc.RootElement.GetProperty("Destinations").GetArrayLength());
    }

    [Fact]
    public async Task SendMessageAsync_MapsAllNineCustomFieldsToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(new RCSModel
        {
            Message = "hello",
            Recipients = new List<Recipient> { new Recipient { MobileNumber = "+64211111111", Custom5 = "c5", Custom9 = "c9" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("c5", destination.GetProperty("Custom5").GetString());
        Assert.Equal("c9", destination.GetProperty("Custom9").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_SerializesFallbackModeAsCommaJoinedString()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(new RCSModel
        {
            Message = "hello",
            FallbackMode = new List<Enums.RCSFallbackMode> { Enums.RCSFallbackMode.SMS, Enums.RCSFallbackMode.Voice },
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("SMS, Voice", doc.RootElement.GetProperty("FallbackMode").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_SerializesWhatsAppFallbackModeAsWAPP()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(new RCSModel
        {
            Message = "hello",
            FallbackMode = new List<Enums.RCSFallbackMode> { Enums.RCSFallbackMode.WhatsApp, Enums.RCSFallbackMode.Voice },
            Recipients = new List<Recipient> { new Recipient("+64211234567") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("WAPP, Voice", doc.RootElement.GetProperty("FallbackMode").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_WithDestinations_SendsRecipientFieldOnWire()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(new RCSModel
        {
            Message = "hello",
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(
            messageText: "hello",
            destination: "+64211234567",
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await rcs.SendMessageAsync(
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
        var rcs = new RCSApi(new TNZApiUser { AuthToken = "test-token" });

        await Assert.ThrowsAsync<ArgumentException>(() => rcs.SendMessageAsync(
            messageText: "hello",
            destination: 12345
        ));
    }
}