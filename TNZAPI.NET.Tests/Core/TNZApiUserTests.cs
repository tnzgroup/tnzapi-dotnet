using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiUserTests
{
    [Fact]
    public void AuthToken_DefaultsToEmptyString()
    {
        var user = new TNZApiUser();

        Assert.Equal(string.Empty, user.AuthToken);
    }

    [Fact]
    public void AuthToken_IsSettable()
    {
        var user = new TNZApiUser
        {
            AuthToken = "test-token"
        };

        Assert.Equal("test-token", user.AuthToken);
    }

    [Fact]
    public void AuthToken_FallsBackToEnvironmentVariable_WhenNotSetExplicitly()
    {
        Environment.SetEnvironmentVariable("TNZ_AUTH_TOKEN", "env-token-value");
        try
        {
            var user = new TNZApiUser();

            Assert.Equal("env-token-value", user.AuthToken);
        }
        finally
        {
            Environment.SetEnvironmentVariable("TNZ_AUTH_TOKEN", null);
        }
    }

    [Fact]
    public void AuthToken_ExplicitlySetValue_OverridesEnvironmentVariable()
    {
        Environment.SetEnvironmentVariable("TNZ_AUTH_TOKEN", "env-token-value");
        try
        {
            var user = new TNZApiUser { AuthToken = "explicit-token" };

            Assert.Equal("explicit-token", user.AuthToken);
        }
        finally
        {
            Environment.SetEnvironmentVariable("TNZ_AUTH_TOKEN", null);
        }
    }
}