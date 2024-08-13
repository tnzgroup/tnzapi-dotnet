using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
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

        private enum Action { Add, Remove, Read };

        private GroupModel Group { get; set; } = new GroupModel();

        private ContactModel Contact { get; set; } = new ContactModel();

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        public GroupContactApi()
        {
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public GroupContactApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public GroupContactApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public GroupContactApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Addressbook Contact
        /// </summary>
        /// <param name="auth">IAuth</param>
        public GroupContactApi(ITNZAuth auth)
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
                case Action.Remove:
                    return xmlDoc;
            }

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("AddContactRequest");
            xmlDoc.AppendChild(rootNode);

            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ContactID", Contact.ContactID));

            return xmlDoc;
        }

        private string BuildAPIURL(Action action)
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group/{Group.GroupID ?? Group.GroupCode}/contact");

            switch (action)
            {
                case Action.Read:
                case Action.Remove:
                    requestUri.Append($"/{Contact.ContactID}");
                    break;
                default:
                    break;
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private GroupContactApiResult SendXML(Action action)
        {
            try
            {
                if (action == Action.Add)
                {
                    return Task.Run(() => HttpRequest.PostXMLAsync<GroupContactApiResult>(
                        new PostHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }
                if (action == Action.Remove)
                {
                    return Task.Run(() => HttpRequest.DeleteXMLAsync<GroupContactApiResult>(
                        new DeleteHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }

                return Task.Run(() => HttpRequest.GetXMLAsync<GroupContactApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(action),
                        User,
                        BuildXmlDocument(action)
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<GroupContactApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<GroupContactApiResult> SendXMLAsync(Action action)
        {
            try
            {
                if (action == Action.Add)
                {
                    return await HttpRequest.PostXMLAsync<GroupContactApiResult>(
                        new PostHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }
                if (action == Action.Remove)
                {
                    return await HttpRequest.DeleteXMLAsync<GroupContactApiResult>(
                        new DeleteHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }

                return await HttpRequest.GetXMLAsync<GroupContactApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(action),
                        User,
                        BuildXmlDocument(action)
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<GroupContactApiResult>(e.Message);
            }
        }

        public void SetContactProperty<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<ContactModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, ContactModel>(propertyExpression);
            PropertyHelper.SetProperty(Contact, convertedExpression, value);
        }

        public void SetGroupProperty<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<GroupModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, GroupModel>(propertyExpression);
            PropertyHelper.SetProperty(Group, convertedExpression, value);
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
        /// Add group contact
        /// </summary>
        /// <returns>GroupContactApiResult</returns>
        private GroupContactApiResult Add()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("AuthToken required for this module.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Group: Please specify group.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Contact: Please specify contact.");
            }
            return SendXML(Action.Add);
        }

        /// <summary>
        /// Add group contact
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Add(GroupModel group, ContactModel contact)
        {
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return Add();
        }

        /// <summary>
        /// Add group contact
        /// </summary>
        /// <param name="groupID">GroupID</param>
        /// <param name="contactID">ContactID</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Add(GroupID groupID, ContactID contactID)
        {
            if (groupID is not null)
            {
                Group.GroupID = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            return Add();
        }

        /// <summary>
        /// Add group contact
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Add(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        )
        {
            if (groupID is not null)
            {
                Group.GroupCode = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return Add();
        }

        #endregion

        #region AddAsync
        /// <summary>
        /// Add group contact (async)
        /// </summary>
        /// <returns>Task<GroupContactApiResult></returns>
        [ComVisible(false)]
        private async Task<GroupContactApiResult> AddAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("AuthToken required for this module.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Group: Please specify group.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Contact: Please specify contact.");
            }
            return await SendXMLAsync(Action.Add);
        }

        /// <summary>
        /// Add group contact (async)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="groupCode">Group Code</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        [ComVisible(false)]
        public async Task<GroupContactApiResult> AddAsync(GroupModel group, ContactModel contact)
        {
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return await AddAsync();
        }

        /// <summary>
        /// Add group contact (async)
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        [ComVisible(false)]
        public async Task<GroupContactApiResult> AddAsync(GroupID groupID, ContactID contactID)
        {
            if (groupID is not null)
            {
                Group.GroupID = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            return await AddAsync();
        }

        /// <summary>
        /// Add group contact (async)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        [ComVisible(false)]
        public async Task<GroupContactApiResult> AddAsync(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        )
        {
            if (groupID is not null)
            {
                Group.GroupCode = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return await AddAsync();
        }
        #endregion

        #region Remove
        /// <summary>
        /// Remove group contact
        /// </summary>
        /// <returns>GroupContactApiResult</returns>
        private GroupContactApiResult Remove()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("AuthToken required for this module.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Group: Please specify group.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Contact: Please specify contact.");
            }
            return SendXML(Action.Remove);
        }

        /// <summary>
        /// Remove group contact
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Remove(GroupModel group, ContactModel contact)
        {
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return Remove();
        }

        /// <summary>
        /// Remove group contact
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Remove(GroupID groupID, ContactID contactID)
        {
            if (groupID is not null)
            {
                Group.GroupID = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            return Remove();
        }

        /// <summary>
        /// Remove group contact
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <param name="group">GroupModel</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Remove(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        )
        {
            if (groupID is not null)
            {
                Group.GroupID = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return Remove();
        }
        #endregion

        #region RemoveAsync
        /// <summary>
        /// Remove group contact (async)
        /// </summary>
        /// <returns>Task<GroupContactApiResult></returns>
        private async Task<GroupContactApiResult> RemoveAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("AuthToken required for this module.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Group: Please specify group.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Contact: Please specify contact.");
            }
            return await SendXMLAsync(Action.Remove);
        }

        /// <summary>
        /// Remove group contact (async)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> RemoveAsync(GroupModel group, ContactModel contact)
        {
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return await RemoveAsync();
        }

        /// <summary>
        /// Remove group contact (async)
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> RemoveAsync(GroupID groupID, ContactID contactID)
        {
            if (groupID is not null)
            {
                Group.GroupID = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            return await RemoveAsync();
        }

        /// <summary>
        /// Remove group contact (async)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> RemoveAsync(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        )
        {
            if (groupID is not null)
            {
                Group.GroupID = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return await RemoveAsync();
        }

        #endregion

        #region Read
        /// <summary>
        /// Read group contact
        /// </summary>
        /// <returns>GroupContactApiResult</returns>
        private GroupContactApiResult Read()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("AuthToken required for this module.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Group: Please specify group.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Contact: Please specify contact.");
            }
            return SendXML(Action.Read);
        }

        /// <summary>
        /// Read group contact
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Read(GroupModel group, ContactModel contact)
        {
            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return Read();
        }

        /// <summary>
        /// Read group contact
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Read(GroupID groupID, ContactID contactID)
        {
            if (groupID is not null)
            {
                Group.GroupCode = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            return Read();
        }

        /// <summary>
        /// Read group contact
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Read(
                GroupModel group = null,
                ContactModel contact = null,
                GroupID groupID = null,
                ContactID contactID = null
            )
        {
            if (groupID is not null)
            {
                Group.GroupCode = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            if (group is not null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is not null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return Read();
        }

        /// <summary>
        /// Read group contact
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Get(GroupModel group, ContactModel contact) => Read(group, contact);

        /// <summary>
        /// Read group contact
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Get(GroupID groupID, ContactID contactID) => Read(groupID, contactID);

        /// <summary>
        /// Read group contact
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>GroupContactApiResult</returns>
        public GroupContactApiResult Get(GroupModel group = null, ContactModel contact = null, GroupID groupID = null, ContactID contactID = null) => Read(group, contact, groupID, contactID);

        #endregion

        #region ReadAsync
        /// <summary>
        /// Read group contact  (async)
        /// </summary>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> ReadAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("AuthToken required for this module.");
            }
            if (Group is null || PropertyHelper.IsNewObject(Group))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Group: Please specify group.");
            }
            if (Contact is null || PropertyHelper.IsNewObject(Contact))
            {
                return ResultHelper.RespondError<GroupContactApiResult>("Empty Contact: Please specify contact.");
            }
            return await SendXMLAsync(Action.Read);
        }

        /// <summary>
        /// Read group contact (async)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> ReadAsync(GroupModel group, ContactModel contact)
        {
            if (group is null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return await ReadAsync();
        }

        /// <summary>
        /// Read group contact (async)
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> ReadAsync(GroupID groupID, ContactID contactID)
        {
            if (groupID is not null)
            {
                Group.GroupID = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = contactID;
            }

            return await ReadAsync();
        }

        /// <summary>
        /// Read group contact (async)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> ReadAsync(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        )
        {
            if (groupID is not null)
            {
                Group.GroupID = groupID;
            }
            if (contactID is not null)
            {
                Contact.ContactID = new ContactID(contactID);
            }

            if (group is null)
            {
                Group = Mapper.Map(Group, group);
            }
            if (contact is null)
            {
                Contact = Mapper.Map(Contact, contact);
            }

            return await ReadAsync();
        }

        /// <summary>
        /// Read group contact (async)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> GetAsync(GroupModel group, ContactModel contact) => await ReadAsync(group, contact);

        /// <summary>
        /// Read group contact (async)
        /// </summary>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> GetAsync(GroupID groupID, ContactID contactID) => await ReadAsync(groupID, contactID);

        /// <summary>
        /// Read group contact (async)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <param name="contact">ContactModel</param>
        /// <param name="groupID">Group ID</param>
        /// <param name="contactID">Contact ID</param>
        /// <returns>Task<GroupContactApiResult></returns>
        public async Task<GroupContactApiResult> GetAsync(GroupModel group = null, ContactModel contact = null, GroupID groupID = null, ContactID contactID = null) => await ReadAsync(group, contact, groupID, contactID);

        #endregion
    }
}
