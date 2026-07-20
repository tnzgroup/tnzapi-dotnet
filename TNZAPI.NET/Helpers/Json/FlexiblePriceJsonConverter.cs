using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TNZAPI.NET.Helpers.Json
{
    // Used by ResultWebhookPayload/InboundSMSWebhookPayload only. The Status endpoint's Price is
    // confirmed via the live API response to always be a JSON number or null, so those
    // *StatusApiResult/*RecipientResult classes use a plain decimal? with no converter — a JSON
    // number token parses into decimal just as directly as it does into double, and decimal is the
    // correct C# type for a monetary value once it reaches the public SDK surface, avoiding
    // float-arithmetic surprises for any consumer code that sums/compares/formats Price afterwards.
    // The outbound webhook callback's Price format hasn't been directly confirmed the same way, and
    // an existing test already asserted a JSON string ("0.10") for it — so this stays tolerant of
    // either representation rather than risk breaking real webhook consumers on an unconfirmed
    // assumption.
    internal sealed class FlexiblePriceJsonConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Null => null,
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetDecimal().ToString(CultureInfo.InvariantCulture),
                _ => throw new JsonException($"Unable to parse '{reader.TokenType}' token as Price.")
            };
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value);
            }
        }
    }
}