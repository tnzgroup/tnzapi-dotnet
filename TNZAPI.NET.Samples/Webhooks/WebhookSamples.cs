using System.Text.Json;
using TNZAPI.NET.Webhooks;

namespace TNZAPI.NET.Samples.Webhooks
{
    /// <summary>
    /// Reference code demonstrating how to deserialize TNZ's inbound webhook payloads
    /// (ResultWebhookPayload / InboundSMSWebhookPayload) in your own webhook receiver endpoint.
    /// This class is not a runnable program. See README.md#inbound-webhooks for the full reference.
    /// </summary>
    // Webhooks are INBOUND - TNZ's servers POST these payloads to your own server
    // (configured via a Send call's WebhookCallbackURL, or your Sender's Dashboard settings).
    // There is no client call here - just deserialize the request body your webhook receiver
    // endpoint gets, using these convenience payload models.
    public class WebhookSamples
    {
        // Call this from your own webhook receiver's action/handler, passing the raw request body.
        public ResultWebhookPayload? ParseResultWebhook(string requestBody)
        {
            var payload = JsonSerializer.Deserialize<ResultWebhookPayload>(requestBody);

            if (payload is not null)
            {
                Console.WriteLine($"{payload.Type} to {payload.Destination}: {payload.Status} ({payload.Result})");
                Console.WriteLine($"    -> MessageID: {payload.MessageID}");
                Console.WriteLine($"    -> SentTimeLocal: {payload.SentTimeLocal}");
            }

            return payload;
        }

        public InboundSMSWebhookPayload? ParseInboundSMSWebhook(string requestBody)
        {
            var payload = JsonSerializer.Deserialize<InboundSMSWebhookPayload>(requestBody);

            if (payload is not null)
            {
                Console.WriteLine($"Inbound SMS from {payload.Destination}: {payload.Message}");
                Console.WriteLine($"    -> Type: {payload.Type}");       // 'SMSReply' if matched to an outbound message, else 'SMSInbound'
                Console.WriteLine($"    -> ReceivedID: {payload.ReceivedID}");
            }

            return payload;
        }

        // Demonstrates both payload types round-tripping through the SDK's own attribute-based
        // JSON converters (FlexibleDateTimeJsonConverter, JsonStringEnumConverter) - no special
        // JsonSerializerOptions configuration needed on your end.
        public void Demo()
        {
            var resultWebhookJson = @"
            {
                ""Version"": ""v3.00"",
                ""Sender"": ""application@domain.com"",
                ""Type"": ""SMS"",
                ""Destination"": ""+6421000001"",
                ""MessageID"": ""a1b2c3d4-e5f6-7890-1234-567890abcdee"",
                ""SentTimeLocal"": ""2026-07-09 10:34:30"",
                ""Status"": ""Success"",
                ""Result"": ""delivered"",
                ""Price"": ""0.10""
            }";

            ParseResultWebhook(resultWebhookJson);

            var inboundSmsWebhookJson = @"
            {
                ""Version"": ""v3.00"",
                ""Sender"": ""application@domain.com"",
                ""Type"": ""SMSReply"",
                ""Destination"": ""+6421000001"",
                ""ReceivedID"": ""a1b2c3d4-e5f6-7890-1234-567890abcdee"",
                ""MessageID"": ""a1b2c3d4-e5f6-7890-1234-567890abcdef"",
                ""SentTimeLocal"": ""2026-07-09 10:34:30"",
                ""Status"": ""RECEIVED"",
                ""Result"": ""RECEIVED"",
                ""Message"": ""This is a received message from a mobile phone.""
            }";

            ParseInboundSMSWebhook(inboundSmsWebhookJson);
        }
    }
}