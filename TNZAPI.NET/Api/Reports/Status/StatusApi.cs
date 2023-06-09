using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Reports.Status.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Reports;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Reports.Status
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class StatusApi : IStatusApi
    {
        private ITNZAuth User = new TNZApiUser();

        private StatusRequestOptions Options { get; set; } = new StatusRequestOptions();

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <returns></returns>
        public StatusApi()
        {
        }

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        /// <returns></returns>
        public StatusApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public StatusApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public StatusApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Get message status from TNZAPI
        /// </summary>
        /// <param name="auth">IAuth</param>
        public StatusApi(ITNZAuth auth)
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

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/get/status");

            if (User.AuthToken.Equals("") == false)
            {
                requestUri.Append($"/{Options.MessageID}");
                requestUri.Append($"?recordsPerPage={Options.RecordsPerPage}&page={Options.Page}");
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private StatusApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.GetXMLAsync<StatusApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<StatusApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<StatusApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.GetXMLAsync<StatusApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<StatusApiResult>(e.Message);
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
        public StatusApiResult Poll()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<StatusApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<StatusApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options is null)
            {
                return ResultHelper.RespondError<StatusApiResult>("Empty Options: Please set options");
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<StatusApiResult>("Empty Message ID: Please specify MessageID");
            }
            return SendXML();
        }

        /// <summary>
        /// Poll message status
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>StatusResult</returns>
        [ComVisible(false)]
        public StatusApiResult Poll(string messageID)
        {
            return Poll(new StatusRequestOptions()
            {
                MessageID = messageID
            });
        }

        /// <summary>
        /// Poll message status
        /// </summary>
        /// <param name="options">StatusOptions</param>
        /// <returns>StatusResult</returns>
        [ComVisible(false)]
        public StatusApiResult Poll(StatusRequestOptions options)
        {
            Options = Mapper.Update(new StatusRequestOptions(), options);

            return Poll();
        }
        #endregion Poll

        #region PollAsync
        /// <summary>
        /// Poll message status (async)
        /// </summary>
        /// <returns>Task<StatusResult></returns>
        [ComVisible(false)]
        public async Task<StatusApiResult> PollAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<StatusApiResult>("Empty sender: Please specify Sender");
                }
                if (Options is null)
                {
                    return ResultHelper.RespondError<StatusApiResult>("Empty Options: Please set options");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<StatusApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<StatusApiResult>("Empty Message ID: Please specify MessageID");
            }

            return await SendXMLAsync();
        }

        /// <summary>
        /// Poll message status (async)
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns>Task<StatusResult></returns>
        [ComVisible(false)]
        public async Task<StatusApiResult> PollAsync(string messageID)
        {
            return await PollAsync(new StatusRequestOptions()
            {
                MessageID = messageID
            });
        }

        /// <summary>
        /// Poll message status (async)
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Task<StatusResult></returns>
        [ComVisible(false)]
        public async Task<StatusApiResult> PollAsync(StatusRequestOptions options)
        {
            Options = Mapper.Update(new StatusRequestOptions(), options);

            return await PollAsync();
        }
        #endregion Poll
    }
}
