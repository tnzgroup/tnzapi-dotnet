using System.Xml.Serialization;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Actions.Pacing.Dto
{
    [XmlType(TypeName = "root")]
    public class PacingApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }

        public MessageID MessageID { get; set; }
        public Enums.StatusCode Status { get; set; } = Enums.StatusCode.Unknown;
        public string JobNum { get; set; } = "";
        public string Action { get; set; } = "";

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }

        public string GetStatusString()
        {
            return Enum.GetName(typeof(Enums.StatusCode), Status);
        }

        public string GetStatusString(Enums.StatusCode code)
        {
            return Enum.GetName(typeof(Enums.StatusCode), code);
        }
    }
}
