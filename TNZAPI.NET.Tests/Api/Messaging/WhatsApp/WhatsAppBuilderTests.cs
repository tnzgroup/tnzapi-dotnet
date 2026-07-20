using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.WhatsApp;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Messaging.WhatsApp;

public class WhatsAppBuilderTests
{
    [Fact]
    public void Build_SetsMessageText()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello world")
            .Build();

        Assert.Equal("hello world", model.Message);
    }

    [Fact]
    public void Build_AddsRecipientsToDestinationsList()
    {
        // AddRecipient(...) routes through Destinations (not Recipients) so every recipient added
        // via the Builder consistently sends the wire's primary "Recipient" field — see the
        // Destination-model-rollout memory for why.
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .AddRecipient("+64211234567")
            .AddRecipient("+64211234568")
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("+64211234567", model.Destinations.First().Recipient);
        Assert.Equal("+64211234568", model.Destinations.Last().Recipient);
    }

    [Fact]
    public void Build_AddsFallbackModesInOrder()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .AddFallbackMode(Enums.WhatsAppFallbackMode.SMS)
            .AddFallbackMode(Enums.WhatsAppFallbackMode.Voice)
            .Build();

        Assert.Equal(new[] { Enums.WhatsAppFallbackMode.SMS, Enums.WhatsAppFallbackMode.Voice }, model.FallbackMode);
    }

    [Fact]
    public void Build_SetsFromNumberAndTemplateID()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .SetFromNumber("+6495006000")
            .SetTemplateID("123e4567-e89b-12d3-a456-426614174000")
            .Build();

        Assert.Equal("+6495006000", model.FromNumber);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.TemplateID);
    }

    [Fact]
    public void Build_SetsSendMode()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .SetSendMode(Enums.SendModeType.Test)
            .Build();

        Assert.Equal(Enums.SendModeType.Test, model.SendMode);
    }

    [Fact]
    public async Task BuildAsync_AddsFileAttachments()
    {
        using var builder = new WhatsAppBuilder();

        var model = await builder
            .SetMessageText("hello")
            .AddAttachment("test.txt", "base64content")
            .BuildAsync();

        Assert.NotNull(model.Files);
        Assert.Single(model.Files);
        Assert.Equal("test.txt", model.Files.First().FileName);
    }

    [Fact]
    public void Build_SetsChargeCode()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .SetChargeCode("CC-001")
            .Build();

        Assert.Equal("CC-001", model.ChargeCode);
    }

    [Fact]
    public void Build_AddsDestinationsToDestinationsList()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .AddDestination(new Destination { Recipient = "+64211234567", FirstName = "John" })
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Single(model.Destinations);
        Assert.Equal("+64211234567", model.Destinations.First().Recipient);
    }

    [Fact]
    public void Build_SetsNotificationType()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .SetNotificationType(Enums.NotificationType.Webhook)
            .Build();

        Assert.Equal(Enums.NotificationType.Webhook, model.NotificationType);
    }

    [Fact]
    public void Build_AddsRecipientsViaAddressbookContactIDAndGroupID()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .AddRecipient(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .AddRecipients(new GroupID("223e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.Destinations.First().ContactID?.Value);
        Assert.Equal("223e4567-e89b-12d3-a456-426614174000", model.Destinations.Last().GroupID?.Value);
    }

    [Fact]
    public void Build_AddsDestinationsViaAddressbookContactIDAndGroupID()
    {
        using var builder = new WhatsAppBuilder();

        var model = builder
            .SetMessageText("hello")
            .AddDestination(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .AddDestinations(new GroupID("223e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.Destinations.First().ContactID?.Value);
        Assert.Equal("223e4567-e89b-12d3-a456-426614174000", model.Destinations.Last().GroupID?.Value);
    }
}