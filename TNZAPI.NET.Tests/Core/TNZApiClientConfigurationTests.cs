using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientConfigurationTests
{
    [Fact]
    public void Client_ExposesConfigurationOptOut()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Configuration.OptOut);
    }
}