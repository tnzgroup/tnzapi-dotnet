using System.Text.Json;
using TNZAPI.NET.Api.Messaging.WhatsApp;

namespace TNZAPI.NET.Tests.Api.Messaging.WhatsApp.Dto;

public class WhatsAppRequestBodyJsonTests
{
    [Fact]
    public void WhatsAppDestinationBody_SerializesToNumberField()
    {
        var destination = new WhatsAppDestinationBody
        {
            ToNumber = "+64211234567",
            FirstName = "John"
        };

        var json = JsonSerializer.Serialize(destination);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("+64211234567", doc.RootElement.GetProperty("ToNumber").GetString());
        Assert.Equal("John", doc.RootElement.GetProperty("FirstName").GetString());
    }

    [Fact]
    public void WhatsAppFileBody_SerializesNameAndData()
    {
        var file = new WhatsAppFileBody
        {
            Name = "test.pdf",
            Data = "base64content"
        };

        var json = JsonSerializer.Serialize(file);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("test.pdf", doc.RootElement.GetProperty("Name").GetString());
        Assert.Equal("base64content", doc.RootElement.GetProperty("Data").GetString());
    }
}
