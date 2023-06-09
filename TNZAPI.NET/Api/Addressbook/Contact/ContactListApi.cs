using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Addressbook;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Contact
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ContactListApi : IContactListApi
    {
        private ITNZAuth User = new TNZApiUser();

        public ContactListRequestOptions Options { get; set; } = new ContactListRequestOptions();

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        public ContactListApi()
        {
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public ContactListApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public ContactListApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public ContactListApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="auth">IAuth</param>
        public ContactListApi(ITNZAuth auth)
        {
            User = auth;
        }

        private XmlDocument BuildXmlDocument()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("ResultRequest");
            xmlDoc.AppendChild(rootNode);

            if (User.AuthToken.Equals(""))
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Sender", User.Sender));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "APIKey", User.APIKey));
            }

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/list?recordsPerPage={Options.RecordsPerPage}&page={Options.Page}");

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private ContactListApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.GetXMLAsync<ContactListApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ContactListApiResult>(e.Message);
            }
        }

        // Get XML from TNZ REST API
        private async Task<ContactListApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.GetXMLAsync<ContactListApiResult>(
                        new GetHttpRequest
                        (
                            BuildAPIURL(),
                            User,
                            BuildXmlDocument()
                        ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ContactListApiResult>(e.Message);
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
        /// Poll Data
        /// </summary>
        /// <returns>ContactListResult</returns>
        private ContactListApiResult List()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactListApiResult>("AuthToken required for this module.");
            }
            return SendXML();
        }

        /// <summary>
        /// List contact using ContactListOptions
        /// </summary>
        /// <param name="options">ContactListOptions</param>
        /// <returns>ContactListResult</returns>
        public ContactListApiResult List(IListRequestOptions options)
        {
            Options = Mapper.Update(new ContactListRequestOptions(), options);

            return List();
        }

        /// <summary>
        /// List contact with optional records per page and/or page number
        /// </summary>
        /// <param name="recordsPerPage">Number of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>ContactListResult</returns>
        public ContactListApiResult List(int? recordsPerPage = null, int? page = null)
        {
            var options = new ContactListRequestOptions();

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


        #endregion Read

        #region ListAsync

        /// <summary>
        /// Read Data Async
        /// </summary>
        /// <returns>ContactListResult</returns>
        [ComVisible(false)]
        private async Task<ContactListApiResult> ListAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactListApiResult>("AuthToken required for this module.");
            }
            return await SendXMLAsync();
        }

        /// <summary>
        /// Contact List using ContactListOptions
        /// </summary>
        /// <param name="options">ContactListOptions</param>
        /// <returns>Task<ContactListResult></returns>
        [ComVisible(false)]
        public async Task<ContactListApiResult> ListAsync(IListRequestOptions options)
        {
            Options = Mapper.Update(new ContactListRequestOptions(), options);

            return await ListAsync();
        }

        /// <summary>
        /// List contact with optional records per page and/or page number (async)
        /// </summary>
        /// <param name="recordsPerPage">Number of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>Task<ContactListResult></returns>
        [ComVisible(false)]
        public async Task<ContactListApiResult> ListAsync(int? recordsPerPage = null, int? page = null)
        {
            var options = new ContactListRequestOptions();

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


        #endregion ReadAsync
    }
}
