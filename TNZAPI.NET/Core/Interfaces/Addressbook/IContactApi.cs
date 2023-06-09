using TNZAPI.NET.Api.Addressbook.Contact.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactApi : IApiClientBase
    {
        ContactApiResult Create(ContactModel entity);
        Task<ContactApiResult> CreateAsync(ContactModel entity);
        ContactApiResult Delete(ContactModel entity);
        ContactApiResult DeleteById(string contactID);
        Task<ContactApiResult> DeleteAsync(ContactModel entity);
        Task<ContactApiResult> DeleteByIdAsync(string contactID);
        ContactApiResult Get(ContactModel entity);
        ContactApiResult GetById(string contactID);
        Task<ContactApiResult> GetAsync(ContactModel entity);
        Task<ContactApiResult> GetByIdAsync(string contactID);
        ContactApiResult Read(ContactModel entity);
        ContactApiResult ReadById(string contactId);
        Task<ContactApiResult> ReadAsync(ContactModel entity);
        Task<ContactApiResult> ReadByIdAsync(string contactID);
        ContactApiResult Update(ContactModel entity);
        Task<ContactApiResult> UpdateAsync(ContactModel entity);

        ContactListApiResult List();
        ContactListApiResult List(IListRequestOptions options);
        Task<ContactListApiResult> ListAsync();
        Task<ContactListApiResult> ListAsync(IListRequestOptions options);

    }
}