using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
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

        private enum Action { Add, Remove, Read };

        private ContactModel Contact { get; set; } = new ContactModel();

        private GroupModel Group { get; set; } = new GroupModel();

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        public ContactGroupApi()
        {
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public ContactGroupApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public ContactGroupApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public ContactGroupApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="auth">ITNZAuth</param>
        public ContactGroupApi(ITNZAuth auth)
        {
            User = auth;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="auth">ITNZAuth</param>
        /// <param name="contact">ContactModel</param>
        public ContactGroupApi(ITNZAuth auth, ContactModel contact)
        {
            User = auth;
            Contact = contact;
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
                case Action.Remove:
                    return xmlDoc;
            }

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("AddGroupRequest");
            xmlDoc.AppendChild(rootNode);

            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "GroupCode", Group.GroupCode));

            return xmlDoc;
        }

        private string BuildAPIURL(Action action)
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/{Contact.ID}/group");

            switch (action)
            {
                case Action.Read:
                case Action.Remove:
                    requestUri.Append($"/{Group.GroupCode}");
                    break;
                default:
                    break;
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private ContactGroupApiResult SendXML(Action action)
        {
            try
            {
                if (action == Action.Add)
                {
                    return Task.Run(() => HttpRequest.PostXMLAsync<ContactGroupApiResult>(
                        new PostHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }
                if (action == Action.Remove)
                {
                    return Task.Run(() => HttpRequest.DeleteXMLAsync<ContactGroupApiResult>(
                        new DeleteHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }

                return Task.Run(() => HttpRequest.GetXMLAsync<ContactGroupApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(action),
                        User,
                        BuildXmlDocument(action)
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ContactGroupApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<ContactGroupApiResult> SendXMLAsync(Action action)
        {
            try
            {
                if (action == Action.Add)
                {
                    return await HttpRequest.PostXMLAsync<ContactGroupApiResult>(
                        new PostHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }
                if (action == Action.Remove)
                {
                    return await HttpRequest.DeleteXMLAsync<ContactGroupApiResult>(
                        new DeleteHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }

                return await HttpRequest.GetXMLAsync<ContactGroupApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(action),
                        User,
                        BuildXmlDocument(action)
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ContactGroupApiResult>(e.Message);
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

        #region Add
        /// <summary>
        /// Add contact group
        /// </summary>
        /// <returns>ContactGroupResult</returns>
        private ContactGroupApiResult Add()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("AuthToken required for this module.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Contact: Please specify contact.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Group: Please specify group.");
            }
            return SendXML(Action.Add);
        }

        /// <summary>
        /// Add contact group
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>ContactGroupApiResult</returns>
        public ContactGroupApiResult Add(ContactModel contact, GroupModel group)
        {
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }

            return Add();
        }

        /// <summary>
        /// Add contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <returns>ContactGroupApiResult</returns>
        public ContactGroupApiResult Add(string contactID, string groupCode)
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            return Add();
        }

        /// <summary>
        /// Add contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>ContactGroupApiResult</returns>
        public ContactGroupApiResult Add(
            ContactModel contact = null,
            GroupModel group = null,
            string contactID = null,
            string groupCode = null
        )
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }

            return Add();
        }
        #endregion

        #region AddAsync
        /// <summary>
        /// Add contact group (async)
        /// </summary>
        /// <returns>Task<ContactGroupResult></returns>
        private async Task<ContactGroupApiResult> AddAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("AuthToken required for this module.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Contact: Please specify contact.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Group: Please specify group.");
            }
            return await SendXMLAsync(Action.Add);
        }

        /// <summary>
        /// Add Contact Group (async)
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> AddAsync(ContactModel contact, GroupModel group)
        {
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }

            return await AddAsync();
        }

        /// <summary>
        /// Add Contact Group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> AddAsync(string contactID, string groupCode)
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            return await AddAsync();
        }

        /// <summary>
        /// Add Contact Group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> AddAsync(
            ContactModel contact = null,
            GroupModel group = null,
            string contactID = null,
            string groupCode = null
        )
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }

            return await AddAsync();
        }
        #endregion

        #region Remove
        /// <summary>
        /// Remove contact group
        /// </summary>
        /// <returns>ContactGroupResult</returns>
        private ContactGroupApiResult Remove()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("AuthToken required for this module.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Contact: Please specify contact.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Group: Please specify group.");
            }
            return SendXML(Action.Remove);
        }

        /// <summary>
        /// Remove contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Remove(ContactModel contact, GroupModel group)
        {
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }

            return Remove();
        }

        /// <summary>
        /// Remove contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Remove(string contactID, string groupCode)
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            return Remove();
        }

        /// <summary>
        /// Remove contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Remove(
            ContactModel contact = null,
            GroupModel group = null,
            string contactID = null,
            string groupCode = null
        )
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }
            
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            
            return Remove();
        }
        #endregion

        #region RemoveAsync
        /// <summary>
        /// Remove contact group (async)
        /// </summary>
        /// <returns>Task<ContactGroupResult></returns>
        private async Task<ContactGroupApiResult> RemoveAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("AuthToken required for this module.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Contact: Please specify contact.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Group: Please specify group.");
            }
            return await SendXMLAsync(Action.Remove);
        }

        /// <summary>
        /// Remove contact group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> RemoveAsync(ContactModel contact, GroupModel group)
        {
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }

            return await RemoveAsync();
        }

        /// <summary>
        /// Remove contact group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> RemoveAsync(
            string contactID,
            string groupCode
        )
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            return await RemoveAsync();
        }

        /// <summary>
        /// Remove contact group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> RemoveAsync(
            ContactModel contact = null, 
            GroupModel group = null, 
            string contactID = null, 
            string groupCode = null
        )
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }
            
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            
            return await RemoveAsync();
        }

        #endregion

        #region Read
        /// <summary>
        /// Read contact group
        /// </summary>
        /// <returns>ContactGroupResult</returns>
        private ContactGroupApiResult Read()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("AuthToken required for this module.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Contact: Please specify contact.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Group: Please specify group.");
            }
            return SendXML(Action.Read);
        }

        /// <summary>
        /// Read contact group
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Read(ContactModel contact, GroupModel group)
        {
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }

            return Read();
        }

        /// <summary>
        /// Read contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Read(string contactID, string groupCode)
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            return Read();
        }

        /// <summary>
        /// Read contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Read(
            ContactModel contact = null, 
            GroupModel group = null, 
            string contactID = null, 
            string groupCode = null
        )
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            
            return Read();
        }

        /// <summary>
        /// Read contact group
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Get(ContactModel contact, GroupModel group) => Read(contact, group);

        /// <summary>
        /// Read contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Get(string contactID = null, string groupCode = null) => Read(contactID, groupCode);

        /// <summary>
        /// Read contact group
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>ContactGroupResult</returns>
        public ContactGroupApiResult Get(ContactModel contact = null, GroupModel group = null, string contactID = null, string groupCode = null) => Read(contact, group, contactID, groupCode);

        #endregion

        #region ReadAsync
        /// <summary>
        /// Read contactgroup  (async)
        /// </summary>
        /// <returns>Task<ContactGroupResult></returns>
        private async Task<ContactGroupApiResult> ReadAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("AuthToken required for this module.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Contact: Please specify contact.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<ContactGroupApiResult>("Empty Group: Please specify group.");
            }
            return await SendXMLAsync(Action.Read);
        }

        /// <summary>
        /// Read contact group (async)
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> ReadAsync(ContactModel contact, GroupModel group)
        {
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }

            return await ReadAsync();
        }

        /// <summary>
        /// Read contact group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> ReadAsync(string contactID, string groupCode )
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }

            return await ReadAsync();
        }

        /// <summary>
        /// Read contact group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> ReadAsync(
            ContactModel contact = null, 
            GroupModel group = null, 
            string contactID = null, 
            string groupCode = null
        )
        {
            if (contactID is not null)
            {
                Contact.ID = contactID;
            }
            if (groupCode is not null)
            {
                Group.GroupCode = groupCode;
            }
            
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            
            return await ReadAsync();
        }

        /// <summary>
        /// Read contact group (async)
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> GetAsync(ContactModel contact, GroupModel group) => await ReadAsync(contact, group);

        /// <summary>
        /// Read contact group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> GetAsync(string contactID, string groupCode) => await ReadAsync(contactID, groupCode);

        /// <summary>
        /// Read contact group (async)
        /// </summary>
        /// <param name="contactID">Contact ID</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>Task<ContactGroupResult></returns>
        public async Task<ContactGroupApiResult> GetAsync(ContactModel contact = null, GroupModel group = null, string contactID = null, string groupCode = null) => await ReadAsync(contact, group, contactID, groupCode);

        #endregion
    }
}
