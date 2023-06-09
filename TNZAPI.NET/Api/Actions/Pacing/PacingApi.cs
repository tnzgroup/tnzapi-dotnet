using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Actions.Pacing.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Actions.Pacing
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class PacingApi : IPacingApi
    {
        private ITNZAuth User = new TNZApiUser();

        private PacingRequestOptions Options { get; set; } = new PacingRequestOptions();

        /// <summary>
        /// Change NumberOfOperators through TNZAPI
        /// </summary>
        /// <returns></returns>
        public PacingApi()
        {
        }

        /// <summary>
        /// Change NumberOfOperators through TNZAPI
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        /// <returns></returns>
        public PacingApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Change NumberOfOperators through TNZAPI
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public PacingApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Change NumberOfOperators through TNZAPI
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public PacingApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Change NumberOfOperators through TNZAPI
        /// </summary>
        /// <param name="auth">IAuth</param>
        public PacingApi(ITNZAuth auth)
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
  <Type>Pacing</Type>
  <APIVersion>1.04</APIVersion>
  <MessageID>ID123456</MessageID>
  <Pacing>1</Pacing>
</root>
*/
            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("PacingRequest");
            xmlDoc.AppendChild(rootNode);

            if (User.AuthToken.Equals(""))
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Sender", User.Sender));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "APIKey", User.APIKey));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessageID", Options.MessageID));
            }

            rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "NumberOfOperators", Options.NumberOfOperators.ToString()));

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/set/pacing");

            if (User.AuthToken.Equals("") == false)
            {
                requestUri.Append($"/{Options.MessageID}");
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private PacingApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.PatchXMLAsync<PacingApiResult>(
                    new PatchHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<PacingApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<PacingApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.PatchXMLAsync<PacingApiResult>(
                    new PatchHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<PacingApiResult>(e.Message);
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
        /// Submit Pacing (TTS/Voice only)
        /// </summary>
        /// <returns>PacingResult</returns>
        [ComVisible(false)]
        private PacingApiResult Submit()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<PacingApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<PacingApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<PacingApiResult>("Empty Message ID: Please specify MessageID");
            }
            if (Options.NumberOfOperators.Equals(""))
            {
                return ResultHelper.RespondError<PacingApiResult>("Empty Message ID: Please specify SendTime");
            }
            return SendXML();
        }

        /// <summary>
        /// Set Pacing (TTS/Voice only)
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="numberOfOperators"></param>
        /// <returns>PacingResult</returns>

        public PacingApiResult Submit(string messageID, int numberOfOperators)
        {
            Options = new PacingRequestOptions
            {
                MessageID = messageID,
                NumberOfOperators = numberOfOperators
            };

            return Submit();
        }

        /// <summary>
        /// Set Pacing with Pacing Options (TTS/Voice only)
        /// </summary>
        /// <param name="options">PacingOptions</param>
        /// <returns>PacingResult</returns>
        [ComVisible(false)]
        public PacingApiResult Submit(PacingRequestOptions options)
        {
            Options = Mapper.Map(Options, options);

            return Submit();
        }
        #endregion Submit

        #region SubmitAsync
        /// <summary>
        /// Set Pacing Async (TTS/Voice only)
        /// </summary>
        /// <returns>PacingResult</returns>
        [ComVisible(false)]
        public async Task<PacingApiResult> SubmitAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<PacingApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<PacingApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Options.MessageID.Equals(""))
            {
                return ResultHelper.RespondError<PacingApiResult>("Empty Message ID: Please specify MessageID");
            }
            if (Options.NumberOfOperators.Equals(""))
            {
                return ResultHelper.RespondError<PacingApiResult>("Empty Message ID: Please specify SendTime");
            }
            return await SendXMLAsync();
        }

        /// <summary>
        /// Set Pacing Async (TTS/Voice only)
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="numberOfOperators"></param>
        /// <returns></returns>
        [ComVisible(false)]
        public async Task<PacingApiResult> SubmitAsync(string messageID, int numberOfOperators)
        {
            Options = new PacingRequestOptions
            {
                MessageID = messageID,
                NumberOfOperators = numberOfOperators
            };

            return await SubmitAsync();
        }

        /// <summary>
        /// Set Pacing Async (TTS/Voice only)
        /// </summary>
        /// <param name="options">PacingOptions</param>
        /// <returns>Task<PacingResult></returns>
        [ComVisible(false)]
        public async Task<PacingApiResult> SubmitAsync(PacingRequestOptions options)
        {
            Options = Mapper.Map(Options, options);

            return await SubmitAsync();
        }
        #endregion Poll
    }
}
