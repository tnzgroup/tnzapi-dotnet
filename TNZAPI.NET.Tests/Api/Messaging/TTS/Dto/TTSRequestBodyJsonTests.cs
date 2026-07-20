using System.Text.Json;
using TNZAPI.NET.Api.Messaging.TTS;

namespace TNZAPI.NET.Tests.Api.Messaging.TTS.Dto;

public class TTSRequestBodyJsonTests
{
    [Fact]
    public void TTSDestinationBody_SerializesMainPhoneField()
    {
        var destination = new TTSDestinationBody
        {
            MainPhone = "+6421000001",
            FirstName = "John"
        };

        var json = JsonSerializer.Serialize(destination);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("+6421000001", doc.RootElement.GetProperty("MainPhone").GetString());
        Assert.Equal("John", doc.RootElement.GetProperty("FirstName").GetString());
    }

    [Fact]
    public void TTSKeypadBody_SerializesToneAndPlay()
    {
        var keypad = new TTSKeypadBody
        {
            Tone = 1,
            Play = "Connecting you now."
        };

        var json = JsonSerializer.Serialize(keypad);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal(1, doc.RootElement.GetProperty("Tone").GetInt32());
        Assert.Equal("Connecting you now.", doc.RootElement.GetProperty("Play").GetString());
    }
}