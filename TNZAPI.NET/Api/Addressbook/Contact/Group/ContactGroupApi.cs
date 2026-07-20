using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Addressbook;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Contact.Group
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ContactGroupApi : IContactGroupApi
    {
        private ITNZAuth User = new TNZApiUser();

        public ContactGroupApi()
        {
        }

        public ContactGroupApi(ITNZAuth auth)
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

        private static string BuildListURL(ContactID contactID)
        {
            IDGuard.EnsureProvided(contactID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/{Uri.EscapeDataString(contactID.Value!)}/group/list";
        }

        private static string BuildRelationURL(ContactID contactID)
        {
            IDGuard.EnsureProvided(contactID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/{Uri.EscapeDataString(contactID.Value!)}/group";
        }

        private static string BuildGroupURL(ContactID contactID, GroupID groupID)
        {
            IDGuard.EnsureProvided(contactID);
            IDGuard.EnsureProvided(groupID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/{Uri.EscapeDataString(contactID.Value!)}/group/{Uri.EscapeDataString(groupID.Value!)}";
        }

        #region List
        public ContactGroupListApiResult List(ContactID contactID)
        {
            return Task.Run(() => ListAsync(contactID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactGroupListApiResult> ListAsync(ContactID contactID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactGroupListApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactGroupListApiResult>("Empty ContactID: Please specify ContactID");
            }

            return await HttpRequest.GetAsync<ContactGroupListApiResult>(BuildListURL(contactID), User);
        }
        #endregion

        #region Add
        public ContactGroupRelationApiResult Add(ContactID contactID, GroupID groupID)
        {
            return Task.Run(() => AddAsync(contactID, groupID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactGroupRelationApiResult> AddAsync(ContactID contactID, GroupID groupID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty ContactID: Please specify ContactID");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty GroupID: Please specify GroupID");
            }

            return await HttpRequest.PatchAsync<ContactGroupRelationApiResult>(BuildRelationURL(contactID), User, new { GroupID = groupID });
        }
        #endregion

        #region Remove
        public ContactGroupRelationApiResult Remove(ContactID contactID, GroupID groupID)
        {
            return Task.Run(() => RemoveAsync(contactID, groupID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactGroupRelationApiResult> RemoveAsync(ContactID contactID, GroupID groupID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty ContactID: Please specify ContactID");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty GroupID: Please specify GroupID");
            }

            return await HttpRequest.DeleteAsync<ContactGroupRelationApiResult>(BuildGroupURL(contactID, groupID), User);
        }
        #endregion

        #region Detail
        public ContactGroupRelationApiResult Detail(ContactID contactID, GroupID groupID)
        {
            return Task.Run(() => DetailAsync(contactID, groupID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactGroupRelationApiResult> DetailAsync(ContactID contactID, GroupID groupID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty ContactID: Please specify ContactID");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<ContactGroupRelationApiResult>("Empty GroupID: Please specify GroupID");
            }

            return await HttpRequest.GetAsync<ContactGroupRelationApiResult>(BuildGroupURL(contactID, groupID), User);
        }
        #endregion
    }
}