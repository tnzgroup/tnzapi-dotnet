using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Actions.Resubmit.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Actions.Resubmit
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ResubmitApi : IResubmitApi
    {
        private ITNZAuth User = new TNZApiUser();

        private ResubmitRequestOptions Options { get; set; } = new ResubmitRequestOptions();

        /// <summary>
        /// Resubmit COMPLETED/TRANSMIT message
        /// </summary>
        /// <param name="sender">API Username - Email Address</param>
        /// <param name="api_key">API Key for TNZAPI</param>
        /// <returns></returns>
        public ResubmitApi()
        {
        }

        /// <summary>
        /// Resubmit COMPLETED/TRANSMIT message
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        /// <returns></returns>
        public ResubmitApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Resubmit COMPLETED/TRANSMIT message
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public ResubmitApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Resubmit COMPLETED/TRANSMIT message
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public ResubmitApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Resubmit COMPLETED/TRANSMIT message
        /// </summary>
        /// <param name="auth">IAuth</param>
        public ResubmitApi(ITNZAuth auth)
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
              <MessageID>ID123456</MessageID>
            </root>
            */
            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("ResubmitRequest");
            xmlDoc.AppendChild(rootNode);

            if (User.AuthToken.Equals(""))
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Sender", User.Sender));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "APIKey", User.APIKey));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessageID", Options.MessageID));
            }

            if (Options.SendTime > DateTime.Now)
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "SendTime", Options.SendTime.ToString("yyyy-MM-ddTHH:mm:ss")));
            }

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/set/resubmit");

            if (User.AuthToken.Equals("") == false)
            {
                requestUri.Append($"/{Options.MessageID}");
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private ResubmitApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.PatchXMLAsync<ResubmitApiResult>(
                    new PatchHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ResubmitApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<ResubmitApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.PatchXMLAsync<ResubmitApiResult>(
                    new PatchHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<ResubmitApiResult>(e.Message);
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
        /// Resubmit message
        /// </summary>
        /// <returns>ResubmitResult</returns>
        [ComVisible(false)]
        public ResubmitApiResult Submit()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<ResubmitApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<ResubmitApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<ResubmitApiResult>("Empty Message ID: Please specify MessageID");
            }
            return SendXML();
        }

        /// <summary>
        /// Resubmit message
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns>ResubmitResult</returns>
        public ResubmitApiResult Submit(string messageID)
        {
            Options = new ResubmitRequestOptions
            {
                MessageID = messageID
            };

            return Submit();
        }

        /// <summary>
        /// Resubmit message
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <param name="sendTime">Date/Time</param>
        /// <returns></returns>
        [ComVisible(false)]
        public ResubmitApiResult Submit(string messageID, DateTime sendTime)
        {
            Options = new ResubmitRequestOptions
            {
                MessageID = messageID,
                SendTime = sendTime
            };

            return Submit();
        }

        /// <summary>
        /// Resubmit message using options
        /// </summary>
        /// <param name="options">ResubmitOptions</param>
        /// <returns>ResubmitResult</returns>
        [ComVisible(false)]
        public ResubmitApiResult Submit(ResubmitRequestOptions options)
        {
            Options = Mapper.Map(Options, options);

            return Submit();
        }
        #endregion Submit

        #region SubmitAsync
        /// <summary>
        /// Resubmit message (async)
        /// </summary>
        /// <returns>ResubmitResult</returns>
        [ComVisible(false)]
        public async Task<ResubmitApiResult> SubmitAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<ResubmitApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<ResubmitApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<ResubmitApiResult>("Empty Message ID: Please specify MessageID");
            }
            return await SendXMLAsync();
        }

        /// <summary>
        /// Resubmit message (async)
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns></returns>
        [ComVisible(false)]
        public async Task<ResubmitApiResult> SubmitAsync(string messageID)
        {
            Options = new ResubmitRequestOptions
            {
                MessageID = messageID
            };

            return await SubmitAsync();
        }

        /// <summary>
        /// Resubmit message (async)
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <param name="sendTime">Date/Time</param>
        /// <returns></returns>
        [ComVisible(false)]
        public async Task<ResubmitApiResult> SubmitAsync(string messageID, DateTime sendTime)
        {
            Options = new ResubmitRequestOptions
            {
                MessageID = messageID,
                SendTime = sendTime
            };

            return await SubmitAsync();
        }

        /// <summary>
        /// Resubmit message using options (async)
        /// </summary>
        /// <param name="options">ResubmitOptions</param>
        /// <returns>Task<ResubmitResult></returns>
        [ComVisible(false)]
        public async Task<ResubmitApiResult> SubmitAsync(ResubmitRequestOptions options)
        {
            Options = Mapper.Map(Options, options);

            return await SubmitAsync();
        }
        #endregion SubmitAsync
    }
}
