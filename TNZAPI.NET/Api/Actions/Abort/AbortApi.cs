using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Actions.Abort.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Actions.Abort
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class AbortApi : IAbortApi
    {
        private ITNZAuth User = new TNZApiUser();

        private AbortRequestOptions Options { get; set; } = new AbortRequestOptions();

        /// <summary>
        /// Abort Pending Messages
        /// </summary>
        /// <returns></returns>
        public AbortApi()
        {
        }

        /// <summary>
        /// Abort Pending Messages
        /// </summary>
        /// <param name="authToken">API Key for TNZAPI</param>
        /// <returns></returns>
        public AbortApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Abort Pending Messages
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public AbortApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Abort Pending Messages
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public AbortApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Abort Pending Messages
        /// </summary>
        /// <param name="auth">IAuth</param>
        public AbortApi(ITNZAuth auth)
        {
            User = auth;
        }

        private XmlDocument BuildXmlDocument()
        {
            #region XML Sample
            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <root>
              <Sender>application@domain.com</Sender>
              <APIKey>ta8wr7ymd</APIKey>
              <Type>Abort</Type>
              <APIVersion>1.04</APIVersion>
              <MessageID>ID123456</MessageID>
            </root>
            */

            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("AbortRequest");
            xmlDoc.AppendChild(rootNode);

            if (User.AuthToken.Equals(""))
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Sender", User.Sender));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "APIKey", User.APIKey));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessageID", Options.MessageID));
            }

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/set/abort");

            if (User.AuthToken.Equals("") == false)
            {
                requestUri.Append($"/{Options.MessageID}");
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private AbortApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.PatchXMLAsync<AbortApiResult>(
                    new PatchHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<AbortApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<AbortApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.PatchXMLAsync<AbortApiResult>(
                    new PatchHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<AbortApiResult>(e.Message);
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

        #region Submit

        /// <summary>
        /// Abort message in PENDING/DELAYED state
        /// </summary>
        /// <returns>AbortResult</returns>
        private AbortApiResult Submit()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<AbortApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<AbortApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<AbortApiResult>("Empty Message ID: Please specify MessageID");
            }
            return SendXML();
        }

        /// <summary>
        /// Abort message in PENDING/DELAYED state
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns>AbortResult</returns>
        [ComVisible(false)]
        public AbortApiResult Submit(string messageID)
        {
            Options = new AbortRequestOptions
            {
                MessageID = messageID
            };

            return Submit();
        }

        /// <summary>
        /// Abort message in PENDING/DELAYED state
        /// </summary>
        /// <param name="options">AbortOptions</param>
        /// <returns>AbortResult</returns>
        [ComVisible(false)]
        public AbortApiResult Submit(AbortRequestOptions options)
        {
            Options = Mapper.Map(Options, options);

            return Submit();
        }
        #endregion Submit

        #region SubmitAsync
        /// <summary>
        /// Abort message in PENDING/DELAYED state (Async)
        /// </summary>
        /// <returns>AbortResult</returns>
        [ComVisible(false)]
        public async Task<AbortApiResult> SubmitAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<AbortApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<AbortApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<AbortApiResult>("Empty Message ID: Please specify MessageID");
            }
            return await SendXMLAsync();
        }

        /// <summary>
        /// Abort message in PENDING/DELAYED state (Async)
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns></returns>
        [ComVisible(false)]
        public async Task<AbortApiResult> SubmitAsync(string messageID)
        {
            Options = new AbortRequestOptions
            {
                MessageID = messageID
            };

            return await SubmitAsync();
        }

        /// <summary>
        /// Abort message in PENDING/DELAYED state (Async)
        /// </summary>
        /// <param name="options">AbortOptions</param>
        /// <returns>Task<AbortResult></returns>
        [ComVisible(false)]
        public async Task<AbortApiResult> SubmitAsync(AbortRequestOptions options)
        {
            Options = Mapper.Map(Options, options);

            return await SubmitAsync();
        }
        #endregion SubmitAsync
    }
}
