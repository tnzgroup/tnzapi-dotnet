using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientTTSTests
{
    [Fact]
    public void Client_ExposesMessagingTTS()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Messaging.TTS);
    }

    [Fact]
    public void Client_ExposesActionsTTS()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Actions.TTS);
    }
}