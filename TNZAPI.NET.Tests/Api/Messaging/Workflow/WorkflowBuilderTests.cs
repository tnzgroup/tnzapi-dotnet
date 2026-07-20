using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Workflow;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Messaging.Workflow;

public class WorkflowBuilderTests
{
    [Fact]
    public void Build_SetsWorkflowTemplateID()
    {
        using var builder = new WorkflowBuilder();

        var model = builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
            .Build();

        Assert.Equal("a1b2c3d4-e5f6-7890-1234-567890abcdef", model.WorkflowTemplateID);
    }

    [Fact]
    public void Build_AddsRecipientsToDestinationsList()
    {
        // AddRecipient(...) routes through Destinations (not Recipients). This test only exercises
        // the plain-string overload, which sets the primary "Recipient" field (asserted below) — the
        // Recipient-object overload is different for Workflow specifically: it maps onto
        // ToNumber/MainPhone/EmailAddress instead, since Workflow is omni-channel (see
        // WorkflowBuilder.ToDestination).
        using var builder = new WorkflowBuilder();

        var model = builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
            .AddRecipient("+64211234567")
            .AddRecipient("+64211234568")
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("+64211234567", model.Destinations.First().Recipient);
        Assert.Equal("+64211234568", model.Destinations.Last().Recipient);
    }

    [Fact]
    public void Build_SetsSendMode()
    {
        using var builder = new WorkflowBuilder();

        var model = builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
            .SetSendMode(Enums.SendModeType.Test)
            .Build();

        Assert.Equal(Enums.SendModeType.Test, model.SendMode);
    }

    [Fact]
    public async Task BuildAsync_ReturnsModelWithRecipients()
    {
        using var builder = new WorkflowBuilder();

        var model = await builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
            .AddRecipient("+64211234567")
            .BuildAsync();

        Assert.NotNull(model.Destinations);
        Assert.Single(model.Destinations);
    }

    [Fact]
    public void Build_SetsChargeCode()
    {
        using var builder = new WorkflowBuilder();

        var model = builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
            .SetChargeCode("CC-001")
            .Build();

        Assert.Equal("CC-001", model.ChargeCode);
    }

    [Fact]
    public void Build_AddsDestinationsToDestinationsList()
    {
        using var builder = new WorkflowBuilder();

        var model = builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
            .AddDestination(new Destination { Recipient = "+64211234567", FirstName = "John" })
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Single(model.Destinations);
        Assert.Equal("+64211234567", model.Destinations.First().Recipient);
    }

    [Fact]
    public void Build_SetsNotificationType()
    {
        using var builder = new WorkflowBuilder();

        var model = builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
            .SetNotificationType(Enums.NotificationType.Webhook)
            .Build();

        Assert.Equal(Enums.NotificationType.Webhook, model.NotificationType);
    }

    [Fact]
    public void Build_AddsRecipientsViaAddressbookContactIDAndGroupID()
    {
        using var builder = new WorkflowBuilder();

        var model = builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
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
        using var builder = new WorkflowBuilder();

        var model = builder
            .SetWorkflowTemplateID("a1b2c3d4-e5f6-7890-1234-567890abcdef")
            .AddDestination(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .AddDestinations(new GroupID("223e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.Destinations.First().ContactID?.Value);
        Assert.Equal("223e4567-e89b-12d3-a456-426614174000", model.Destinations.Last().GroupID?.Value);
    }
}