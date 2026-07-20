using System.Text.Json.Serialization;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Webhooks
{
    public class InboundSMSWebhookPayload
    {
        public string? Version { get; set; }
        public string? Sender { get; set; }
        public string? APIKey { get; set; }
        public string? Type { get; set; }
        public string? Destination { get; set; }
        public ContactID? ContactID { get; set; }
        public ReceivedID? ReceivedID { get; set; }
        public MessageID? MessageID { get; set; }
        public string? SubAccount { get; set; }
        public string? Department { get; set; }
        public string? JobNumber { get; set; }

        [JsonConverter(typeof(FlexibleDateTimeJsonConverter))]
        public DateTime? SentTimeLocal { get; set; }

        [JsonConverter(typeof(FlexibleDateTimeJsonConverter))]
        public DateTime? SendTimeUTC { get; set; }

        [JsonConverter(typeof(FlexibleDateTimeJsonConverter))]
        public DateTime? SentTimeUTC_RFC3339 { get; set; }

        public string? Status { get; set; }
        public string? Result { get; set; }
        public string? Message { get; set; }

        [JsonConverter(typeof(FlexiblePriceJsonConverter))]
        public string? Price { get; set; }
        public string? Detail { get; set; }
        public string? URL { get; set; }
    }
}