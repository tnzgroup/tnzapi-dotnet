using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Contact.Group.Dto
{
    public class ContactGroupListRequestOptions : IListRequestOptions
    {
        public string ContactID { get; set; } = "";

        public int RecordsPerPage { get; set; } = 100;

        public int Page { get; set; } = 1;
    }
}
