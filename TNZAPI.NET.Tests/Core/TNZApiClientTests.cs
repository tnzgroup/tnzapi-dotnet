using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientTests
{
    [Fact]
    public void ParameterlessConstructor_InitialisesAllFacades()
    {
        var client = new TNZApiClient();

        Assert.NotNull(client.Messaging);
        Assert.NotNull(client.Actions);
        Assert.NotNull(client.Addressbook);
        Assert.NotNull(client.Configuration);
    }
}