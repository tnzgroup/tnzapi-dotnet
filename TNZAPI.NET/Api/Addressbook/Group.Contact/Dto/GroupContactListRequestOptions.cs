using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Group.Contact.Dto
{
    public class GroupContactListRequestOptions : IListRequestOptions
    {
        public string GroupCode { get; set; } = "";

        public int RecordsPerPage { get; set; } = 100;

        public int Page { get; set; } = 1;
    }
}
