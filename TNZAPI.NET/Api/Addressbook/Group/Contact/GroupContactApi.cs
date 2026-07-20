using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Addressbook;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Group.Contact
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class GroupContactApi : IGroupContactApi
    {
        private ITNZAuth User = new TNZApiUser();

        public GroupContactApi()
        {
        }

        public GroupContactApi(ITNZAuth auth)
        {
            User = auth;
        }

        #region Set API User
        public void SetAPIUser(ITNZAuth apiUser)
        {
            User = apiUser;
        }

        public void SetAuthToken(string authToken)
        {
            User.AuthToken = authToken;
        }
        #endregion

        private static string BuildListURL(GroupID groupID)
        {
            IDGuard.EnsureProvided(groupID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group/{Uri.EscapeDataString(groupID.Value!)}/contact/list";
        }

        #region List
        public GroupContactListApiResult List(GroupID groupID)
        {
            return Task.Run(() => ListAsync(groupID)).Result;
        }

        [ComVisible(false)]
        public async Task<GroupContactListApiResult> ListAsync(GroupID groupID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<GroupContactListApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<GroupContactListApiResult>("Empty GroupID: Please specify GroupID");
            }

            return await HttpRequest.GetAsync<GroupContactListApiResult>(BuildListURL(groupID), User);
        }
        #endregion

        private static string BuildRelationURL(GroupID groupID)
        {
            IDGuard.EnsureProvided(groupID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group/{Uri.EscapeDataString(groupID.Value!)}/contact";
        }

        private static string BuildContactURL(GroupID groupID, ContactID contactID)
        {
            IDGuard.EnsureProvided(groupID);
            IDGuard.EnsureProvided(contactID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group/{Uri.EscapeDataString(groupID.Value!)}/contact/{Uri.EscapeDataString(contactID.Value!)}";
        }

        #region Add
        public ContactGroupRelationApiResult Add(GroupID groupID, ContactID contactID)
        {
            return Task.Run(() => AddAsync(groupID, contactID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactGroupRelationApiResult> AddAsync(GroupID groupID, ContactID contactID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty GroupID: Please specify GroupID");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty ContactID: Please specify ContactID");
            }

            return await HttpRequest.PatchAsync<ContactGroupRelationApiResult>(BuildRelationURL(groupID), User, new { ContactID = contactID });
        }
        #endregion

        #region Remove
        public ContactGroupRelationApiResult Remove(GroupID groupID, ContactID contactID)
        {
            return Task.Run(() => RemoveAsync(groupID, contactID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactGroupRelationApiResult> RemoveAsync(GroupID groupID, ContactID contactID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty GroupID: Please specify GroupID");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty ContactID: Please specify ContactID");
            }

            return await HttpRequest.DeleteAsync<ContactGroupRelationApiResult>(BuildContactURL(groupID, contactID), User);
        }
        #endregion

        #region Detail
        public ContactGroupRelationApiResult Detail(GroupID groupID, ContactID contactID)
        {
            return Task.Run(() => DetailAsync(groupID, contactID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactGroupRelationApiResult> DetailAsync(GroupID groupID, ContactID contactID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty GroupID: Please specify GroupID");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty ContactID: Please specify ContactID");
            }

            return await HttpRequest.GetAsync<ContactGroupRelationApiResult>(BuildContactURL(groupID, contactID), User);
        }
        #endregion
    }
}