using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactGroupApi
    {
        void SetAPIUser(ITNZAuth apiUser);
        void SetAuthToken(string authToken);

        ContactGroupListApiResult List(ContactID contactID);
        Task<ContactGroupListApiResult> ListAsync(ContactID contactID);

        ContactGroupRelationApiResult Add(ContactID contactID, GroupID groupID);
        Task<ContactGroupRelationApiResult> AddAsync(ContactID contactID, GroupID groupID);

        ContactGroupRelationApiResult Remove(ContactID contactID, GroupID groupID);
        Task<ContactGroupRelationApiResult> RemoveAsync(ContactID contactID, GroupID groupID);

        ContactGroupRelationApiResult Detail(ContactID contactID, GroupID groupID);
        Task<ContactGroupRelationApiResult> DetailAsync(ContactID contactID, GroupID groupID);
    }
}