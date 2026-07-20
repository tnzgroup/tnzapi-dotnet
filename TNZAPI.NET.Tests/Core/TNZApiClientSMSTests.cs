using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientSMSTests
{
    [Fact]
    public void Client_ExposesMessagingSMS()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Messaging);
        Assert.NotNull(client.Messaging.SMS);
    }

    [Fact]
    public void Client_ExposesActionsSMS()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Actions);
        Assert.NotNull(client.Actions.SMS);
    }
}