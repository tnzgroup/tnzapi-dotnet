using System.Xml.Serialization;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Contact.Group.Dto
{
    [XmlType(TypeName = "root")]
    public class ContactGroupListApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }

        public int TotalRecords { get; set; } = 0;

        public int RecordsPerPage { get; set; } = 100;

        public int PageCount { get; set; } = 0;

        public int Page { get; set; } = 1;

        public ContactModel Contact { get; set; }

        public List<GroupModel> Groups { get; set; }

        public List<string> ErrorMessage { get; set; }
    }
}
