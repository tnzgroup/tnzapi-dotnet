using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Tests.Helpers;

public class EnumListHelperTests
{
    [Fact]
    public void ToCommaSeparatedString_WithNullCollection_ReturnsNull()
    {
        var result = EnumListHelper.ToCommaSeparatedString<Enums.RCSFallbackMode>(null);

        Assert.Null(result);
    }

    [Fact]
    public void ToCommaSeparatedString_WithEmptyCollection_ReturnsNull()
    {
        var result = EnumListHelper.ToCommaSeparatedString(new List<Enums.RCSFallbackMode>());

        Assert.Null(result);
    }

    [Fact]
    public void ToCommaSeparatedString_WithSingleValue_ReturnsItsName()
    {
        var result = EnumListHelper.ToCommaSeparatedString(new List<Enums.RCSFallbackMode> { Enums.RCSFallbackMode.SMS });

        Assert.Equal("SMS", result);
    }

    [Fact]
    public void ToCommaSeparatedString_WithMultipleValues_JoinsWithCommaAndSpace()
    {
        var result = EnumListHelper.ToCommaSeparatedString(new List<Enums.RCSFallbackMode>
        {
            Enums.RCSFallbackMode.SMS,
            Enums.RCSFallbackMode.Voice,
            Enums.RCSFallbackMode.WhatsApp
        });

        Assert.Equal("SMS, Voice, WhatsApp", result);
    }

    [Fact]
    public void ToCommaSeparatedString_WorksForAnyEnumType_NotJustRCSFallbackMode()
    {
        var result = EnumListHelper.ToCommaSeparatedString(new List<Enums.WhatsAppFallbackMode>
        {
            Enums.WhatsAppFallbackMode.RCS,
            Enums.WhatsAppFallbackMode.SMS
        });

        Assert.Equal("RCS, SMS", result);
    }

    [Fact]
    public void ToCommaSeparatedString_WithTokenMapper_UsesMappedTokensInsteadOfEnumNames()
    {
        var result = EnumListHelper.ToCommaSeparatedString(
            new List<Enums.RCSFallbackMode> { Enums.RCSFallbackMode.SMS, Enums.RCSFallbackMode.WhatsApp },
            EnumListHelper.ToFallbackModeWireToken);

        Assert.Equal("SMS, WAPP", result);
    }

    [Fact]
    public void ToFallbackModeWireToken_WithWhatsApp_ReturnsWAPP()
    {
        Assert.Equal("WAPP", EnumListHelper.ToFallbackModeWireToken(Enums.RCSFallbackMode.WhatsApp));
        Assert.Equal("WAPP", EnumListHelper.ToFallbackModeWireToken(Enums.SMSFallbackMode.WhatsApp));
    }

    [Fact]
    public void ToFallbackModeWireToken_WithNonWhatsAppValue_ReturnsItsOwnName()
    {
        Assert.Equal("SMS", EnumListHelper.ToFallbackModeWireToken(Enums.RCSFallbackMode.SMS));
        Assert.Equal("Voice", EnumListHelper.ToFallbackModeWireToken(Enums.RCSFallbackMode.Voice));
        Assert.Equal("None", EnumListHelper.ToFallbackModeWireToken(Enums.RCSFallbackMode.None));
        Assert.Equal("RCS", EnumListHelper.ToFallbackModeWireToken(Enums.WhatsAppFallbackMode.RCS));
    }
}