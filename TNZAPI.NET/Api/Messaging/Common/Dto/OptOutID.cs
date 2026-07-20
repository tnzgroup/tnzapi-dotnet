using System.Text.Json.Serialization;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Api.Messaging.Common.Dto
{
    [JsonConverter(typeof(OptOutIDJsonConverter))]
    public record OptOutID
    {
        public string? Value { get; set; }

        public OptOutID(string value)
        {
            Value = value;
        }

        public OptOutID(object obj)
        {
            if (obj is string s)
            {
                Value = s;
                return;
            }
            if (obj is OptOutID optOutID)
            {
                Value = optOutID;
                return;
            }

            throw new Exception($"Unsupported type - {obj}");
        }

        public OptOutID()
        {
        }

        public static implicit operator string?(OptOutID optOutID) => optOutID?.Value;

        public override string? ToString()
        {
            if (Value is null)
            {
                return null;
            }

            return $"{Value}";
        }
    }

    internal sealed class OptOutIDJsonConverter : IdValueJsonConverter<OptOutID>
    {
        public OptOutIDJsonConverter() : base(v => new OptOutID(v))
        {
        }
    }
}