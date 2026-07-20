using System.Text.Json.Serialization;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Api.Messaging.Common.Dto
{
    [JsonConverter(typeof(ReceivedIDJsonConverter))]
    public record ReceivedID
    {
        public string? Value { get; set; }

        public ReceivedID(string value)
        {
            Value = value;
        }

        public ReceivedID()
        {
        }

        public static implicit operator string?(ReceivedID receivedID) => receivedID?.Value;

        public override string? ToString()
        {
            if (Value is null)
            {
                return null;
            }

            return $"{Value}";
        }
    }

    internal sealed class ReceivedIDJsonConverter : IdValueJsonConverter<ReceivedID>
    {
        public ReceivedIDJsonConverter() : base(v => new ReceivedID(v))
        {
        }
    }
}