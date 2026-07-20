using System.Text.Json;
using TNZAPI.NET.Api.Messaging.SMS;

namespace TNZAPI.NET.Tests.Api.Messaging.SMS.Dto;

public class SMSRequestBodyJsonTests
{
    [Fact]
    public void SMSDestinationBody_SerializesToNumberField_NotMobileNumber()
    {
        var destination = new SMSDestinationBody
        {
            ToNumber = "+64211234567",
            Attention = "Jane Smith"
        };

        var json = JsonSerializer.Serialize(destination);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("+64211234567", doc.RootElement.GetProperty("ToNumber").GetString());
        Assert.Equal("Jane Smith", doc.RootElement.GetProperty("Attention").GetString());
        Assert.False(doc.RootElement.TryGetProperty("MobileNumber", out _));
    }

    [Fact]
    public void SMSFileBody_SerializesNameAndData()
    {
        var file = new SMSFileBody
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