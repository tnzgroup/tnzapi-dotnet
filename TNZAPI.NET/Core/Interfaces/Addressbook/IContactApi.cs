using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactApi
    {
        ContactApiResult Create(ContactModel entity);
        Task<ContactApiResult> CreateAsync(ContactModel entity);

        ContactApiResult Details(ContactID contactID);
        Task<ContactApiResult> DetailsAsync(ContactID contactID);

        ContactApiResult Update(ContactID contactID, ContactModel entity);
        Task<ContactApiResult> UpdateAsync(ContactID contactID, ContactModel entity);

        ContactApiResult Delete(ContactID contactID);
        Task<ContactApiResult> DeleteAsync(ContactID contactID);

        ContactListApiResult Search(string? emailAddress = null, string? mobilePhone = null, string? mainPhone = null, string? attention = null, string? firstName = null, string? lastName = null, string? company = null, int page = 1, int recordsPerPage = 100);
        Task<ContactListApiResult> SearchAsync(string? emailAddress = null, string? mobilePhone = null, string? mainPhone = null, string? attention = null, string? firstName = null, string? lastName = null, string? company = null, int page = 1, int recordsPerPage = 100);

        ContactListApiResult List(int page = 1, int recordsPerPage = 100);
        Task<ContactListApiResult> ListAsync(int page = 1, int recordsPerPage = 100);

        IContactGroupApi Group { get; set; }
    }
}