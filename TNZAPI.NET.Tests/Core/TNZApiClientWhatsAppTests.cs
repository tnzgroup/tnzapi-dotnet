using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientWhatsAppTests
{
    [Fact]
    public void Client_ExposesMessagingWhatsApp()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Messaging.WhatsApp);
    }

    [Fact]
    public void Client_ExposesActionsWhatsApp()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Actions.WhatsApp);
    }
}