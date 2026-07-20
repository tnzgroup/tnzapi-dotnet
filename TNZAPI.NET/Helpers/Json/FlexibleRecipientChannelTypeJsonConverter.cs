using System.Text.Json;
using System.Text.Json.Serialization;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Helpers.Json
{
    // Recipients[].Type doesn't use the SDK's own channel names on the wire — confirmed via the live
    // API response. "Text" means SMS; "Voice" covers both TTS and interactive Voice calls (the API
    // doesn't distinguish them at this level). WhatsApp/RCS aren't confirmed the same way, so they're
    // included as a best-effort guess rather than left unmapped. Anything unrecognized falls back to
    // Unknown instead of throwing, since TNZ's exact wire vocabulary here has already proven to not
    // match the SDK's assumptions once.
    internal sealed class FlexibleRecipientChannelTypeJsonConverter : JsonConverter<Enums.RecipientChannelType>
    {
        public override Enums.RecipientChannelType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            return value?.ToLowerInvariant() switch
            {
                "sms" => Enums.RecipientChannelType.SMS,
                "text" => Enums.RecipientChannelType.SMS,
                "email" => Enums.RecipientChannelType.Email,
                "voice" => Enums.RecipientChannelType.Voice,
                "fax" => Enums.RecipientChannelType.Fax,
                "whatsapp" => Enums.RecipientChannelType.WhatsApp,
                "rcs" => Enums.RecipientChannelType.RCS,
                _ => Enums.RecipientChannelType.Unknown
            };
        }

        public override void Write(Utf8JsonWriter writer, Enums.RecipientChannelType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}