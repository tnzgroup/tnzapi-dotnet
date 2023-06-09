using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Contact.Dto
{
    public class ContactListRequestOptions : IListRequestOptions
    {
        public int RecordsPerPage { get; set; } = 100;

        public int Page { get; set; } = 1;
    }
}
