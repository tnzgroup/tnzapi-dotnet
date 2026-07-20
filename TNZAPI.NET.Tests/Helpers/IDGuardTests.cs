using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Tests.Helpers;

public class IDGuardTests
{
    [Fact]
    public void EnsureProvided_MessageID_WithValidValue_DoesNotThrow()
    {
        IDGuard.EnsureProvided(new MessageID("abc-123"));
    }

    [Fact]
    public void EnsureProvided_MessageID_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => IDGuard.EnsureProvided((MessageID?)null));
    }

    [Fact]
    public void EnsureProvided_MessageID_WithEmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => IDGuard.EnsureProvided(new MessageID("")));
    }

    [Fact]
    public void EnsureProvided_ContactID_WithValidValue_DoesNotThrow()
    {
        IDGuard.EnsureProvided(new ContactID("123e4567-e89b-12d3-a456-426614174000"));
    }

    [Fact]
    public void EnsureProvided_ContactID_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => IDGuard.EnsureProvided((ContactID?)null));
    }

    [Fact]
    public void EnsureProvided_ContactID_WithEmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => IDGuard.EnsureProvided(new ContactID("")));
    }

    [Fact]
    public void EnsureProvided_GroupID_WithValidValue_DoesNotThrow()
    {
        IDGuard.EnsureProvided(new GroupID("123e4567-e89b-12d3-a456-426614174000"));
    }

    [Fact]
    public void EnsureProvided_GroupID_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => IDGuard.EnsureProvided((GroupID?)null));
    }

    [Fact]
    public void EnsureProvided_GroupID_WithEmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => IDGuard.EnsureProvided(new GroupID("")));
    }

    [Fact]
    public void EnsureProvided_OptOutID_WithValidValue_DoesNotThrow()
    {
        IDGuard.EnsureProvided(new OptOutID("123e4567-e89b-12d3-a456-426614174000"));
    }

    [Fact]
    public void EnsureProvided_OptOutID_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => IDGuard.EnsureProvided((OptOutID?)null));
    }

    [Fact]
    public void EnsureProvided_OptOutID_WithEmptyValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => IDGuard.EnsureProvided(new OptOutID("")));
    }
}
