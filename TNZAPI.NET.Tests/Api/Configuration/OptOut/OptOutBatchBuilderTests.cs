using TNZAPI.NET.Api.Configuration.OptOut;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Configuration.OptOut;

public class OptOutBatchBuilderTests
{
    [Fact]
    public void Build_SetsDestTypeAndDestination()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType("SMS")
            .SetDestination("+6421003004")
            .Build();

        Assert.Equal("SMS", model.DestType);
        Assert.Equal("+6421003004", model.Destination);
    }

    [Fact]
    public void Build_SetsDestTypeFromEnum()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType(Enums.OptOutDestType.SMS)
            .Build();

        Assert.Equal("SMS", model.DestType);
    }

    [Fact]
    public void Build_AddsDestinationsToDestinationsList()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType("SMS")
            .AddDestination("+6421003004")
            .AddDestination("+6421003005")
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Contains("+6421003004", model.Destinations);
        Assert.Contains("+6421003005", model.Destinations);
    }

    [Fact]
    public void Build_AddDestinationsBulk_AddsAllToDestinationsList()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType("SMS")
            .AddDestinations(new List<string> { "+6421003004", "+6421003005" })
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Equal(2, model.Destinations.Count);
        Assert.Contains("+6421003004", model.Destinations);
        Assert.Contains("+6421003005", model.Destinations);
    }

    [Fact]
    public void Build_SetsContactID()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType("Email")
            .SetContactID(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.ContactID);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.ContactID.Value);
    }

    [Fact]
    public void Build_AddsContactIDsToContactIDsList()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType("Email")
            .AddContactID(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .AddContactID(new ContactID("223e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.ContactIDs);
        Assert.Equal(2, model.ContactIDs.Count);
    }

    [Fact]
    public void Build_AddContactIDsBulk_AddsAllToContactIDsList()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType("Email")
            .AddContactIDs(new List<ContactID>
            {
                new ContactID("123e4567-e89b-12d3-a456-426614174000"),
                new ContactID("223e4567-e89b-12d3-a456-426614174000")
            })
            .Build();

        Assert.NotNull(model.ContactIDs);
        Assert.Equal(2, model.ContactIDs.Count);
        Assert.Contains(model.ContactIDs, c => c.Value == "123e4567-e89b-12d3-a456-426614174000");
        Assert.Contains(model.ContactIDs, c => c.Value == "223e4567-e89b-12d3-a456-426614174000");
    }

    [Fact]
    public void Build_SetsSubAccountAndDepartment()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType("SMS")
            .SetSubAccount("Business Unit One")
            .SetDepartment("Team Alpha")
            .Build();

        Assert.Equal("Business Unit One", model.SubAccount);
        Assert.Equal("Team Alpha", model.Department);
    }

    [Fact]
    public void Build_WithNoDestinationsOrContactIDsAdded_LeavesCollectionsEmpty()
    {
        using var builder = new OptOutBatchBuilder();

        var model = builder
            .SetDestType("SMS")
            .SetDestination("+6421003004")
            .Build();

        Assert.NotNull(model.Destinations);
        Assert.Empty(model.Destinations);
        Assert.NotNull(model.ContactIDs);
        Assert.Empty(model.ContactIDs);
    }
}
