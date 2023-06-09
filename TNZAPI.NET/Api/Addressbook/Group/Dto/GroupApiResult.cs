using System.Xml.Serialization;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Group.Dto
{
    [XmlType(TypeName = "root")]
    public class GroupApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public GroupModel Group { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}
