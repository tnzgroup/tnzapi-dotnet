using TNZAPI.NET.Api.Addressbook.Contact.Dto;

namespace TNZAPI.NET.Api.Addressbook.Contact.Group.Dto
{
    internal class ContactGroupListRequestOptions
    {
        internal ContactModel Contact { get; set; }

        internal int RecordsPerPage { get; set; } = 100;

        internal int Page { get; set; } = 1;
    }
}
