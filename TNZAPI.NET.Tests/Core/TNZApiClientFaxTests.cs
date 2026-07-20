using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientFaxTests
{
    [Fact]
    public void Client_ExposesMessagingFax()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Messaging.Fax);
    }

    [Fact]
    public void Client_ExposesActionsFax()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Actions.Fax);
    }
}
