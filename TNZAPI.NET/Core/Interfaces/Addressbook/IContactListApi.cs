using TNZAPI.NET.Api.Addressbook.Contact.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactListApi : IApiClientBase
    {
        ContactListRequestOptions Options { get; set; }

        ContactListApiResult List(IListRequestOptions options);
        ContactListApiResult List(int? recordsPerPage = null, int? page = null);

        Task<ContactListApiResult> ListAsync(IListRequestOptions options);
        Task<ContactListApiResult> ListAsync(int? recordsPerPage, int? page);
    }
}