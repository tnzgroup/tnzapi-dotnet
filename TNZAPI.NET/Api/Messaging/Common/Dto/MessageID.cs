using System.Xml.Serialization;

namespace TNZAPI.NET.Api.Messaging.Common.Dto
{
    [XmlType(TypeName = "MessageID")]
    public record MessageID
    {
        [XmlText]
        public string Value { get; set; }

        public MessageID(string value)
        {
            Value = value;
        }

        // Required for XmlSerializer
        public MessageID()
        {
        }

        public static implicit operator string(MessageID messageID) => messageID.Value;

        public override string ToString()
        {
            if (Value is null)
            {
                return null;
            }

            return $"{Value}";
        }
    }
}
