using System.Xml.Serialization;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Contact.Dto
{
    [XmlType(TypeName = "root")]
    public class ContactListApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }

        public int TotalRecords { get; set; } = 0;

        public int RecordsPerPage { get; set; } = 100;

        public int PageCount { get; set; } = 0;

        public int Page { get; set; } = 1;

        public List<ContactModel> Contacts { get; set; } = new List<ContactModel>();

        public List<string> ErrorMessage { get; set; } = new List<string>();
    }
}
