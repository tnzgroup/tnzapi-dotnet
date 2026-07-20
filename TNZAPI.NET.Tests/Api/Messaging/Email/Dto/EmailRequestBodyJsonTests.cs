using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Email;

namespace TNZAPI.NET.Tests.Api.Messaging.Email.Dto;

public class EmailRequestBodyJsonTests
{
    [Fact]
    public void EmailDestinationBody_SerializesEmailAddressField()
    {
        var destination = new EmailDestinationBody
        {
            EmailAddress = "john.doe@example.com",
            FirstName = "John"
        };

        var json = JsonSerializer.Serialize(destination);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("john.doe@example.com", doc.RootElement.GetProperty("EmailAddress").GetString());
        Assert.Equal("John", doc.RootElement.GetProperty("FirstName").GetString());
    }

    [Fact]
    public void EmailFileBody_SerializesNameAndData()
    {
        var file = new EmailFileBody
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