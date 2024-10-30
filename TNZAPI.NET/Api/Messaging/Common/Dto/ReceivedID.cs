using System.Xml.Serialization;

namespace TNZAPI.NET.Api.Messaging.Common.Dto
{
    [XmlType(TypeName = "ReceivedID")]
    public record ReceivedID
		{
        [XmlText]
        public string Value { get; set; }

        public ReceivedID(string value)
        {
            Value = value;
        }

        // Required for XmlSerializer
        public ReceivedID()
        {
        }

        public static implicit operator string(ReceivedID receivedID) => receivedID?.Value;

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
