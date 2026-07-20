using System.Text.Json;
using TNZAPI.NET.Api.Configuration.OptOut.Dto;

namespace TNZAPI.NET.Tests.Api.Configuration.OptOut.Dto;

public class OptOutDtoJsonTests
{
    [Fact]
    public void OptOutModel_SerializesFieldsDirectlyAsWireBody()
    {
        var model = new OptOutModel
        {
            DestType = "SMS",
            Destination = "+6421003004",
            SubAccount = "Business Unit One"
        };

        var json = JsonSerializer.Serialize(model);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("SMS", doc.RootElement.GetProperty("DestType").GetString());
        Assert.Equal("+6421003004", doc.RootElement.GetProperty("Destination").GetString());
        Assert.Equal("Business Unit One", doc.RootElement.GetProperty("SubAccount").GetString());
    }

    [Fact]
    public void OptOutApiResult_DeserializesFlatOptOutDetailJson()
    {
        var json = "{\"ID\":\"8000000a-f002-4007-b00a-d00000000001\",\"DestType\":\"SMS\",\"Destination\":\"+6421003004\"}";

        var result = JsonSerializer.Deserialize<OptOutApiResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        Assert.NotNull(result.ID);
        Assert.Equal("8000000a-f002-4007-b00a-d00000000001", result.ID);
        Assert.Equal("SMS", result.DestType);
        Assert.Equal("+6421003004", result.Destination);
    }

    [Fact]
    public void OptOutListApiResult_DeserializesOptOutsArray()
    {
        var json = "{\"TotalRecords\":1,\"OptOuts\":[{\"DestType\":\"Email\",\"Destination\":\"john.doe@example.com\"}]}";

        var result = JsonSerializer.Deserialize<OptOutListApiResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        Assert.Equal(1, result.TotalRecords);
        Assert.Single(result.OptOuts);
        Assert.Equal("Email", result.OptOuts[0].DestType);
    }

    [Fact]
    public void OptOutBatchModel_SerializesDestinationsAndContactIDsArrays()
    {
        var model = new OptOutBatchModel
        {
            DestType = "SMS,Email",
            Destinations = new List<string> { "+6421003004", "john.doe@example.com" }
        };

        var json = JsonSerializer.Serialize(model);
        using var doc = JsonDocument.Parse(json);

        Assert.Equal("SMS,Email", doc.RootElement.GetProperty("DestType").GetString());
        var destinations = doc.RootElement.GetProperty("Destinations");
        Assert.Equal(2, destinations.GetArrayLength());
    }
}