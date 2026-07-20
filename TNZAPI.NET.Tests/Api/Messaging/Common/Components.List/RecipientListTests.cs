using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Tests.Api.Messaging.Common.Components.List;

public class RecipientListTests
{
    [Fact]
    public void Add_WithStringRecipient_AddsRecipientWithContactFieldsSet()
    {
        using var list = new RecipientList();

        list.Add("+64211234567");

        var result = list.ToList();

        var recipient = Assert.Single(result);
        Assert.Equal("+64211234567", recipient.MobileNumber);
        Assert.Equal("+64211234567", recipient.PhoneNumber);
        Assert.Equal("+64211234567", recipient.FaxNumber);
        Assert.Equal("+64211234567", recipient.EmailAddress);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Add_WithNullOrEmptyStringRecipient_IsIgnored(string? recipient)
    {
        using var list = new RecipientList();

        list.Add(recipient);

        var result = list.ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void Add_WithRecipientInstance_IncludesItInResult()
    {
        using var list = new RecipientList();

        list.Add(new Recipient("+64211234567"));

        var result = list.ToList();

        var recipient = Assert.Single(result);
        Assert.Equal("+64211234567", recipient.MobileNumber);
    }

    [Fact]
    public void Add_WithNullRecipientInstance_IsIgnored()
    {
        using var list = new RecipientList();

        list.Add((Recipient)null!);

        var result = list.ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void Add_WithGroupID_AddsRecipientWithGroupIDSet()
    {
        using var list = new RecipientList();

        list.Add(new GroupID("group-1"));

        var result = list.ToList();

        var recipient = Assert.Single(result);
        Assert.NotNull(recipient.GroupID);
        Assert.Equal("group-1", recipient.GroupID);
    }

    [Fact]
    public void Add_WithContactID_AddsRecipientWithContactIDSet()
    {
        using var list = new RecipientList();

        list.Add(new ContactID("contact-1"));

        var result = list.ToList();

        var recipient = Assert.Single(result);
        Assert.NotNull(recipient.ContactID);
        Assert.Equal("contact-1", recipient.ContactID);
    }

    [Fact]
    public void Add_WithGenericCollectionOfStrings_AddsEachAsRecipient()
    {
        using var list = new RecipientList();

        list.Add(new List<string> { "+64211234567", "+64211234568" });

        var result = list.ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Add_WithGenericCollectionOfStringsContainingNull_SkipsNullElementWithoutThrowing()
    {
        using var list = new RecipientList();

        list.Add(new List<string?> { "+64211234567", null, "+64211234568" });

        var result = list.ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Add_WithGenericCollectionOfRecipients_AddsEachRecipient()
    {
        using var list = new RecipientList();

        list.Add(new List<Recipient>
        {
            new Recipient("+64211234567"),
            new Recipient("+64211234568")
        });

        var result = list.ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Add_WithGenericCollectionOfContactIDs_AddsEachAsRecipient()
    {
        using var list = new RecipientList();

        list.Add(new List<ContactID> { new ContactID("contact-1"), new ContactID("contact-2") });

        var result = list.ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Add_WithGenericCollectionOfGroupIDs_AddsEachAsRecipient()
    {
        using var list = new RecipientList();

        list.Add(new List<GroupID> { new GroupID("group-1"), new GroupID("group-2") });

        var result = list.ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Add_WithNullOrEmptyGenericCollection_IsIgnored()
    {
        using var list = new RecipientList();

        list.Add((ICollection<string>?)null).Add(new List<string>());

        var result = list.ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void Add_WithUnsupportedGenericType_ThrowsNotSupportedException()
    {
        using var list = new RecipientList();

        var ex = Assert.Throws<NotSupportedException>(() => list.Add(new List<int> { 1 }));
        Assert.Contains("Int32", ex.Message);
    }

    [Fact]
    public void ToList_WithNoRecipientsAdded_ReturnsEmptyCollection()
    {
        using var list = new RecipientList();

        var result = list.ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Add_ReturnsSameInstanceForChaining()
    {
        using var list = new RecipientList();

        var result = list.Add("+64211234567").Add(new Recipient("+64211234568"));

        Assert.Same(list, result);
    }

    [Fact]
    public void ToList_AfterDispose_ReturnsEmptyCollectionInsteadOfThrowing()
    {
        var list = new RecipientList();
        list.Add("+64211234567");
        list.Dispose();

        var result = list.ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Add_AfterDispose_DoesNotThrow()
    {
        var list = new RecipientList();
        list.Dispose();

        var result = list.Add("+64211234567");

        Assert.Same(list, result);
        var recipient = Assert.Single(result.ToList());
        Assert.Equal("+64211234567", recipient.MobileNumber);
    }

    [Fact]
    public void Add_WithCommaSeparatedString_AddsMultipleRecipients()
    {
        using var list = new RecipientList();

        list.Add("+64211111111, +64222222222,+64233333333");

        var result = list.ToList();
        Assert.Equal(3, result.Count);
        Assert.Equal("+64211111111", result.ElementAt(0).MobileNumber);
        Assert.Equal("+64222222222", result.ElementAt(1).MobileNumber);
        Assert.Equal("+64233333333", result.ElementAt(2).MobileNumber);
    }

    [Fact]
    public void Add_WithCommaSeparatedStringContainingEmptySegments_SkipsEmptySegments()
    {
        using var list = new RecipientList();

        list.Add("+64211111111,,+64222222222,");

        Assert.Equal(2, list.ToList().Count);
    }
}