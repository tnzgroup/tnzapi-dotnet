using System.Xml.Serialization;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Contact.Dto
{
    [XmlType(TypeName = "root")]
    public class ContactListApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }

        public int TotalRecords { get; set; }

        public int RecordsPerPage { get; set; }

        public int PageCount { get; set; }

        public int Page { get; set; }

        [XmlArray("Contacts")]
        public List<ContactModel> Contacts { get; set; }

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }
    }
}
