using System.Linq.Expressions;
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
    public class ContactApi : IContactApi
    {
        private ITNZAuth User = new TNZApiUser();

        private enum Action { Create, Update, Delete, Read };

        private ContactModel Entity { get; set; } = new ContactModel();

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        public ContactApi()
        {
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public ContactApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public ContactApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public ContactApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="auth">IAuth</param>
        public ContactApi(ITNZAuth auth)
        {
            User = auth;
        }

        private XmlDocument BuildXmlDocument(Action action)
        {
            #region XML Sample

            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <Contact>
                <ExType></ExType>
                <ExID>0</ExID>
                <Attention>Test 2 Lawry (TEST 2023-04-17)</Attention>
                <Title>Mrs</Title>
                <Company></Company>
                <RecipDepartment/>
                <FirstName>Elrose </FirstName>
                <LastName>Lawry</LastName>
                <Position></Position>
                <StreetAddress></StreetAddress>
                <Suburb></Suburb>
                <City></City>
                <State></State>
                <Country>NZ</Country>
                <Postcode></Postcode>
                <MainPhone></MainPhone>
                <AltPhone1/>
                <AltPhone2/>
                <DirectPhone></DirectPhone>
                <MobilePhone>0211144489</MobilePhone>
                <FaxNumber></FaxNumber>
                <EmailAddress></EmailAddress>
                <WebAddress></WebAddress>
                <Context/>
                <Custom1></Custom1>
                <Custom2></Custom2>
                <Custom3></Custom3>
                <Custom4></Custom4>
            </Contact>     
            */

            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            switch (action)
            {
                case Action.Read:
                case Action.Delete:
                    return xmlDoc;
            }

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("Contact");
            xmlDoc.AppendChild(rootNode);

            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Attention", Entity.Attention));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Company", Entity.Company));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "RecipDepartment", Entity.CompanyDepartment));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "FirstName", Entity.FirstName));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "LastName", Entity.LastName));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Position", Entity.Position));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "StreetAddress", Entity.StreetAddress));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Suburb", Entity.Suburb));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "City", Entity.City));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "State", Entity.State));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Country", Entity.Country));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Postcode", Entity.Postcode));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MainPhone", Entity.MainPhone));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "AltPhone1", Entity.AltPhone1));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "AltPhone2", Entity.AltPhone2));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "DirectPhone", Entity.DirectPhone));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MobilePhone", Entity.MobilePhone));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "FaxNumber", Entity.FaxNumber));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "EmailAddress", Entity.EmailAddress));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "WebAddress", Entity.WebAddress));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Custom1", Entity.Custom1));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Custom2", Entity.Custom2));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Custom3", Entity.Custom3));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Custom4", Entity.Custom4));

            return xmlDoc;
        }

        private string BuildAPIURL(Action action)
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact");

            switch (action)
            {
                case Action.Read:
                case Action.Update:
                case Action.Delete:
                    requestUri.Append($"/{Entity.ID}");
                    break;
                case Action.Create:
                    break;
                default:
                    break;
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private ContactApiResult SendXML(Action action)
        {
            try
            {
                if (action == Action.Create)
                {
                    return Task.Run(() => HttpRequest.PostXMLAsync<ContactApiResult>(
                        new PostHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }
                if (action == Action.Update)
                {
                    return Task.Run(() => HttpRequest.PatchXMLAsync<ContactApiResult>(
                        new PatchHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }
                if (action == Action.Delete)
                {
                    return Task.Run(() => HttpRequest.DeleteXMLAsync<ContactApiResult>(
                        new DeleteHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }

                return Task.Run(() => HttpRequest.GetXMLAsync<ContactApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(action),
                        User,
                        BuildXmlDocument(action)
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ContactApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<ContactApiResult> SendXMLAsync(Action action)
        {
            try
            {
                if (action == Action.Create)
                {
                    return await HttpRequest.PostXMLAsync<ContactApiResult>(
                        new PostHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }
                if (action == Action.Update)
                {
                    return await HttpRequest.PatchXMLAsync<ContactApiResult>(
                        new PatchHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }
                if (action == Action.Delete)
                {
                    return await HttpRequest.DeleteXMLAsync<ContactApiResult>(
                        new DeleteHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }

                return await HttpRequest.GetXMLAsync<ContactApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(action),
                        User,
                        BuildXmlDocument(action)
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ContactApiResult>(e.Message);
            }
        }

        public void SetContactProperty<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<ContactModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, ContactModel>(propertyExpression);
            PropertyHelper.SetProperty(Entity, convertedExpression, value);
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

        #region Create
        /// <summary>
        /// Create Contact
        /// </summary>
        /// <returns>ContactResult</returns>
        private ContactApiResult Create()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact: Please specify any value");
            }
            return SendXML(Action.Create);
        }

        /// <summary>
        /// Create Contact
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns>ContactResult</returns>
        public ContactApiResult Create(ContactModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return Create();
        }
        #endregion

        #region CreateAsync
        /// <summary>
        /// Create Contact (async)
        /// </summary>
        /// <returns>Task<ContactResult></returns>
        private async Task<ContactApiResult> CreateAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact: Please specify any value");
            }
            return await SendXMLAsync(Action.Create);
        }

        /// <summary>
        /// Create Contact (async)
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns>Task<ContactResult></returns>
        public async Task<ContactApiResult> CreateAsync(ContactModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return await CreateAsync();
        }
        #endregion

        #region Update
        /// <summary>
        /// Update contact
        /// </summary>
        /// <returns>ContactResult</returns>
        private ContactApiResult Update()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact: Please specify any value");
            }
            if (Entity.ID is null || Entity.ID == "")
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact.ID: Please specify contact ID");
            }
            return SendXML(Action.Update);
        }

        /// <summary>
        /// Update contact
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns>ContactResult</returns>
        public ContactApiResult Update(ContactModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return Update();
        }
        #endregion

        #region UpdateAsync
        /// <summary>
        /// Update contact (async)
        /// </summary>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        private async Task<ContactApiResult> UpdateAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact: Please specify any value");
            }
            if (Entity.ID is null || Entity.ID == "")
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact.ID: Please specify contact ID");
            }
            return await SendXMLAsync(Action.Update);
        }

        /// <summary>
        /// Update contact (async)
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        public async Task<ContactApiResult> UpdateAsync(ContactModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return await UpdateAsync();
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete contact
        /// </summary>
        /// <returns>ContactResult</returns>
        private ContactApiResult Delete()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact: Please specify any value");
            }
            if (Entity.ID is null || Entity.ID == "")
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact.ID: Please specify contact ID");
            }
            return SendXML(Action.Delete);
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns>ContactResult</returns>
        public ContactApiResult Delete(ContactModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return Delete();
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <returns></returns>
        public ContactApiResult DeleteById(string contactID)
        {
            Entity = new ContactModel() { ID = contactID };

            return Delete();
        }
        #endregion

        #region DeleteAsync
        /// <summary>
        /// Delete contact (async)
        /// </summary>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        private async Task<ContactApiResult> DeleteAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact: Please specify any value");
            }
            if (Entity.ID is null || Entity.ID == "")
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact.ID: Please specify contact ID");
            }
            return await SendXMLAsync(Action.Delete);
        }

        /// <summary>
        /// Delete contact (async)
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns></returns>
        [ComVisible(false)]
        public async Task<ContactApiResult> DeleteAsync(ContactModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return await DeleteAsync();
        }

        /// <summary>
        /// Delete contact (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        public async Task<ContactApiResult> DeleteByIdAsync(string contactID)
        {
            Entity = new ContactModel() { ID = contactID };

            return await DeleteAsync();
        }
        #endregion

        #region Read
        /// <summary>
        /// Read contact
        /// </summary>
        /// <returns>ContactResult</returns>
        private ContactApiResult Read()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact: Please specify any value");
            }
            if (Entity.ID is null || Entity.ID == "")
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact.ID: Please specify contact ID");
            }
            return SendXML(Action.Read);
        }

        /// <summary>
        /// Read contact
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns>ContactResult</returns>
        public ContactApiResult Read(ContactModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return Read();
        }

        /// <summary>
        /// Read contact
        /// </summary>
        /// <param name="contactId">Contact ID</param>
        /// <returns>ContactResult</returns>
        public ContactApiResult ReadById(string contactId)
        {
            Entity = new ContactModel() { ID = contactId };

            return Read();
        }

        /// <summary>
        /// Read contact
        /// </summary>
        /// <returns>ContactResult</returns>
        public ContactApiResult Get() => Read();

        /// <summary>
        /// Read contact
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns>ContactResult</returns>
        public ContactApiResult Get(ContactModel entity) => Read(entity);

        /// <summary>
        /// Read contact
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <returns>ContactResult</returns>
        public ContactApiResult GetById(string contactID) => ReadById(contactID);
        #endregion

        #region ReadAsync
        /// <summary>
        /// Read contact (async)
        /// </summary>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        private async Task<ContactApiResult> ReadAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact: Please specify any value");
            }
            if (Entity.ID is null || Entity.ID == "")
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty Contact.ID: Please specify contact ID");
            }
            return await SendXMLAsync(Action.Read);
        }

        /// <summary>
        /// Read contact (async)
        /// </summary>
        /// <param name="entity">ContactModel</param>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        public async Task<ContactApiResult> ReadAsync(ContactModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return await ReadAsync();
        }

        /// <summary>
        /// Read contact (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        public async Task<ContactApiResult> ReadByIdAsync(string contactID)
        {
            Entity = new ContactModel() { ID = contactID };

            return await ReadAsync();
        }

        /// <summary>
        /// Read contact (async)
        /// </summary>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        public async Task<ContactApiResult> GetAsync() => await ReadAsync();

        /// <summary>
        /// Read contact (async)
        /// </summary>
        /// <param name="contactModel">ContactModel</param>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        public async Task<ContactApiResult> GetAsync(ContactModel entity) => await ReadAsync(entity);

        /// <summary>
        /// Read contact (async)
        /// </summary>
        /// <param name="contactID">ContactModel</param>
        /// <returns>Task<ContactResult></returns>
        [ComVisible(false)]
        public async Task<ContactApiResult> GetByIdAsync(string contactID) => await ReadByIdAsync(contactID);
        #endregion

        #region List
        public ContactListApiResult List()
        {
            return new ContactListApi(User)
                .List();
        }

        public ContactListApiResult List(IListRequestOptions options)
        {
            return new ContactListApi(User)
                .List(options);
        }
        #endregion

        #region ListAsync
        public async Task<ContactListApiResult> ListAsync()
        {
            return await new ContactListApi(User)
                .ListAsync();
        }

        public async Task<ContactListApiResult> ListAsync(IListRequestOptions options)
        {
            return await new ContactListApi(User)
                .ListAsync(options);
        }
        #endregion
    }
}
