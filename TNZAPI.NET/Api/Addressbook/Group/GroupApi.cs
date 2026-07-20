using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Addressbook.Group.Contact;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Addressbook;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Group
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class GroupApi : IGroupApi
    {
        private ITNZAuth User = new TNZApiUser();

        public IGroupContactApi Contact { get; set; }

        public GroupApi()
        {
            Contact = new GroupContactApi(User);
        }

        public GroupApi(string authToken)
        {
            User.AuthToken = authToken;
            Contact = new GroupContactApi(User);
        }

        public GroupApi(TNZApiUser apiUser)
        {
            User = apiUser;
            Contact = new GroupContactApi(User);
        }

        public GroupApi(ITNZAuth auth)
        {
            User = auth;
            Contact = new GroupContactApi(User);
        }

        #region Set API User
        public void SetAPIUser(ITNZAuth apiUser)
        {
            User = apiUser;
            Contact.SetAPIUser(apiUser);
        }

        public void SetAuthToken(string authToken)
        {
            User.AuthToken = authToken;
        }
        #endregion

        private static string BuildAPIURL()
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group";
        }

        private static string BuildGroupURL(GroupID groupID)
        {
            IDGuard.EnsureProvided(groupID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group/{Uri.EscapeDataString(groupID.Value!)}";
        }

        private static string BuildListURL(int page, int recordsPerPage)
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group/list?page={page}&recordsPerPage={recordsPerPage}";
        }

        #region Create
        public GroupApiResult Create(GroupModel entity)
        {
            return Task.Run(() => CreateAsync(entity)).Result;
        }

        [ComVisible(false)]
        public async Task<GroupApiResult> CreateAsync(GroupModel entity)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty AuthToken: Please specify AuthToken");
            }

            return await HttpRequest.PostAsync<GroupApiResult>(BuildAPIURL(), User, entity);
        }
        #endregion

        #region List
        public GroupListApiResult List(int page = 1, int recordsPerPage = 100)
        {
            return Task.Run(() => ListAsync(page, recordsPerPage)).Result;
        }

        [ComVisible(false)]
        public async Task<GroupListApiResult> ListAsync(int page = 1, int recordsPerPage = 100)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<GroupListApiResult>("Empty AuthToken: Please specify AuthToken");
            }

            return await HttpRequest.GetAsync<GroupListApiResult>(BuildListURL(page, recordsPerPage), User);
        }
        #endregion

        #region Details
        public GroupApiResult Details(GroupID groupID)
        {
            return Task.Run(() => DetailsAsync(groupID)).Result;
        }

        [ComVisible(false)]
        public async Task<GroupApiResult> DetailsAsync(GroupID groupID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupID: Please specify GroupID");
            }

            return await HttpRequest.GetAsync<GroupApiResult>(BuildGroupURL(groupID), User);
        }
        #endregion

        #region Update
        public GroupApiResult Update(GroupID groupID, GroupModel entity)
        {
            return Task.Run(() => UpdateAsync(groupID, entity)).Result;
        }

        [ComVisible(false)]
        public async Task<GroupApiResult> UpdateAsync(GroupID groupID, GroupModel entity)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupID: Please specify GroupID");
            }

            return await HttpRequest.PatchAsync<GroupApiResult>(BuildGroupURL(groupID), User, entity);
        }
        #endregion

        #region Delete
        public GroupApiResult Delete(GroupID groupID)
        {
            return Task.Run(() => DeleteAsync(groupID)).Result;
        }

        [ComVisible(false)]
        public async Task<GroupApiResult> DeleteAsync(GroupID groupID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (groupID is null || string.IsNullOrEmpty(groupID.Value))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupID: Please specify GroupID");
            }

            return await HttpRequest.DeleteAsync<GroupApiResult>(BuildGroupURL(groupID), User);
        }
        #endregion
    }
}