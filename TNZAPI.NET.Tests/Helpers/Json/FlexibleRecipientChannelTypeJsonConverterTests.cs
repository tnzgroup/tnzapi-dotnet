using System.Text.Json;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Tests.Helpers.Json;

public class FlexibleRecipientChannelTypeJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new FlexibleRecipientChannelTypeJsonConverter() }
    };

    [Theory]
    [InlineData("\"sms\"", Enums.RecipientChannelType.SMS)]
    [InlineData("\"Text\"", Enums.RecipientChannelType.SMS)]
    [InlineData("\"TEXT\"", Enums.RecipientChannelType.SMS)]
    [InlineData("\"email\"", Enums.RecipientChannelType.Email)]
    [InlineData("\"Voice\"", Enums.RecipientChannelType.Voice)]
    [InlineData("\"fax\"", Enums.RecipientChannelType.Fax)]
    [InlineData("\"whatsapp\"", Enums.RecipientChannelType.WhatsApp)]
    [InlineData("\"rcs\"", Enums.RecipientChannelType.RCS)]
    public void ReadsKnownWireValues(string json, Enums.RecipientChannelType expected)
    {
        var result = JsonSerializer.Deserialize<Enums.RecipientChannelType>(json, Options);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("\"Postal\"")]
    [InlineData("\"File\"")]
    [InlineData("\"\"")]
    public void FallsBackToUnknownForUnrecognizedValues(string json)
    {
        // TNZ's wire vocabulary here includes values (e.g. Postal/File) the SDK doesn't model — these
        // must degrade to Unknown rather than throw, since it has already proven to not match the
        // SDK's assumptions once (the "Text" -> SMS mapping was the original surprise).
        var result = JsonSerializer.Deserialize<Enums.RecipientChannelType>(json, Options);

        Assert.Equal(Enums.RecipientChannelType.Unknown, result);
    }

    [Fact]
    public void ReadsJsonNullAsUnknown()
    {
        var result = JsonSerializer.Deserialize<Enums.RecipientChannelType>("null", Options);

        Assert.Equal(Enums.RecipientChannelType.Unknown, result);
    }

    [Fact]
    public void WritesEnumNameVerbatim()
    {
        var json = JsonSerializer.Serialize(Enums.RecipientChannelType.WhatsApp, Options);

        Assert.Equal("\"WhatsApp\"", json);
    }
}