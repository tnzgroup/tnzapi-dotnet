using System.Xml.Serialization;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Addressbook.Group.Dto
{
    [XmlType(TypeName = "root")]
    public class GroupApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; } = ResultCode.Failed;
        public GroupModel Group { get; set; }

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }
    }
}
