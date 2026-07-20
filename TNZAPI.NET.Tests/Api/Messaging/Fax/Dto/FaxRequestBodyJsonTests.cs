using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Fax;

namespace TNZAPI.NET.Tests.Api.Messaging.Fax.Dto;

public class FaxRequestBodyJsonTests
{
    [Fact]
    public void FaxDestinationBody_SerializesToNumberField()
    {
        var destination = new FaxDestinationBody
        {
            ToNumber = "+6495006000",
            FirstName = "John"
        };

        var json = JsonSerializer.Serialize(destination);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("+6495006000", doc.RootElement.GetProperty("ToNumber").GetString());
        Assert.Equal("John", doc.RootElement.GetProperty("FirstName").GetString());
    }

    [Fact]
    public void FaxFileBody_SerializesNameAndData()
    {
        var file = new FaxFileBody
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