using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientRCSTests
{
    [Fact]
    public void Client_ExposesMessagingRCS()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Messaging.RCS);
    }

    [Fact]
    public void Client_ExposesActionsRCS()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Actions.RCS);
    }
}