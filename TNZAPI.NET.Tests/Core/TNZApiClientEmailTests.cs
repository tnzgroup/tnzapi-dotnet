using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientEmailTests
{
    [Fact]
    public void Client_ExposesMessagingEmail()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Messaging.Email);
    }

    [Fact]
    public void Client_ExposesActionsEmail()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Actions.Email);
    }
}