using System.Text.Json;
using System.Text.Json.Serialization;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Addressbook.Contact.Dto;

public class ContactDtoJsonTests
{
    [Fact]
    public void ContactModel_SerializesFieldsDirectlyAsWireBody()
    {
        var model = new ContactModel
        {
            FirstName = "John",
            LastName = "Doe",
            EmailAddress = "john.doe@example.com",
            ViewBy = Enums.ViewEditByOptions.Account,
            AccessControl = Enums.AccessControlLevel.Granted
        };

        var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        var json = JsonSerializer.Serialize(model, options);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("John", doc.RootElement.GetProperty("FirstName").GetString());
        Assert.Equal("Doe", doc.RootElement.GetProperty("LastName").GetString());
        Assert.Equal("john.doe@example.com", doc.RootElement.GetProperty("EmailAddress").GetString());
        Assert.Equal("Account", doc.RootElement.GetProperty("ViewBy").GetString());
        Assert.Equal("Granted", doc.RootElement.GetProperty("AccessControl").GetString());
    }

    [Fact]
    public void ContactApiResult_DeserializesFlatContactDetailJson()
    {
        var json = "{\"ContactID\":\"123e4567-e89b-12d3-a456-426614174000\",\"FirstName\":\"John\",\"Owner\":\"COM\\\\DOMAIN\\\\APP\"}";

        var result = JsonSerializer.Deserialize<ContactApiResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        Assert.NotNull(result.ContactID);
        Assert.Equal("123e4567-e89b-12d3-a456-426614174000", result.ContactID);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("COM\\DOMAIN\\APP", result.Owner);
    }

    [Fact]
    public void ContactListApiResult_DeserializesContactsArray()
    {
        var json = "{\"TotalRecords\":2,\"Contacts\":[{\"FirstName\":\"John\"},{\"FirstName\":\"Jane\"}]}";

        var result = JsonSerializer.Deserialize<ContactListApiResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        Assert.Equal(2, result.TotalRecords);
        Assert.Equal(2, result.Contacts.Count);
        Assert.Equal("John", result.Contacts[0].FirstName);
        Assert.Equal("Jane", result.Contacts[1].FirstName);
    }
}