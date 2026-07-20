using System.Text.Json;
using System.Text.Json.Serialization;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Addressbook.Group.Dto;

public class GroupDtoJsonTests
{
    [Fact]
    public void GroupModel_SerializesFieldsDirectlyAsWireBody()
    {
        var model = new GroupModel
        {
            GroupName = "VIP Customers",
            SubAccount = "Business Unit One",
            ViewEditBy = Enums.ViewEditByOptions.Account
        };

        var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        var json = JsonSerializer.Serialize(model, options);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("VIP Customers", doc.RootElement.GetProperty("GroupName").GetString());
        Assert.Equal("Business Unit One", doc.RootElement.GetProperty("SubAccount").GetString());
        Assert.Equal("Account", doc.RootElement.GetProperty("ViewEditBy").GetString());
    }

    [Fact]
    public void GroupApiResult_DeserializesFlatGroupDetailJson()
    {
        var json = "{\"GroupID\":\"223e4567-e89b-12d3-a456-426614175000\",\"GroupName\":\"VIP\"}";

        var result = JsonSerializer.Deserialize<GroupApiResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        Assert.NotNull(result.GroupID);
        Assert.Equal("223e4567-e89b-12d3-a456-426614175000", result.GroupID);
        Assert.Equal("VIP", result.GroupName);
    }

    [Fact]
    public void GroupListApiResult_DeserializesGroupsArray()
    {
        var json = "{\"TotalRecords\":1,\"Groups\":[{\"GroupName\":\"VIP\"}]}";

        var result = JsonSerializer.Deserialize<GroupListApiResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        Assert.Equal(1, result.TotalRecords);
        Assert.Single(result.Groups);
        Assert.Equal("VIP", result.Groups[0].GroupName);
    }
}