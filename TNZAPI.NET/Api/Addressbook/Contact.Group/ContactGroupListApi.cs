﻿using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Addressbook;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Contact.Group
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ContactGroupListApi : IContactGroupListApi
    {
        private ITNZAuth User = new TNZApiUser();

        private ContactGroupListRequestOptions Options { get; set; } = new ContactGroupListRequestOptions();

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        public ContactGroupListApi()
        {
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public ContactGroupListApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public ContactGroupListApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public ContactGroupListApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="auth">IAuth</param>
        public ContactGroupListApi(ITNZAuth auth)
        {
            User = auth;
        }

        /// <summary>
        /// Addressbook Contact List
        /// </summary>
        /// <param name="auth">ITNZAuth</param>
        /// <param name="contact">ContactModel</param>
        public ContactGroupListApi(ITNZAuth auth, ContactModel contact)
        {
            User = auth;

            Options.ContactID = contact.ID;
        }

        private XmlDocument BuildXmlDocument()
        {
            return new XmlDocument();
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/{Options.ContactID}/group/list?recordsPerPage={Options.RecordsPerPage}&page={Options.Page}");

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private ContactGroupListApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.GetXMLAsync<ContactGroupListApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ContactGroupListApiResult>(e.Message);
            }
        }

        // Get XML from TNZ REST API
        private async Task<ContactGroupListApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.GetXMLAsync<ContactGroupListApiResult>(
                        new GetHttpRequest
                        (
                            BuildAPIURL(),
                            User,
                            BuildXmlDocument()
                        ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ContactGroupListApiResult>(e.Message);
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
        /// <returns>ContactGroupListResult</returns>
        private ContactGroupListApiResult List()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactGroupListApiResult>("AuthToken required for this module.");
            }
            if (Options.ContactID is null || Options.ContactID == "")
            {
                return ResultHelper.RespondError<ContactGroupListApiResult>("Empty Contact ID - Please specify Options.ContactID.");
            }
            return SendXML();
        }

        /// <summary>
        /// List contact groups
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <param name="options">ContactGroupListOptions</param>
        /// <returns>ContactGroupListResult</returns>
        public ContactGroupListApiResult List(ContactModel entity, ContactGroupListRequestOptions options) => ListById(entity.ID, options);

        /// <summary>
        /// List contact groups with records per page with page number
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>ContactGroupListResult</returns>
        public ContactGroupListApiResult List(ContactModel entity, int? recordsPerPage = null, int? page = null) => ListById(entity.ID, recordsPerPage, page);

        #endregion

        #region ListById

        /// <summary>
        /// List contact groups
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <param name="options">ContactGroupListOptions</param>
        /// <returns>ContactGroupListResult</returns>
        public ContactGroupListApiResult ListById(string contactId, ContactGroupListRequestOptions options)
        {
            options.ContactID = contactId;

            Options = Mapper.Update(new ContactGroupListRequestOptions(), options);

            return List();
        }

        /// <summary>
        /// List contact groups with records per page with page number
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>ContactGroupListResult</returns>
        public ContactGroupListApiResult ListById(string contactId, int? recordsPerPage = null, int? page = null)
        {
            var options = new ContactGroupListRequestOptions()
            {
                ContactID = contactId
            };

            if (recordsPerPage is not null)
            {
                options.RecordsPerPage = (int)recordsPerPage;
            }

            if (page is not null)
            {
                options.Page = (int)page;
            }

            return ListById(contactId, options);
        }

        #endregion

        #region ListAsync

        /// <summary>
        /// List contact groups Async
        /// </summary>
        /// <returns>ContactGroupListResult</returns>
        [ComVisible(false)]
        private async Task<ContactGroupListApiResult> ListAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactGroupListApiResult>("AuthToken required for this module.");
            }
            if (Options.ContactID is null || Options.ContactID == "")
            {
                return ResultHelper.RespondError<ContactGroupListApiResult>("Empty Contact ID - Please specify contact id.");
            }
            return await SendXMLAsync();
        }

        /// <summary>
        /// List contact groups
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <param name="options">ContactGroupListOptions</param>
        /// <returns>Task<ContactGroupListResult></returns>
        public async Task<ContactGroupListApiResult> ListAsync(ContactModel entity, ContactGroupListRequestOptions options) => await ListByIdAsync(entity.ID, options);

        /// <summary>
        /// List contact groups using records per page and page number (async)
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>ContactGroupListResult</returns>
        [ComVisible(false)]
        public async Task<ContactGroupListApiResult> ListAsync(ContactModel entity, int? recordsPerPage = null, int? page = null) => await ListByIdAsync(entity.ID, recordsPerPage, page);

        #endregion ReadAsync

        #region ListByAsync

        /// <summary>
        /// List contact groups
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <param name="options">ContactGroupListOptions</param>
        /// <returns>Task<ContactGroupListResult></returns>
        public async Task<ContactGroupListApiResult> ListByIdAsync(string contactID, ContactGroupListRequestOptions options)
        {
            options.ContactID = contactID;

            Options = Mapper.Update(new ContactGroupListRequestOptions(), options);

            return await ListAsync();
        }

        /// <summary>
        /// List contact groups using records per page and page number (async)
        /// </summary>
        /// <param name="recordsPerPage">No of records per page</param>
        /// <param name="page">Page number</param>
        /// <returns>ContactGroupListResult</returns>
        [ComVisible(false)]
        public async Task<ContactGroupListApiResult> ListByIdAsync(string contactID, int? recordsPerPage = null, int? page = null)
        {
            var options = new ContactGroupListRequestOptions()
            {
                ContactID = contactID
            };

            if (recordsPerPage is not null)
            {
                options.RecordsPerPage = (int)recordsPerPage;
            }
            if (page is not null)
            {
                options.Page = (int)page;
            }

            return await ListByIdAsync(contactID, options);
        }

        #endregion ReadAsync
    }
}
