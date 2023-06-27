using System.Xml.Serialization;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Contact.Group.Dto
{
    [XmlType(TypeName = "root")]
    public class ContactGroupApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }

        public ContactModel Contact { get; set; }

        public GroupModel Group { get; set; }

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }
    }
}
