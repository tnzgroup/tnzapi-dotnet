using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiConfigTests
{
    [Fact]
    public void Version_Is300()
    {
        Assert.Equal("3.00", TNZApiConfig.Version);
    }

    [Fact]
    public void UserAgent_ReflectsVersion()
    {
        Assert.Equal("TNZAPI.NET-3.00", TNZApiConfig.UserAgent);
    }

    [Fact]
    public void Domain_DefaultsToProductionHost_WhenEnvVarNotSet()
    {
        Assert.False(string.IsNullOrEmpty(TNZApiConfig.Domain));
    }

    [Fact]
    public void Domain_ReReadsEnvironmentVariable_OnEveryAccess()
    {
        Environment.SetEnvironmentVariable("TNZ_API_URL", "https://api.custom-override.example.com");
        try
        {
            Assert.Equal("https://api.custom-override.example.com", TNZApiConfig.Domain);
        }
        finally
        {
            Environment.SetEnvironmentVariable("TNZ_API_URL", null);
        }

        Assert.Equal("https://api.tnz.co.nz", TNZApiConfig.Domain);
    }

    [Fact]
    public void Domain_FallsBackToProductionHost_WhenEnvVarIsSetButEmpty()
    {
        Environment.SetEnvironmentVariable("TNZ_API_URL", "");
        try
        {
            Assert.Equal("https://api.tnz.co.nz", TNZApiConfig.Domain);
        }
        finally
        {
            Environment.SetEnvironmentVariable("TNZ_API_URL", null);
        }
    }

    [Fact]
    public void Domain_TrimsTrailingSlash_FromEnvVarValue()
    {
        Environment.SetEnvironmentVariable("TNZ_API_URL", "https://api.custom-override.example.com/");
        try
        {
            Assert.Equal("https://api.custom-override.example.com", TNZApiConfig.Domain);
        }
        finally
        {
            Environment.SetEnvironmentVariable("TNZ_API_URL", null);
        }
    }
}