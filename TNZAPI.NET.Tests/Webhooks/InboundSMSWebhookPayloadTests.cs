using System.Text.Json;
using TNZAPI.NET.Webhooks;

namespace TNZAPI.NET.Tests.Webhooks;

public class InboundSMSWebhookPayloadTests
{
    [Fact]
    public void DeserializesRealisticInboundSMSWebhookPayload()
    {
        var json = """
        {
            "Version": "v3.00",
            "Sender": "application@domain.com",
            "APIKey": "ta8wr7ymd",
            "Type": "SMSReply",
            "Destination": "+6421000001",
            "ContactID": "6000000b-f002-4007-b00a-c00000000001",
            "ReceivedID": "a1b2c3d4-e5f6-7890-1234-567890abcdee",
            "MessageID": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
            "SubAccount": "Business Unit One",
            "Department": "Team Alpha",
            "JobNumber": "10AB20CE",
            "SentTimeLocal": "2026-07-09 10:34:30",
            "SendTimeUTC": "2026-07-08 22:34:30",
            "SentTimeUTC_RFC3339": "2026-07-08T22:34:30.472Z",
            "Status": "RECEIVED",
            "Result": "RECEIVED",
            "Message": "This is a received message from a mobile phone.",
            "Price": null,
            "Detail": "InputToNumber:021-777909",
            "URL": "https://www.example.com/data"
        }
        """;

        var result = JsonSerializer.Deserialize<InboundSMSWebhookPayload>(json)!;

        Assert.Equal("SMSReply", result.Type);
        Assert.NotNull(result.ContactID);
        Assert.Equal("6000000b-f002-4007-b00a-c00000000001", result.ContactID);
        Assert.NotNull(result.ReceivedID);
        Assert.Equal("a1b2c3d4-e5f6-7890-1234-567890abcdee", result.ReceivedID);
        Assert.NotNull(result.MessageID);
        Assert.Equal("a1b2c3d4-e5f6-7890-1234-567890abcdef", result.MessageID);
        Assert.Equal(new DateTime(2026, 7, 9, 10, 34, 30), result.SentTimeLocal);
        Assert.Equal("RECEIVED", result.Status);
        Assert.Equal("This is a received message from a mobile phone.", result.Message);
        Assert.Null(result.Price);
        Assert.Equal("InputToNumber:021-777909", result.Detail);
    }
}