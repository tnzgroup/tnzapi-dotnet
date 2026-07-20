using System.Text.Json;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Tests.Helpers.Json;

public class FlexiblePriceJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new FlexiblePriceJsonConverter() }
    };

    [Fact]
    public void ReadsJsonString()
    {
        var result = JsonSerializer.Deserialize<string?>("\"0.10\"", Options);

        Assert.Equal("0.10", result);
    }

    [Fact]
    public void ReadsJsonNumber()
    {
        var result = JsonSerializer.Deserialize<string?>("0.10", Options);

        Assert.Equal("0.10", result);
    }

    [Fact]
    public void ReadsJsonNull()
    {
        var result = JsonSerializer.Deserialize<string?>("null", Options);

        Assert.Null(result);
    }

    [Fact]
    public void ThrowsOnUnsupportedToken()
    {
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<string?>("true", Options));
    }

    [Fact]
    public void WritesStringValueVerbatim()
    {
        var json = JsonSerializer.Serialize("0.10", Options);

        Assert.Equal("\"0.10\"", json);
    }

    [Fact]
    public void WritesNullAsJsonNull()
    {
        var json = JsonSerializer.Serialize((string?)null, Options);

        Assert.Equal("null", json);
    }
}