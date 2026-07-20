using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Api.Messaging.Voice;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Messaging.Voice;

public class VoiceBuilderTests
{
    [Fact]
    public void Build_SetsMessageToPeople()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
            .Build();

        Assert.Equal("base64audiodata", model.MessageToPeople);
    }

    [Fact]
    public void Build_AddsRecipientsToDestinationsList()
    {
        // AddRecipient(...) routes through Destinations (not Recipients) so every recipient added
        // via the Builder consistently sends the wire's primary "Recipient" field — see the
        // Destination-model-rollout memory for why.
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
            .AddRecipient("+6421000001")
            .AddRecipient("+6421000002")
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("+6421000001", model.Destinations.First().Recipient);
        Assert.Equal("+6421000002", model.Destinations.Last().Recipient);
    }

    [Fact]
    public void Build_AddsKeypadsToKeypadsList()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
            .AddKeypad(1, "base64keypadaudio")
            .AddKeypad(2, "base64keypadaudio2")
            .Build();

        Assert.NotNull(model.Keypads);
        Assert.Equal(2, model.Keypads.Count);
    }

    [Fact]
    public void Build_SetsAnswerPhoneFields()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
            .SetMessageToAnswerPhones("base64answerphoneaudio")
            .SetAnswerPhoneMode(Enums.AnswerPhoneMode.DAS)
            .Build();

        Assert.Equal("base64answerphoneaudio", model.MessageToAnswerPhones);
        Assert.Equal(Enums.AnswerPhoneMode.DAS, model.AnswerPhoneMode);
    }

    [Fact]
    public void Build_SetsSendMode()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
            .SetSendMode(Enums.SendModeType.Test)
            .Build();

        Assert.Equal(Enums.SendModeType.Test, model.SendMode);
    }

    [Fact]
    public void Build_SetsChargeCode()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("hello")
            .SetChargeCode("CC-001")
            .Build();

        Assert.Equal("CC-001", model.ChargeCode);
    }

    [Fact]
    public void Build_SetsEndCallMessage()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("hello")
            .SetEndCallMessage("Goodbye!")
            .Build();

        Assert.Equal("Goodbye!", model.EndCallMessage);
    }

    [Fact]
    public void Build_AddsVoiceFile()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .AddRecipient("+64211111111")
            .AddVoiceFile("greeting", "base64audiodata")
            .Build();

        Assert.NotNull(model.VoiceFiles);
        Assert.Single(model.VoiceFiles);
        Assert.Equal("greeting", model.VoiceFiles.First().Name);
        Assert.Equal("base64audiodata", model.VoiceFiles.First().File);
    }

    [Fact]
    public void Build_AddsKeypadWithPlayFile()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .AddRecipient("+64211111111")
            .AddKeypad(new KeypadModel { Tone = 1, PlayFile = "sales-greeting", File = "base64audiodata" })
            .Build();

        Assert.Equal("sales-greeting", model.Keypads!.First().PlayFile);
        Assert.Equal("base64audiodata", model.Keypads!.First().File);
    }

    [Fact]
    public void Build_AddsDestinationsToDestinationsList()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
            .AddDestination(new Destination { Recipient = "+6421000001", FirstName = "John" })
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Single(model.Destinations);
        Assert.Equal("+6421000001", model.Destinations.First().Recipient);
    }

    [Fact]
    public void Build_SetsNotificationType()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
            .SetNotificationType(Enums.NotificationType.Webhook)
            .Build();

        Assert.Equal(Enums.NotificationType.Webhook, model.NotificationType);
    }

    [Fact]
    public void Build_AddsRecipientsViaAddressbookContactIDAndGroupID()
    {
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
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
        using var builder = new VoiceBuilder();

        var model = builder
            .SetMessageToPeople("base64audiodata")
            .AddDestination(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .AddDestinations(new GroupID("223e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.Destinations.First().ContactID?.Value);
        Assert.Equal("223e4567-e89b-12d3-a456-426614174000", model.Destinations.Last().GroupID?.Value);
    }
}