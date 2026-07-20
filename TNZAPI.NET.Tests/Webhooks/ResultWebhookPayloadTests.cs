using System.Text.Json;
using TNZAPI.NET.Core;
using TNZAPI.NET.Webhooks;

namespace TNZAPI.NET.Tests.Webhooks;

public class ResultWebhookPayloadTests
{
    [Fact]
    public void DeserializesRealisticResultWebhookPayload()
    {
        var json = """
        {
            "Version": "v3.00",
            "Sender": "application@domain.com",
            "APIKey": "ta8wr7ymd",
            "Type": "SMS",
            "Destination": "+6421000001",
            "ContactID": "6000000b-f002-4007-b00a-c00000000001",
            "ReceivedID": null,
            "MessageID": "a1b2c3d4-e5f6-7890-1234-567890abcdee",
            "SubAccount": "Business Unit One",
            "Department": "Team Alpha",
            "JobNumber": "10AB20CE",
            "SentTimeLocal": "2026-07-09 10:34:30",
            "SendTimeUTC": "2026-07-08 22:34:30",
            "SentTimeUTC_RFC3339": "2026-07-08T22:34:30.472Z",
            "Status": "Success",
            "Result": "delivered",
            "Message": null,
            "Price": "0.10",
            "Detail": "SMSParts:2",
            "URL": "https://www.example.com/data"
        }
        """;

        var result = JsonSerializer.Deserialize<ResultWebhookPayload>(json)!;

        Assert.Equal("v3.00", result.Version);
        Assert.Equal("application@domain.com", result.Sender);
        Assert.Equal("SMS", result.Type);
        Assert.Equal("+6421000001", result.Destination);
        Assert.NotNull(result.ContactID);
        Assert.Equal("6000000b-f002-4007-b00a-c00000000001", result.ContactID);
        Assert.Null(result.ReceivedID);
        Assert.NotNull(result.MessageID);
        Assert.Equal("a1b2c3d4-e5f6-7890-1234-567890abcdee", result.MessageID);
        Assert.Equal("10AB20CE", result.JobNumber);
        Assert.Equal(new DateTime(2026, 7, 9, 10, 34, 30), result.SentTimeLocal);
        Assert.Equal(Enums.MessageStatus.Success, result.Status);
        Assert.Equal("delivered", result.Result);
        Assert.Null(result.Message);
        Assert.Equal("0.10", result.Price);
        Assert.Equal("SMSParts:2", result.Detail);
    }

    [Fact]
    public void DeserializesRFC3339TimestampCorrectly()
    {
        var json = "{\"SentTimeUTC_RFC3339\":\"2026-07-08T22:34:30.472Z\"}";

        var result = JsonSerializer.Deserialize<ResultWebhookPayload>(json)!;

        Assert.NotNull(result.SentTimeUTC_RFC3339);
    }
}