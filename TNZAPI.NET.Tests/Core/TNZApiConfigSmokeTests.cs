using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiConfigSmokeTests
{
    [Fact]
    public void Domain_IsConfigured()
    {
        Assert.Equal("https://api.tnz.co.nz", TNZApiConfig.Domain);
    }
}