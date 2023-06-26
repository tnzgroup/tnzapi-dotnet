using System.Xml.Serialization;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Group.Contact.Dto
{
    [XmlType(TypeName = "root")]
    public class GroupContactApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }

        public GroupModel Group { get; set; }

        public ContactModel Contact { get; set; }

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }
    }
}
