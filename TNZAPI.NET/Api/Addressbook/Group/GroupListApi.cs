using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Addressbook;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Group
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class GroupListApi : IGroupListApi
    {
        private ITNZAuth User = new TNZApiUser();

        private GroupListRequestOptions Options { get; set; } = new GroupListRequestOptions();

        /// <summary>
        /// Addressbook Group List
        /// </summary>
        public GroupListApi()
        {
        }

        /// <summary>
        /// Addressbook Group List
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public GroupListApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Addressbook Group List
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        public GroupListApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Addressbook Group List
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public GroupListApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Addressbook Group List
        /// </summary>
        /// <param name="auth">IAuth</param>
        public GroupListApi(ITNZAuth auth)
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

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group/list?recordsPerPage={Options.RecordsPerPage}&page={Options.Page}");

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private GroupListApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.GetXMLAsync<GroupListApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<GroupListApiResult>(e.Message);
            }
        }

        // Get XML from TNZ REST API
        private async Task<GroupListApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.GetXMLAsync<GroupListApiResult>(
                        new GetHttpRequest
                        (
                            BuildAPIURL(),
                            User,
                            BuildXmlDocument()
                        ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<GroupListApiResult>(e.Message);
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
        /// List groups
        /// </summary>
        /// <returns>GroupListResult</returns>
        private GroupListApiResult List()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupListApiResult>("AuthToken required for this module.");
            }
            return SendXML();
        }

        /// <summary>
        /// List groups
        /// </summary>
        /// <param name="options">GroupListOptions</param>
        /// <returns>GroupListResult</returns>
        public GroupListApiResult List(IListRequestOptions options)
        {
            Options = Mapper.Map(new GroupListRequestOptions(), options);

            return List();
        }

        /// <summary>
        /// List groups
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>GroupListResult</returns>
        public GroupListApiResult List(int? recordsPerPage = null, int? page = null)
        {
            var options = new GroupListRequestOptions();

            if (recordsPerPage is not null)
            {
                options.RecordsPerPage = (int)recordsPerPage;
            }
            if (page is not null)
            {
                options.Page = (int)page;
            }

            return List(options);
        }

        #endregion List

        #region ListAsync

        /// <summary>
        /// List groups (async)
        /// </summary>
        /// <returns>Task<GroupListResult></returns>
        [ComVisible(false)]
        private async Task<GroupListApiResult> ListAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupListApiResult>("AuthToken required for this module.");
            }
            return await SendXMLAsync();
        }

        /// <summary>
        /// List groups (async)
        /// </summary>
        /// <param name="options">GroupListOptions</param>
        /// <returns>Task<GroupListResult></returns>
        [ComVisible(false)]
        public async Task<GroupListApiResult> ListAsync(IListRequestOptions options)
        {
            Options = Mapper.Update(new GroupListRequestOptions(), options);

            return await ListAsync();
        }

        /// <summary>
        /// List groups (async)
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>Task<GroupListResult></returns>
        [ComVisible(false)]
        public async Task<GroupListApiResult> ListAsync(int? recordsPerPage = null, int? page = null)
        {
            var options = new GroupListRequestOptions();

            if (recordsPerPage is not null)
            {
                options.RecordsPerPage = (int)recordsPerPage;
            }
            if (page is not null)
            {
                options.Page = (int)page;
            }

            return await ListAsync(options);
        }

        #endregion ListAsync
    }
}
