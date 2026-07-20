using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Contact.Group.Dto
{
    public class ContactGroupListApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();

        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }

        public ContactDetail? Contact { get; set; }
        public List<GroupDetail> Groups { get; set; } = new List<GroupDetail>();
    }
}