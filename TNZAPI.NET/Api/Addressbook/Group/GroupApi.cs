﻿using System.Runtime.InteropServices;
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
    public class GroupApi : IGroupApi
    {
        private ITNZAuth User = new TNZApiUser();

        private enum Action { Create, Update, Delete, Read };

        public GroupModel Entity { get; set; } = new GroupModel();

        /// <summary>
        /// Update group details using TNZAPI
        /// </summary>
        public GroupApi()
        {

        }

        /// <summary>
        /// Update group details using TNZAPI
        /// </summary>
        /// <param name="authToken">API Key for TNZAPI</param>
        /// <returns></returns>
        /// 
        public GroupApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Update group details using TNZAPI
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public GroupApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Update group details using TNZAPI
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public GroupApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Update group details using TNZAPI
        /// </summary>
        /// <param name="auth">IAuth</param>
        public GroupApi(ITNZAuth auth)
        {
            User = auth;
        }

        private XmlDocument BuildXmlDocument(Action action)
        {
            #region XML Sample

            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <Group>
                <GroupCode>Test-Group</GroupCode>
                <GroupName>Test Group</GroupName>
                <SubAccount>SubAccount</SubAccount>
                <Department>Department</Department>
                <ViewEditBy>Account</ViewEditBy>
            </Group>     
            */

            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("Group");
            xmlDoc.AppendChild(rootNode);

            if (action == Action.Create)
            {
                if (Entity.GroupCode is not null && Entity.GroupCode != "")
                {
                    rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "GroupCode", Entity.GroupCode));
                }
            }
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "GroupName", Entity.GroupName));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "SubAccount", Entity.SubAccount));
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Department", Entity.Department));
            if (Entity.ViewEditBy is not null && Entity.ViewEditBy != "")
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ViewEditBy", Entity.ViewEditBy));
            }

            return xmlDoc;
        }

        private string BuildAPIURL(Action action)
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/group");

            switch (action)
            {
                case Action.Read:
                case Action.Update:
                case Action.Delete:
                    requestUri.Append($"/{Entity.GroupCode}");
                    break;
                case Action.Create:
                    break;
                default:
                    break;
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private GroupApiResult SendXML(Action action)
        {
            try
            {
                if (action == Action.Create)
                {
                    return Task.Run(() => HttpRequest.PostXMLAsync<GroupApiResult>(
                        new PostHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }
                if (action == Action.Update)
                {
                    return Task.Run(() => HttpRequest.PatchXMLAsync<GroupApiResult>(
                        new PatchHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }
                if (action == Action.Delete)
                {
                    return Task.Run(() => HttpRequest.DeleteXMLAsync<GroupApiResult>(
                        new DeleteHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ))).Result;
                }

                return Task.Run(() => HttpRequest.GetXMLAsync<GroupApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(action),
                        User,
                        BuildXmlDocument(action)
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<GroupApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<GroupApiResult> SendXMLAsync(Action action)
        {
            try
            {
                if (action == Action.Create)
                {
                    return await HttpRequest.PostXMLAsync<GroupApiResult>(
                        new PostHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }
                if (action == Action.Update)
                {
                    return await HttpRequest.PatchXMLAsync<GroupApiResult>(
                        new PatchHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }
                if (action == Action.Delete)
                {
                    return await HttpRequest.DeleteXMLAsync<GroupApiResult>(
                        new DeleteHttpRequest
                        (
                            BuildAPIURL(action),
                            User,
                            BuildXmlDocument(action)
                        ));
                }

                return await HttpRequest.GetXMLAsync<GroupApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(action),
                        User,
                        BuildXmlDocument(action)
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<GroupApiResult>(e.Message);
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

        #region Create
        /// <summary>
        /// Create group
        /// </summary>
        /// <returns>GroupResult</returns>
        private GroupApiResult Create()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel: Please specify any value");
            }
            if (Entity.GroupName is null || Entity.GroupName == "")
            {
                return ResultHelper.RespondError<GroupApiResult>("GroupModel.GroupName is required");
            }
            return SendXML(Action.Create);
        }

        /// <summary>
        /// Create group
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult Create(GroupModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return Create();
        }

        /// <summary>
        /// Create group
        /// </summary>
        /// <param name="groupCode">Group Code</param>
        /// <param name="groupName">Group Name</param>
        /// <param name="subaccount">SubAccount</param>
        /// <param name="department">Department</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult Create(string groupCode = null, string groupName = null, string subaccount = null, string department = null)
        {
            if (groupCode is not null)
            {
                Entity.GroupCode = groupCode;
            }
            if (groupName is not null)
            {
                Entity.GroupName = groupName;
            }
            if (subaccount is not null)
            {
                Entity.SubAccount = subaccount;
            }
            if (department is not null)
            {
                Entity.Department = department;
            }

            return Create();
        }
        #endregion Update

        #region CreateAsync
        /// <summary>
        /// Create group (async)
        /// </summary>
        /// <returns>Task<GroupResult></returns>
        [ComVisible(false)]
        private async Task<GroupApiResult> CreateAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty Model: Please specify any value");
            }
            if (Entity.GroupName is null || Entity.GroupName == "")
            {
                return ResultHelper.RespondError<GroupApiResult>("Model.GroupName is required");
            }
            return await SendXMLAsync(Action.Create);
        }

        /// <summary>
        /// Create group (async)
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>Task<GroupResult></returns>
        [ComVisible(false)]
        public async Task<GroupApiResult> CreateAsync(GroupModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return await CreateAsync();
        }

        /// <summary>
        /// Create group (async)
        /// </summary>
        /// <param name="groupCode">Group Code</param>
        /// <param name="groupName">Group Name</param>
        /// <param name="subaccount">SubAccount</param>
        /// <param name="department">Department</param>
        /// <returns></returns>
        [ComVisible(false)]
        public async Task<GroupApiResult> CreateAsync(string groupCode = null, string groupName = null, string subaccount = null, string department = null)
        {
            if (groupCode is not null)
            {
                Entity.GroupCode = groupCode;
            }
            if (groupName is not null)
            {
                Entity.GroupName = groupName;
            }
            if (subaccount is not null)
            {
                Entity.SubAccount = subaccount;
            }
            if (department is not null)
            {
                Entity.Department = department;
            }
            return await CreateAsync();
        }
        #endregion CreateAsync

        #region Update
        /// <summary>
        /// Update group
        /// </summary>
        /// <returns>GroupResult</returns>
        private GroupApiResult Update()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty Group: Please specify any value");
            }
            if (Entity.GroupCode is null || Entity.GroupCode == "")
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty Group.GroupCode: Please specify contact ID");
            }
            return SendXML(Action.Update);
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult Update(GroupModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return Update();
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="groupCode">Group code</param>
        /// <param name="groupName">Group name</param>
        /// <param name="subaccount">Subaccount</param>
        /// <param name="department">Department</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult Update(string groupCode = null, string groupName = null, string subaccount = null, string department = null)
        {
            if (groupCode is not null)
            {
                Entity.GroupCode = groupCode;
            }
            if (groupName is not null)
            {
                Entity.GroupName = groupName;
            }
            if (subaccount is not null)
            {
                Entity.SubAccount = subaccount;
            }
            if (department is not null)
            {
                Entity.Department = department;
            }

            return Update();
        }
        #endregion

        #region UpdateAsync
        /// <summary>
        /// Update group (async)
        /// </summary>
        /// <returns>Task<GroupResult></returns>
        [ComVisible(false)]
        private async Task<GroupApiResult> UpdateAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty Group: Please specify any value");
            }
            if (Entity.GroupCode is null || Entity.GroupCode == "")
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty Group.GroupCode: Please specify contact ID");
            }
            return await SendXMLAsync(Action.Update);
        }

        /// <summary>
        /// Update group (async)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Task<GroupResult></returns>
        [ComVisible(false)]
        public async Task<GroupApiResult> UpdateAsync(GroupModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return await UpdateAsync();
        }

        public async Task<GroupApiResult> UpdateAsync(string groupCode = null, string groupName = null, string subaccount = null, string department = null)
        {
            if (groupCode is not null)
            {
                Entity.GroupCode = groupCode;
            }
            if (groupName is not null)
            {
                Entity.GroupName = groupName;
            }
            if (subaccount is not null)
            {
                Entity.SubAccount = subaccount;
            }
            if (department is not null)
            {
                Entity.Department = department;
            }
            return await UpdateAsync();
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete group
        /// </summary>
        /// <returns>GroupResult</returns>
        private GroupApiResult Delete()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel: Please specify any value");
            }
            if (Entity.GroupCode is null || Entity.GroupCode == "")
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel.GroupCode: Please specify Group Code");
            }
            return SendXML(Action.Delete);
        }

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult Delete(GroupModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return Delete();
        }

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="groupCode">Group Code</param>
        /// <returns></returns>
        public GroupApiResult DeleteByGroupCode(string groupCode)
        {
            Entity.GroupCode = groupCode;

            return Delete();
        }
        #endregion

        #region DeleteAsync
        /// <summary>
        /// Delete group (async)
        /// </summary>
        /// <returns>Task<GroupResult></returns>
        [ComVisible(false)]
        private async Task<GroupApiResult> DeleteAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel: Please specify any value");
            }
            if (Entity.GroupCode is null || Entity.GroupCode == "")
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel.GroupCode: Please specify Group Code");
            }
            return await SendXMLAsync(Action.Delete);
        }

        /// <summary>
        /// Delete group (async)
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>Task<GroupResult></returns>
        [ComVisible(false)]
        public async Task<GroupApiResult> DeleteAsync(GroupModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return await DeleteAsync();
        }

        /// <summary>
        /// Delete group using GroupCode
        /// </summary>
        /// <param name="groupCode">Group Code</param>
        /// <returns>Task<GroupResult></returns>
        [ComVisible(false)]
        public async Task<GroupApiResult> DeleteByGroupCodeAsync(string groupCode)
        {
            Entity.GroupCode = groupCode;

            return await DeleteAsync();
        }
        #endregion

        #region Read
        /// <summary>
        /// Read group
        /// </summary>
        /// <returns>GroupResult</returns>
        private GroupApiResult Read()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel: Please specify any value");
            }
            if (Entity.GroupCode is null || Entity.GroupCode == "")
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel.GroupCode: Please specify Group Code");
            }
            return SendXML(Action.Read);
        }

        /// <summary>
        /// Read group
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult Read(GroupModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return Read();
        }

        /// <summary>
        /// Read group using GroupCode
        /// </summary>
        /// <param name="groupCode">GroupCode</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult ReadByGroupCode(string groupCode)
        {
            Entity.GroupCode = groupCode;

            return Read();
        }

        /// <summary>
        /// Read group
        /// </summary>
        /// <returns>GroupResult</returns>
        public GroupApiResult Get() => Read();

        /// <summary>
        /// Read group
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult Get(GroupModel entity) => Read(entity);

        /// <summary>
        /// Read group
        /// </summary>
        /// <param name="groupCode">Group ID</param>
        /// <returns>GroupResult</returns>
        public GroupApiResult GetByGroupCode(string groupCode) => ReadByGroupCode(groupCode);
        #endregion

        #region ReadAsync
        /// <summary>
        /// Read group (async)
        /// </summary>
        /// <returns>Task<GroupResult></returns>
        private async Task<GroupApiResult> ReadAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                return ResultHelper.RespondError<GroupApiResult>("AuthToken required for this module.");
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel: Please specify any value");
            }
            if (Entity.GroupCode is null || Entity.GroupCode == "")
            {
                return ResultHelper.RespondError<GroupApiResult>("Empty GroupModel.GroupCode: Please specify Group Code");
            }
            return await SendXMLAsync(Action.Read);
        }

        /// <summary>
        /// Read group (async)
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>Task<GroupResult></returns>
        public async Task<GroupApiResult> ReadAsync(GroupModel entity)
        {
            Entity = Mapper.Map(Entity, entity);

            return await ReadAsync();
        }

        /// <summary>
        /// Read group (async)
        /// </summary>
        /// <param name="groupCode">Group Code</param>
        /// <returns>Task<GroupResult></returns>
        public async Task<GroupApiResult> ReadByGroupCodeAsync(string groupCode)
        {
            Entity.GroupCode = groupCode;

            return await ReadAsync();
        }

        /// <summary>
        /// Read group (async)
        /// </summary>
        /// <returns>Task<GroupResult></returns>
        public async Task<GroupApiResult> GetAsync() => await ReadAsync();

        /// <summary>
        /// Read group (async)
        /// </summary>
        /// <param name="entity">GroupModel</param>
        /// <returns>Task<GroupResult></returns>
        public async Task<GroupApiResult> GetAsync(GroupModel entity) => await ReadAsync(entity);

        /// <summary>
        /// Read group (async)
        /// </summary>
        /// <param name="groupCode">Group Code</param>
        /// <returns>Task<GroupResult></returns>
        public async Task<GroupApiResult> GetByGroupCodeAsync(string groupCode) => await ReadByGroupCodeAsync(groupCode);
        #endregion

        #region List
        public GroupListApiResult List()
        {
            return new GroupListApi(User)
                .List();
        }

        public GroupListApiResult List(IListRequestOptions options)
        {
            return new GroupListApi(User)
                .List(options);
        }
        #endregion

        #region ListAsync
        public async Task<GroupListApiResult> ListAsync()
        {
            return await new GroupListApi(User)
                .ListAsync();
        }

        public async Task<GroupListApiResult> ListAsync(IListRequestOptions options)
        {
            return await new GroupListApi(User)
                .ListAsync(options);
        }
        #endregion
    }
}
