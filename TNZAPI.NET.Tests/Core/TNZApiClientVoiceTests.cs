using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientVoiceTests
{
    [Fact]
    public void Client_ExposesMessagingVoice()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Messaging.Voice);
    }

    [Fact]
    public void Client_ExposesActionsVoice()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Actions.Voice);
    }
}