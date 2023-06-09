using System.Xml.Serialization;
using TNZAPI.NET.Core.Interfaces;
using static TNZAPI.NET.Api.Messaging.Common.Enums;

namespace TNZAPI.NET.Api.Addressbook.Contact.Dto
{
    [XmlType(TypeName = "root")]
    public class ContactApiResult : IApiResult
    {
        public ResultCode Result { get; set; } = ResultCode.Failed;

        public ContactModel Contact { get; set; }

        public List<string> ErrorMessage { get; set; }
    }
}
