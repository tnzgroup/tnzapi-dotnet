using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Tests.Api.Messaging.Common.Components;

public class RecipientTests
{
    [Fact]
    public void Constructor_WithGroupID_SetsGroupIDFromParameter()
    {
        var groupID = new GroupID("group-1");

        var recipient = new Recipient(groupID);

        Assert.NotNull(recipient.GroupID);
        Assert.Equal("group-1", recipient.GroupID);
    }

    [Fact]
    public void Constructor_WithContactID_SetsContactIDFromParameter()
    {
        var contactID = new ContactID("contact-1");

        var recipient = new Recipient(contactID);

        Assert.NotNull(recipient.ContactID);
        Assert.Equal("contact-1", recipient.ContactID);
    }
}