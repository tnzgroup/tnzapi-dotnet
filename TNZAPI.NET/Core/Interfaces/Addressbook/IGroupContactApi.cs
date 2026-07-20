using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupContactApi
    {
        void SetAPIUser(ITNZAuth apiUser);
        void SetAuthToken(string authToken);

        GroupContactListApiResult List(GroupID groupID);
        Task<GroupContactListApiResult> ListAsync(GroupID groupID);

        ContactGroupRelationApiResult Add(GroupID groupID, ContactID contactID);
        Task<ContactGroupRelationApiResult> AddAsync(GroupID groupID, ContactID contactID);
        ContactGroupRelationApiResult Remove(GroupID groupID, ContactID contactID);
        Task<ContactGroupRelationApiResult> RemoveAsync(GroupID groupID, ContactID contactID);
        ContactGroupRelationApiResult Detail(GroupID groupID, ContactID contactID);
        Task<ContactGroupRelationApiResult> DetailAsync(GroupID groupID, ContactID contactID);
    }
}