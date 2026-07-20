using TNZAPI.NET.Api.Configuration.OptOut;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Configuration.OptOut;

public class OptOutBuilderTests
{
    [Fact]
    public void Build_SetsDestTypeAndDestination()
    {
        using var builder = new OptOutBuilder();

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
        using var builder = new OptOutBuilder();

        var model = builder
            .SetDestType(Enums.OptOutDestType.SMS)
            .SetDestination("+6421003004")
            .Build();

        Assert.Equal("SMS", model.DestType);
    }

    [Fact]
    public void Build_SetsContactID()
    {
        using var builder = new OptOutBuilder();

        var model = builder
            .SetDestType("Email")
            .SetContactID(new ContactID("123e4567-e89b-12d3-a456-426614174000"))
            .Build();

        Assert.NotNull(model.ContactID);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", model.ContactID.Value);
    }

    [Fact]
    public void Build_SetsSubAccountDepartmentAndNotes()
    {
        using var builder = new OptOutBuilder();

        var model = builder
            .SetDestType("SMS")
            .SetSubAccount("Business Unit One")
            .SetDepartment("Team Alpha")
            .SetNotes("Entry edited using the API.")
            .Build();

        Assert.Equal("Business Unit One", model.SubAccount);
        Assert.Equal("Team Alpha", model.Department);
        Assert.Equal("Entry edited using the API.", model.Notes);
    }
}