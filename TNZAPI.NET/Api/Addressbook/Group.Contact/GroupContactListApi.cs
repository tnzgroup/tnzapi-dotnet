using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Addressbook;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Group.Contact
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class GroupContactListApi : IGroupContactListApi
    {
        private ITNZAuth User = new TNZApiUser();

        public GroupContactListRequestOptions Options = new GroupContactListRequestOptions();

        /// <summary>
        /// Addressbook group contact list
        /// </summary>
        public GroupContactListApi()
        {
        }

        /// <summary>
        /// Addressbook group contact list
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public GroupContactListApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Addressbook group contact list
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public GroupContactListApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Addressbook group contact list
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public GroupContactListApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Addressbook group contact list
        /// </summary>
        /// <param name="auth">IAuth</param>
        public GroupContactListApi(ITNZAuth auth)
        {
            User = auth;
        }

        private XmlDocument BuildXmlDocument()
        {
            return new XmlDocument();
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group/{Options.GroupCode}/contact/list?recordsPerPage={Options.RecordsPerPage}&page={Options.Page}");

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private GroupContactListApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.GetXMLAsync<GroupContactListApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<GroupContactListApiResult>(e.Message);
            }
        }

        // Get XML from TNZ REST API
        private async Task<GroupContactListApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.GetXMLAsync<GroupContactListApiResult>(
                        new GetHttpRequest
                        (
                            BuildAPIURL(),
                            User,
                            BuildXmlDocument()
                        ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<GroupContactListApiResult>(e.Message);
            }
        }

        #region Set API User
        /// <summary>
        /// Sets the API User
        /// </summary>
        /// <param name="apiUser">IAuth</param>
        public void SetAPIUser(ITNZAuth apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Sets the Authorization Token
        /// </summary>
        /// <param name="authToken">Authorization Token</param>
        public void SetAuthToken(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Sets the API Sender
        /// </summary>
        /// <param name="apiSender">Sender</param>
        public void SetAPISender(string apiSender)
        {
            User.Sender = apiSender;
        }

        /// <summary>
        /// Sets the API Key
        /// </summary>
        /// <param name="apiKey">API Key</param>
        public void SetAPIKey(string apiKey)
        {
            User.APIKey = apiKey;
        }
        #endregion

        #region List

        /// <summary>
        /// List group contacts
        /// </summary>
        /// <returns>GroupContactListResult</returns>
        private GroupContactListApiResult List()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupContactListApiResult>("AuthToken required for this module.");
            }
            if (Options.GroupCode == null || Options.GroupCode == "")
            {
                return ResultHelper.RespondError<GroupContactListApiResult>("Missing GroupCode. Please specify Options.GroupCode.");
            }
            return SendXML();
        }

        /// <summary>
        /// List group contacts
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>GroupContactListResult</returns>
        public GroupContactListApiResult List(GroupModel entity, int? recordsPerPage = null, int? page = null) => ListByGroupCode(entity.GroupCode, recordsPerPage, page);

        /// <summary>
        /// List group contacts
        /// </summary>
        /// <param name="options">GroupContactListOptions</param>
        /// <returns>GroupContactListResult</returns>
        public GroupContactListApiResult List(GroupModel entity, GroupContactListRequestOptions options) => ListByGroupCode(entity.GroupCode, options);

        #endregion Read

        #region ListByGroupCode

        /// <summary>
        /// List group contacts
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>GroupContactListResult</returns>
        public GroupContactListApiResult ListByGroupCode(string groupCode, int? recordsPerPage = null, int? page = null)
        {
            var options = new GroupContactListRequestOptions()
            {
                GroupCode = groupCode
            };

            if (recordsPerPage is not null)
            {
                options.RecordsPerPage = (int)recordsPerPage;
            }
            if (page is not null)
            {
                options.Page = (int)page;
            }

            return ListByGroupCode(groupCode, options);
        }

        /// <summary>
        /// List group contacts
        /// </summary>
        /// <param name="options">GroupContactListOptions</param>
        /// <returns>GroupContactListResult</returns>
        public GroupContactListApiResult ListByGroupCode(string groupCode, GroupContactListRequestOptions options)
        {
            options.GroupCode = groupCode;

            Options = Mapper.Update(new GroupContactListRequestOptions(), options);

            return List();
        }

        #endregion

        #region ListAsync

        /// <summary>
        /// List group contacts (async)
        /// </summary>
        /// <returns>Task<GroupContactListResult></returns>
        [ComVisible(false)]
        private async Task<GroupContactListApiResult> ListAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupContactListApiResult>("AuthToken required for this module.");
            }
            if (Options.GroupCode == null || Options.GroupCode == "")
            {
                return ResultHelper.RespondError<GroupContactListApiResult>("Missing GroupCode. Please specify Options.GroupCode.");
            }

            return await SendXMLAsync();
        }

        /// <summary>
        /// List group contacts (async)
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>Task<GroupContactListResult></returns>
        public async Task<GroupContactListApiResult> ListAsync(GroupModel entity, int? recordsPerPage = null, int? page = null) => await ListByGroupCodeAsync(entity.GroupCode, recordsPerPage, page);

        /// <summary>
        /// List group contacts (async)
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Task<GroupContactListResult></returns>
        [ComVisible(false)]
        public async Task<GroupContactListApiResult> ListAsync(GroupModel entity, GroupContactListRequestOptions options) => await ListByGroupCodeAsync(entity.GroupCode, options);

        #endregion ReadAsync

        #region ListByGroupCodeAsync

        /// <summary>
        /// List group contacts (async)
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>Task<GroupContactListResult></returns>
        public async Task<GroupContactListApiResult> ListByGroupCodeAsync(string groupCode, int? recordsPerPage = null, int? page = null)
        {
            var options = new GroupContactListRequestOptions()
            {
                GroupCode = groupCode
            };

            if (recordsPerPage is not null)
            {
                options.RecordsPerPage = (int)recordsPerPage;
            }
            if (page is not null)
            {
                options.Page = (int)page;
            }

            return await ListByGroupCodeAsync(groupCode, options);
        }

        /// <summary>
        /// List group contacts (async)
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Task<GroupContactListResult></returns>
        [ComVisible(false)]
        public async Task<GroupContactListApiResult> ListByGroupCodeAsync(string groupCode, GroupContactListRequestOptions options)
        {
            options.GroupCode = groupCode;

            Options = Mapper.Update(new GroupContactListRequestOptions(), options);

            return await ListAsync();
        }
        #endregion ReadAsync
    }
}
