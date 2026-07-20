using System.Text.Json.Serialization;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Api.Messaging.Common.Dto
{
    [JsonConverter(typeof(MessageIDJsonConverter))]
    public record MessageID
    {
        public string? Value { get; set; }

        public MessageID(string value)
        {
            Value = value;
        }

        // Required for object initializer / converter round-trip
        public MessageID()
        {
        }

        public static implicit operator string?(MessageID messageID) => messageID?.Value;

        public override string? ToString()
        {
            if (Value is null)
            {
                return null;
            }

            return $"{Value}";
        }
    }

    internal sealed class MessageIDJsonConverter : IdValueJsonConverter<MessageID>
    {
        public MessageIDJsonConverter() : base(v => new MessageID(v))
        {
        }
    }
}