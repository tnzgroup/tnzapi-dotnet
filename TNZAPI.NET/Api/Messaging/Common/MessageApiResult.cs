using System.Xml.Serialization;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Messaging.Common
{
    [XmlType(TypeName = "root")]
    public class MessageApiResult : IApiResult
    {
        [XmlElement("Result")]
        public Enums.ResultCode Result { get; set; }

        [XmlElement("MessageID")]
        public string MessageID { get; set; } = "";

        [XmlArray("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }
    }
}
