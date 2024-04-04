using System.Xml.Serialization;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Messaging.Common
{
    [XmlType(TypeName = "root")]
    public class MessageApiResult : IApiResult
    {
        [XmlElement("Result")]
        public Enums.ResultCode Result { get; set; }

        [XmlElement("MessageID")]
        public MessageID MessageID { get; set; }

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }
    }
}
