using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactGroupListApi : IApiClientBase
    {
        ContactGroupListApiResult List(ContactModel entity, int? recordsPerPage = null, int? page = null);
        ContactGroupListApiResult List(ContactModel entity, IListRequestOptions options);

        ContactGroupListApiResult ListById(string contactId, int? recordsPerPage = null, int? page = null);
        ContactGroupListApiResult ListById(string contactId, IListRequestOptions options);

        Task<ContactGroupListApiResult> ListAsync(ContactModel entity, int? recordsPerPage = null, int? page = null);
        Task<ContactGroupListApiResult> ListAsync(ContactModel entity, IListRequestOptions options);

        Task<ContactGroupListApiResult> ListByIdAsync(string contactId, int? recordsPerPage = null, int? page = null);
        Task<ContactGroupListApiResult> ListByIdAsync(string contactId, IListRequestOptions options);
    }
}