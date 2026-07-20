using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Messaging.SMS;

public class SMSBuilderTests
{
    [Fact]
    public void Build_SetsMessageText()
    {
        using var builder = new SMSBuilder();

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
        using var builder = new SMSBuilder();

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
    public void Build_SetsFallbackModeAndCharacterConversion()
    {
        using var builder = new SMSBuilder();

        var model = builder
            .SetMessageText("hello")
            .AddFallbackMode(Enums.SMSFallbackMode.Voice)
            .SetCharacterConversion(true)
            .Build();

        Assert.NotNull(model.FallbackMode);
        Assert.Equal(new[] { Enums.SMSFallbackMode.Voice }, model.FallbackMode);
        Assert.True(model.CharacterConversion);
    }

    [Fact]
    public void Build_AddsMultipleFallbackModesInOrder()
    {
        using var builder = new SMSBuilder();

        var model = builder
            .SetMessageText("hello")
            .AddFallbackMode(Enums.SMSFallbackMode.RCS)
            .AddFallbackMode(Enums.SMSFallbackMode.WhatsApp)
            .AddFallbackMode(Enums.SMSFallbackMode.Voice)
            .Build();

        Assert.NotNull(model.FallbackMode);
        Assert.Equal(new[] { Enums.SMSFallbackMode.RCS, Enums.SMSFallbackMode.WhatsApp, Enums.SMSFallbackMode.Voice }, model.FallbackMode);
    }

    [Fact]
    public void Build_SetsSendMode()
    {
        using var builder = new SMSBuilder();

        var model = builder
            .SetMessageText("hello")
            .SetSendMode(Enums.SendModeType.Test)
            .Build();

        Assert.Equal(Enums.SendModeType.Test, model.SendMode);
    }

    [Fact]
    public async Task BuildAsync_AddsFileAttachments()
    {
        using var builder = new SMSBuilder();

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
        using var builder = new SMSBuilder();

        var model = builder
            .SetMessageText("hello")
            .SetChargeCode("CC-001")
            .Build();

        Assert.Equal("CC-001", model.ChargeCode);
    }

    [Fact]
    public void Build_AddsDestinationsToDestinationsList()
    {
        using var builder = new SMSBuilder();

        var model = builder
            .SetMessageText("hello")
            .AddDestination("+64211234567")
            .AddDestination(new Destination { Recipient = "+64211234568", FirstName = "Jane" })
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("+64211234567", model.Destinations.First().Recipient);
        Assert.Equal("Jane", model.Destinations.Last().FirstName);
    }

    [Fact]
    public void Build_SetsNotificationType()
    {
        using var builder = new SMSBuilder();

        var model = builder
            .SetMessageText("hello")
            .SetNotificationType(Enums.NotificationType.Webhook)
            .Build();

        Assert.Equal(Enums.NotificationType.Webhook, model.NotificationType);
    }

    [Fact]
    public void Build_AddsRecipientsViaAddressbookContactIDAndGroupID()
    {
        using var builder = new SMSBuilder();

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
        using var builder = new SMSBuilder();

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