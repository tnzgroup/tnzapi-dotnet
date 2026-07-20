using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Messaging.Fax;

public class FaxBuilderTests
{
    [Fact]
    public void Build_AddsRecipientsToDestinationsList()
    {
        // AddRecipient(...) routes through Destinations (not Recipients) so every recipient added
        // via the Builder consistently sends the wire's primary "Recipient" field — see the
        // Destination-model-rollout memory for why.
        using var builder = new FaxBuilder();

        var model = builder
            .AddRecipient("+6495006000")
            .AddRecipient("+6495006001")
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("+6495006000", model.Destinations.First().Recipient);
        Assert.Equal("+6495006001", model.Destinations.Last().Recipient);
    }

    [Fact]
    public async Task BuildAsync_AddsFileAttachments()
    {
        using var builder = new FaxBuilder();

        var model = await builder
            .AddAttachment("test.pdf", "base64content")
            .BuildAsync();

        Assert.NotNull(model.Files);
        Assert.Single(model.Files);
        Assert.Equal("test.pdf", model.Files.First().FileName);
    }

    [Fact]
    public void Build_SetsCSIDAndResolution()
    {
        using var builder = new FaxBuilder();

        var model = builder
            .AddAttachment("test.pdf", "base64content")
            .SetCSID("My HP Fax Machine")
            .SetResolution(Enums.FaxResolution.High)
            .Build();

        Assert.Equal("My HP Fax Machine", model.CSID);
        Assert.Equal(Enums.FaxResolution.High, model.Resolution);
    }

    [Fact]
    public void Build_SetsWatermarkFields()
    {
        using var builder = new FaxBuilder();

        var model = builder
            .AddAttachment("test.pdf", "base64content")
            .SetWatermarkFolder("Folder01")
            .SetWatermarkFirstPage("Watermark File Name.ps")
            .SetWatermarkAllPages("Watermark File Name.docx")
            .Build();

        Assert.Equal("Folder01", model.WatermarkFolder);
        Assert.Equal("Watermark File Name.ps", model.WatermarkFirstPage);
        Assert.Equal("Watermark File Name.docx", model.WatermarkAllPages);
    }

    [Fact]
    public void Build_SetsSendMode()
    {
        using var builder = new FaxBuilder();

        var model = builder
            .AddAttachment("test.pdf", "base64content")
            .SetSendMode(Enums.SendModeType.Test)
            .Build();

        Assert.Equal(Enums.SendModeType.Test, model.SendMode);
    }

    [Fact]
    public void Build_SetsChargeCode()
    {
        using var builder = new FaxBuilder();

        var model = builder
            .SetChargeCode("CC-001")
            .Build();

        Assert.Equal("CC-001", model.ChargeCode);
    }

    [Fact]
    public void Build_AddsDestinationsToDestinationsList()
    {
        using var builder = new FaxBuilder();

        var model = builder
            .AddDestination(new Destination { Recipient = "+6495006000", FirstName = "John" })
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Single(model.Destinations);
        Assert.Equal("+6495006000", model.Destinations.First().Recipient);
    }

    [Fact]
    public void Build_SetsNotificationType()
    {
        using var builder = new FaxBuilder();

        var model = builder
            .SetNotificationType(Enums.NotificationType.Email)
            .Build();

        Assert.Equal(Enums.NotificationType.Email, model.NotificationType);
    }

    [Fact]
    public void Build_AddsRecipientsViaAddressbookContactIDAndGroupID()
    {
        using var builder = new FaxBuilder();

        var model = builder
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
        using var builder = new FaxBuilder();

        var model = builder
            .AddDestination(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .AddDestinations(new GroupID("223e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.Destinations.First().ContactID?.Value);
        Assert.Equal("223e4567-e89b-12d3-a456-426614174000", model.Destinations.Last().GroupID?.Value);
    }
}