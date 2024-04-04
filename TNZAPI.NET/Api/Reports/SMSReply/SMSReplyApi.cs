using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Reports.SMSReply.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Reports;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Reports.SMSReply
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class SMSReplyApi : ISMSReplyApi
    {
        private ITNZAuth User = new TNZApiUser();

        private SMSReplyRequestOptions Options { get; set; } = new SMSReplyRequestOptions();

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <returns></returns>
        public SMSReplyApi()
        {
        }

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        /// <returns></returns>
        public SMSReplyApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public SMSReplyApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public SMSReplyApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <param name="auth">IAuth</param>
        public SMSReplyApi(ITNZAuth auth)
        {
            User = auth;
        }

        private XmlDocument BuildXmlDocument()
        {
            #region XML Sample
            /*
            <root>
                <Sender>application@domain.com</Sender>
                <APIKey>ta8wr7ymd</APIKey>
                <Type>SMSReceived</Type>
                <APIVersion>1.03</APIVersion>
                <MessageID>ID123456</MessageID>
            </root>      
            */
            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("StatusRequest");
            xmlDoc.AppendChild(rootNode);

            if (User.AuthToken.Equals(""))
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Sender", User.Sender));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "APIKey", User.APIKey));
            }
            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessageID", Options.MessageID));

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/get/sms/reply");

            if (User.AuthToken.Equals("") == false)
            {
                requestUri.Append($"/{Options.MessageID}");
                requestUri.Append($"?recordsPerPage={Options.RecordsPerPage}&page={Options.Page}");
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private SMSReplyApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.GetXMLAsync<SMSReplyApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<SMSReplyApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<SMSReplyApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.GetXMLAsync<SMSReplyApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<SMSReplyApiResult>(e.Message);
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

        #region Poll
        /// <summary>
        /// Poll message status
        /// </summary>
        /// <returns>StatusResult</returns>
        public SMSReplyApiResult Poll()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<SMSReplyApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<SMSReplyApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options is null)
            {
                return ResultHelper.RespondError<SMSReplyApiResult>("Empty Options: Please set options");
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<SMSReplyApiResult>("Empty Message ID: Please specify MessageID");
            }
            return SendXML();
        }

        /// <summary>
        /// Poll SMS replies
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>SMSReplyApiResult</returns>
        [ComVisible(false)]
        public SMSReplyApiResult Poll(MessageID messageID)
        {
            return Poll(new SMSReplyRequestOptions()
            {
                MessageID = messageID
            });
        }

        /// <summary>
        /// Poll SMS replies
        /// </summary>
        /// <param name="options">SMSReplyRequestOptions</param>
        /// <returns>SMSReplyApiResult</returns>
        [ComVisible(false)]
        public SMSReplyApiResult Poll(SMSReplyRequestOptions options)
        {
            Options = Mapper.Update(new SMSReplyRequestOptions(), options);

            return Poll();
        }

        /// <summary>
        /// Poll SMS replies
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="listOptions">IListRequestOptions</param>
        /// <returns>SMSReplyApiResult</returns>
        [ComVisible(false)]
        public SMSReplyApiResult Poll(MessageID messageID, IListRequestOptions listOptions = null)
        {
            if (messageID is not null)
            {
                Options.MessageID = messageID;
            }

            if (listOptions is not null)
            {
                Options.RecordsPerPage = listOptions.RecordsPerPage;
                Options.Page = listOptions.Page;
            }

            return Poll();
        }
        #endregion Poll

        #region PollAsync
        /// <summary>
        /// Poll message status (async)
        /// </summary>
        /// <returns>Task<SMSReplyApiResult></returns>
        [ComVisible(false)]
        public async Task<SMSReplyApiResult> PollAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<SMSReplyApiResult>("Empty sender: Please specify Sender");
                }
                if (Options is null)
                {
                    return ResultHelper.RespondError<SMSReplyApiResult>("Empty Options: Please set options");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<SMSReplyApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<SMSReplyApiResult>("Empty Message ID: Please specify MessageID");
            }

            return await SendXMLAsync();
        }

        /// <summary>
        /// Poll message status (async)
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>Task<SMSReplyApiResult></returns>
        [ComVisible(false)]
        public async Task<SMSReplyApiResult> PollAsync(MessageID messageID)
        {
            return await PollAsync(new SMSReplyRequestOptions()
            {
                MessageID = messageID
            });
        }

        /// <summary>
        /// Poll message status (async)
        /// </summary>
        /// <param name="options">SMSReplyRequestOptions</param>
        /// <returns>Task<SMSReplyApiResult></returns>
        [ComVisible(false)]
        public async Task<SMSReplyApiResult> PollAsync(SMSReplyRequestOptions options)
        {
            Options = Mapper.Update(new SMSReplyRequestOptions(), options);

            return await PollAsync();
        }

        /// <summary>
        /// Poll SMS replies
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <param name="listOptions">IListRequestOptions</param>
        /// <returns>SMSReplyApiResult</returns>
        [ComVisible(false)]
        public async Task<SMSReplyApiResult> PollAsync(MessageID messageID, IListRequestOptions listOptions = null)
        {
            if (messageID is not null)
            {
                Options.MessageID = messageID;
            }

            if (listOptions is not null)
            {
                Options.RecordsPerPage = listOptions.RecordsPerPage;
                Options.Page = listOptions.Page;
            }

            return await PollAsync();
        }
        #endregion Poll

        #region Deprecated
        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        public SMSReplyApiResult Poll(string messageID) => Poll(new MessageID(messageID));

        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        public SMSReplyApiResult Poll(string messageID, IListRequestOptions listOptions = null) => Poll(new MessageID(messageID), listOptions);

        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        public async Task<SMSReplyApiResult> PollAsync(string messageID) => await PollAsync(new MessageID(messageID));

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		public async Task<SMSReplyApiResult> PollAsync(string messageID, IListRequestOptions listOptions = null) => await PollAsync(new MessageID(messageID), listOptions);
		#endregion
	}
}
