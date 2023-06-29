using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Api.Addressbook.Group.Contact.Dto
{
    internal class GroupContactListRequestOptions
    {
        public GroupModel Group { get; set; }

        public int RecordsPerPage { get; set; } = 100;

        public int Page { get; set; } = 1;
    }
}
