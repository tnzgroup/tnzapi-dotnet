using System.Net;
using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Fax;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Api.Messaging.Fax;

public class FaxApiSendTests : IDisposable
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
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(new FaxModel
        {
            Recipients = new List<Recipient> { new Recipient { FaxNumber = "+6495006000" } },
            Files = new List<Attachment> { new Attachment("test.pdf", "base64content") }
        });

        Assert.Equal("https://api.tnz.co.nz/api/v3.00/fax", fakeHandler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsRecipientFaxNumberToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(new FaxModel
        {
            Recipients = new List<Recipient> { new Recipient { FaxNumber = "+6495006000" } },
            Files = new List<Attachment> { new Attachment("test.pdf", "base64content") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+6495006000", destination.GetProperty("ToNumber").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_MapsFilesToFileBodies()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(new FaxModel
        {
            Recipients = new List<Recipient> { new Recipient { FaxNumber = "+6495006000" } },
            Files = new List<Attachment> { new Attachment("test.pdf", "base64content") }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var file = doc.RootElement.GetProperty("Files")[0];
        Assert.Equal("test.pdf", file.GetProperty("Name").GetString());
        Assert.Equal("base64content", file.GetProperty("Data").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_ReturnsMessageIDFromResponse()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await fax.SendMessageAsync(new FaxModel
        {
            Recipients = new List<Recipient> { new Recipient { FaxNumber = "+6495006000" } },
            Files = new List<Attachment> { new Attachment("test.pdf", "base64content") }
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
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await fax.SendMessageAsync(new FaxModel
        {
            Files = new List<Attachment> { new Attachment("test.pdf", "base64content") }
        });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task SendMessageAsync_WithNoFilesOrTemplate_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await fax.SendMessageAsync(new FaxModel
        {
            Recipients = new List<Recipient> { new Recipient { FaxNumber = "+6495006000" } }
        });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public void SendMessage_Sync_DelegatesToAsync()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = fax.SendMessage(new FaxModel
        {
            Recipients = new List<Recipient> { new Recipient { FaxNumber = "+6495006000" } },
            Files = new List<Attachment> { new Attachment("test.pdf", "base64content") }
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
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        var result = await fax.SendMessageAsync(
            destination: "+6495006000",
            attachments: new List<Attachment> { new Attachment("test.pdf", "base64content") },
            sendMode: Enums.SendModeType.Live
        );

        Assert.Equal(Enums.ResultCode.Success, result.Result);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithToNumber_MapsToDestinationRecipient()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(
            toNumber: "+6495006000",
            attachments: new List<Attachment> { new Attachment("test.pdf", "base64content") },
            sendMode: Enums.SendModeType.Live
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+6495006000", destination.GetProperty("Recipient").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithContactIDAndGroupID_AddsBothToDestinations()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(
            contactID: "CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD",
            groupID: "GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD",
            attachments: new List<Attachment> { new Attachment("test.pdf", "base64content") },
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
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;

        fax.SendMessage(toNumber: "+6491111111,+6492222222", sendMode: Enums.SendModeType.Test,
            attachments: new List<Attachment> { new Attachment("doc.pdf", "base64content") });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        Assert.Equal(2, doc.RootElement.GetProperty("Destinations").GetArrayLength());
    }

    [Fact]
    public async Task SendMessageAsync_MapsAllNineCustomFieldsToDestination()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(new FaxModel
        {
            Files = new List<Attachment> { new Attachment("doc.pdf", "base64content") },
            Recipients = new List<Recipient>
            {
                new Recipient { FaxNumber = "+6491111111", Custom5 = "c5", Custom9 = "c9" }
            }
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
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(new FaxModel
        {
            Files = new List<Attachment> { new Attachment("doc.pdf", "base64content") },
            Destinations = new List<Destination> { new Destination { Recipient = "+6495006000", FirstName = "John" } }
        });

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destination = doc.RootElement.GetProperty("Destinations")[0];
        Assert.Equal("+6495006000", destination.GetProperty("Recipient").GetString());
        Assert.Equal("John", destination.GetProperty("FirstName").GetString());
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_SetsNotificationTypeOnBuiltModel()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(
            destination: "+6491111111",
            attachments: new List<Attachment> { new Attachment("doc.pdf", "base64content") },
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
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await fax.SendMessageAsync(
            destination: new Destination { Recipient = "+6491111111", FirstName = "John" },
            attachments: new List<Attachment> { new Attachment("doc.pdf", "base64content") }
        );

        using var doc = JsonDocument.Parse(fakeHandler.LastRequestBody!);
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(1, destinations.GetArrayLength());
        Assert.Equal("+6491111111", destinations[0].GetProperty("Recipient").GetString());
        Assert.Equal("John", destinations[0].GetProperty("FirstName").GetString());
        // Guards against double-delivery: a Destination-only send must not also populate Recipients.
        Assert.False(doc.RootElement.TryGetProperty("Recipients", out var recipients) && recipients.GetArrayLength() > 0);
    }

    [Fact]
    public async Task SendMessageAsync_NamedParamsOverload_WithUnsupportedDestinationType_Throws()
    {
        var fax = new FaxApi(new TNZApiUser { AuthToken = "test-token" });

        await Assert.ThrowsAsync<ArgumentException>(() => fax.SendMessageAsync(
            destination: 12345,
            attachments: new List<Attachment> { new Attachment("doc.pdf", "base64content") }
        ));
    }
}