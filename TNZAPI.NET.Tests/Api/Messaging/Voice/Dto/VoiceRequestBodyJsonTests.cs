using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Voice;

namespace TNZAPI.NET.Tests.Api.Messaging.Voice.Dto;

public class VoiceRequestBodyJsonTests
{
    [Fact]
    public void VoiceDestinationBody_SerializesMainPhoneField()
    {
        var destination = new VoiceDestinationBody
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
    public void VoiceKeypadBody_SerializesToneAndPlay()
    {
        var keypad = new VoiceKeypadBody
        {
            Tone = 1,
            Play = "base64audiodata"
        };

        var json = JsonSerializer.Serialize(keypad);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal(1, doc.RootElement.GetProperty("Tone").GetInt32());
        Assert.Equal("base64audiodata", doc.RootElement.GetProperty("Play").GetString());
    }
}