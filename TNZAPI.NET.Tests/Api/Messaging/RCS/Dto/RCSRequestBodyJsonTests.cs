using System.Text.Json;
using TNZAPI.NET.Api.Messaging.RCS;

namespace TNZAPI.NET.Tests.Api.Messaging.RCS.Dto;

public class RCSRequestBodyJsonTests
{
    [Fact]
    public void RCSDestinationBody_SerializesToNumberField()
    {
        var destination = new RCSDestinationBody
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
    public void RCSFileBody_SerializesNameAndData()
    {
        var file = new RCSFileBody
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