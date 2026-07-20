using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Messaging.Email;

public class EmailBuilderTests
{
    [Fact]
    public void Build_SetsMessagePlainAndHTML()
    {
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello world")
            .SetMessageHTML("<p>hello world</p>")
            .Build();

        Assert.Equal("hello world", model.MessagePlain);
        Assert.Equal("<p>hello world</p>", model.MessageHTML);
    }

    [Fact]
    public void Build_AddsRecipientsToDestinationsList()
    {
        // AddRecipient(...) routes through Destinations (not Recipients) so every recipient added
        // via the Builder consistently sends the wire's primary "Recipient" field — see the
        // Destination-model-rollout memory for why.
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello")
            .AddRecipient("john.doe@example.com")
            .AddRecipient("jane.doe@example.com")
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("john.doe@example.com", model.Destinations.First().Recipient);
        Assert.Equal("jane.doe@example.com", model.Destinations.Last().Recipient);
    }

    [Fact]
    public void Build_SetsFromAndSubjectFields()
    {
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello")
            .SetFrom("Company One Ltd")
            .SetFromEmail("help@example.com")
            .SetEmailSubject("Test Subject")
            .SetCCEmail("archive@example.com")
            .Build();

        Assert.Equal("Company One Ltd", model.From);
        Assert.Equal("help@example.com", model.FromEmail);
        Assert.Equal("Test Subject", model.EmailSubject);
        Assert.Equal("archive@example.com", model.CCEmail);
    }

    [Fact]
    public void Build_SetsSendMode()
    {
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello")
            .SetSendMode(Enums.SendModeType.Test)
            .Build();

        Assert.Equal(Enums.SendModeType.Test, model.SendMode);
    }

    [Fact]
    public async Task BuildAsync_AddsFileAttachments()
    {
        using var builder = new EmailBuilder();

        var model = await builder
            .SetMessagePlain("hello")
            .AddAttachment("test.txt", "base64content")
            .BuildAsync();

        Assert.NotNull(model.Files);
        Assert.Single(model.Files);
        Assert.Equal("test.txt", model.Files.First().FileName);
    }

    [Fact]
    public void Build_SetsChargeCode()
    {
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello")
            .SetChargeCode("CC-001")
            .Build();

        Assert.Equal("CC-001", model.ChargeCode);
    }

    [Fact]
    public void Build_AddsDestinationsToDestinationsList()
    {
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello")
            .AddDestination(new Destination { Recipient = "john.doe@example.com", FirstName = "John" })
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Single(model.Destinations);
        Assert.Equal("john.doe@example.com", model.Destinations.First().Recipient);
    }

    [Fact]
    public void Build_SetsNotificationType()
    {
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello")
            .SetNotificationType(Enums.NotificationType.Email)
            .Build();

        Assert.Equal(Enums.NotificationType.Email, model.NotificationType);
    }

    [Fact]
    public void Build_AddsRecipientsViaAddressbookContactIDAndGroupID()
    {
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello")
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
        using var builder = new EmailBuilder();

        var model = builder
            .SetMessagePlain("hello")
            .AddDestination(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .AddDestinations(new GroupID("223e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.Destinations.First().ContactID?.Value);
        Assert.Equal("223e4567-e89b-12d3-a456-426614174000", model.Destinations.Last().GroupID?.Value);
    }
}