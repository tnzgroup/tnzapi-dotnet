using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.TTS;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.TTS;

public class TTSApiSendTests : IDisposable
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
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(new TTSModel
        {
            MessageToPeople = "hello",
            Recipients = new List<Recipient> { new Recipient { PhoneNumber = "+6421000001" } }
        });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/tts", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsRecipientPhoneNumberToMainPhone()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(new TTSModel
        {
            MessageToPeople = "hello",
            Recipients = new List<Recipient> { new Recipient { PhoneNumber = "+6421000001" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+6421000001", destination.GetProperty("MainPhone").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsKeypadsToKeypadBodies()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(new TTSModel
        {
            MessageToPeople = "hello",
            Recipients = new List<Recipient> { new Recipient { PhoneNumber = "+6421000001" } },
            Keypads = new List<KeypadModel> { new KeypadModel(1, "Connecting you now.") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var keypad = doc.RootElement.GetProperty("Keypads")[0];
        Assert.Equal(1, keypad.GetProperty("Tone").GetInt32());
        Assert.Equal("Connecting you now.", keypad.GetProperty("Play").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_ReturnsMessageIDFromResponse()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await tts.SendMessageAsync(new TTSModel
        {
            MessageToPeople = "hello",
            Recipients = new List<Recipient> { new Recipient { PhoneNumber = "+6421000001" } }
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
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await tts.SendMessageAsync(new TTSModel { MessageToPeople = "hello" });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task SendMessageAsync_WithNoMessageContent_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await tts.SendMessageAsync(new TTSModel
        {
            Recipients = new List<Recipient> { new Recipient { PhoneNumber = "+6421000001" } }
        });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void SendMessage_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = tts.SendMessage(new TTSModel
        {
            MessageToPeople = "hello",
            Recipients = new List<Recipient> { new Recipient { PhoneNumber = "+6421000001" } }
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
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await tts.SendMessageAsync(
            messageToPeople: "hello",
            destination: "+6421000001",
            sendMode: Enums.SendModeType.Live
        );

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithToNumber_MapsToDestinationRecipient()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(
            messageToPeople: "hello",
            toNumber: "+6421000001",
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+6421000001", destination.GetProperty("Recipient").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithContactIDAndGroupID_AddsBothToDestinations()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(
            messageToPeople: "hello",
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
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;

        tts.SendMessage(messageToPeople: "hello", toNumber: "+64211111111,+64222222222", sendMode: Enums.SendModeType.Test);

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal(2, doc.RootElement.GetProperty("Destinations").GetArrayLength());
    }

    [Fact]
    public async Task SendMessageAsync_MapsAllNineCustomFieldsToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(new TTSModel
        {
            MessageToPeople = "hello",
            Recipients = new List<Recipient> { new Recipient { PhoneNumber = "+64211111111", Custom5 = "c5", Custom9 = "c9" } }
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
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(new TTSModel
        {
            MessageToPeople = "hello",
            Destinations = new List<Destination> { new Destination { Recipient = "+6421000001", FirstName = "John" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+6421000001", destination.GetProperty("Recipient").GetString());
        Assert.Equal("John", destination.GetProperty("FirstName").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithDestinationObject_SendsRecipientFieldOnWire()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(
            messageToPeople: "hello",
            destination: new Destination { Recipient = "+6421000001", FirstName = "John" }
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(1, destinations.GetArrayLength());
        Assert.Equal("+6421000001", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("John", destinations[0].GetProperty("FirstName").GetString());
        // Guards against double-delivery: a Destination-only send must not also populate Recipients.
        Assert.False(doc.RootElement.TryGetProperty("Recipients", out var recipients) && recipients.GetArrayLength() > 0);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithUnsupportedDestinationType_Throws()
    {
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await Assert.ThrowsAsync<ArgumentException>(() => tts.SendMessageAsync(
            messageToPeople: "hello",
            destination: 12345
        ));
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_SetsNotificationTypeOnBuiltModel()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var tts = new TTSApi(new TNZApiUser { AuthToken = "test-token" });

        await tts.SendMessageAsync(
            messageToPeople: "hello",
            destination: "+64211111111",
            notificationType: Enums.NotificationType.Webhook
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal("Webhook", doc.RootElement.GetProperty("NotificationType").GetString());
    }
}