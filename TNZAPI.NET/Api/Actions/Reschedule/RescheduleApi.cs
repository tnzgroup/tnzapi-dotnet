using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Actions.Reschedule.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Actions.Reschedule
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class RescheduleApi : IRescheduleApi
    {
        private ITNZAuth User = new TNZApiUser();

        public RescheduleRequestOptions Options { get; set; } = new RescheduleRequestOptions();

        /// <summary>
        /// Reschedule DELAYED message
        /// </summary>
        public RescheduleApi()
        {
        }

        /// <summary>
        /// Reschedule DELAYED message
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public RescheduleApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Reschedule DELAYED message
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        public RescheduleApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Reschedule DELAYED message
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public RescheduleApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Reschedule DELAYED message
        /// </summary>
        /// <param name="auth">IAuth</param>
        public RescheduleApi(ITNZAuth auth)
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
              <SendTime>2023-05-12 12:00:00</SendTime>
            </root>
            */

            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("RescheduleRequest");
            xmlDoc.AppendChild(rootNode);

            if (User.AuthToken.Equals(""))
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Sender", User.Sender));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "APIKey", User.APIKey));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessageID", Options.MessageID));
            }

            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "SendTime", Options.SendTime.ToString("yyyy-MM-ddTHH:mm:ss")));

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/set/reschedule");

            if (User.AuthToken.Equals("") == false)
            {
                requestUri.Append($"/{Options.MessageID}");
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private RescheduleApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.PatchXMLAsync<RescheduleApiResult>(
                    new PatchHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<RescheduleApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<RescheduleApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.PatchXMLAsync<RescheduleApiResult>(
                    new PatchHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<RescheduleApiResult>(e.Message);
            }
        }

        public void SetActionProperty<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<RescheduleRequestOptions, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, RescheduleRequestOptions>(propertyExpression);
            PropertyHelper.SetProperty(Options, convertedExpression, value);
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
        /// Reschedule message
        /// </summary>
        /// <returns>RescheduleResult</returns>
        [ComVisible(false)]
        private RescheduleApiResult Submit()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<RescheduleApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<RescheduleApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<RescheduleApiResult>("Empty Message ID: Please specify MessageID");
            }
            if (Options.SendTime.Equals(""))
            {
                return ResultHelper.RespondError<RescheduleApiResult>("Empty Message ID: Please specify SendTime");
            }
            return SendXML();
        }

        /// <summary>
        /// Reschedule message
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <param name="sendTime">Scheduled for</param>
        /// <returns>RescheduleResult</returns>
        public RescheduleApiResult Submit(string messageID, DateTime sendTime)
        {
            Options = new RescheduleRequestOptions
            {
                MessageID = messageID,
                SendTime = sendTime
            };

            return Submit();
        }

        /// <summary>
        /// Reschedule message using options
        /// </summary>
        /// <param name="options">RescheduleOptions</param>
        /// <returns>RescheduleResult</returns>
        [ComVisible(false)]
        public RescheduleApiResult Submit(RescheduleRequestOptions options)
        {
            Options = Mapper.Map(Options, options);

            return Submit();
        }
        #endregion Submit

        #region SubmitAsync
        /// <summary>
        /// Reschedule message (async)
        /// </summary>
        /// <returns>Task<RescheduleResult></returns>
        [ComVisible(false)]
        public async Task<RescheduleApiResult> SubmitAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<RescheduleApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<RescheduleApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<RescheduleApiResult>("Empty Message ID: Please specify MessageID");
            }
            if (Options.SendTime.Equals(""))
            {
                return ResultHelper.RespondError<RescheduleApiResult>("Empty Message ID: Please specify SendTime");
            }

            return await SendXMLAsync();
        }

        /// <summary>
        /// Reschedule message (async)
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <param name="sendTime">DateTime</param>
        /// <returns>Task<RescheduleResult></returns>
        [ComVisible(false)]
        public async Task<RescheduleApiResult> SubmitAsync(string messageID, DateTime sendTime)
        {
            Options = new RescheduleRequestOptions
            {
                MessageID = messageID,
                SendTime = sendTime
            };

            return await SubmitAsync();
        }

        /// <summary>
        /// Reschedule message using options (async)
        /// </summary>
        /// <param name="options">RescheduleOptions</param>
        /// <returns>Task<RescheduleResult></returns>
        [ComVisible(false)]
        public async Task<RescheduleApiResult> SubmitAsync(RescheduleRequestOptions options)
        {
            Options = Mapper.Map(Options, options);

            return await SubmitAsync();
        }
        #endregion Submit
    }
}
