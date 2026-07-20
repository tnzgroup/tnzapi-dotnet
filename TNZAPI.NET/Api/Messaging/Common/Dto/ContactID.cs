using System.Text.Json.Serialization;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Api.Messaging.Common.Dto
{
    [JsonConverter(typeof(ContactIDJsonConverter))]
    public record ContactID
    {
        public string? Value { get; set; }

        public ContactID(string value)
        {
            Value = value;
        }

        public ContactID(object obj)
        {
            if (obj is string s)
            {
                Value = s;
                return;
            }
            if (obj is ContactID contactID)
            {
                Value = contactID;
                return;
            }

            throw new Exception($"Unsupported type - {obj}");
        }

        public ContactID()
        {
        }

        public static implicit operator string?(ContactID contactID) => contactID?.Value;

        public override string? ToString()
        {
            if (Value is null)
            {
                return null;
            }

            return $"{Value}";
        }
    }

    internal sealed class ContactIDJsonConverter : IdValueJsonConverter<ContactID>
    {
        public ContactIDJsonConverter() : base(v => new ContactID(v))
        {
        }
    }
}