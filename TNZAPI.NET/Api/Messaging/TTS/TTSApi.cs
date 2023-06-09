﻿using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Xml;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using static TNZAPI.NET.Api.Messaging.Common.Enums;
using static TNZAPI.NET.Api.Messaging.TTS.Dto.TTSModel;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.TTS
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class TTSApi : ITTSApi
    {
        private ITNZAuth User = new TNZApiUser();

        private TTSModel Entity { get; set; } = new TTSModel();

        /// <summary>
        /// Initiates Text-To-Speech message to send through TNZAPI
        /// </summary>
        /// <returns></returns>
        public TTSApi()
        {
        }

        /// <summary>
        /// Initiates Text-To-Speech message to send through TNZAPI
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        /// <returns></returns>
        public TTSApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Initiates Text-To-Speech message to send through TNZAPI
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public TTSApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Initiates Text-To-Speech message to send through TNZAPI
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public TTSApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Initiates Text-To-Speech message to send through TNZAPI
        /// </summary>
        /// <param name="auth">IAuth</param>
        public TTSApi(ITNZAuth auth)
        {
            User = auth;
        }

        public void SetSendMode(SendModeType mode)
        {
            Entity.SendMode = mode;
        }

        public void SetMessageProperty<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<TTSModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, TTSModel>(propertyExpression);
            PropertyHelper.SetProperty(Entity, convertedExpression, value);
        }

        private XmlDocument BuildXmlDocument()
        {
            #region XML Sample
            /*
						<?xml version="1.0" encoding="UTF-8"?>
						<TTSRequest>
							<Sender>application@domain.com</Sender>
							<APIKey>ta8wr7ymd</APIKey>
							<MessageType>TextToSpeech</MessageType>
							<APIVersion>1.02</APIVersion>
							<MessageID>ID123456</MessageID>
							<MessageData>
								<Reference>Test1</Reference>
								<SendTime></SendTime>
								<TimeZone>New Zealand</TimeZone>
								<SubAccount>SubAccount01</SubAccount>
								<Department>Department01</Department>
								<ChargeCode>BillingGroup01</ChargeCode>
								<MessageToPeople>Hello, this is a call from Department01. This is relevant information. Press one to be connected to our call centre.</MessageToPeople>
								<MessageToAnswerphones>Hello, sorry we missed you. This is a call from Department 01. Please contact us on 0800 123123.</MessageToAnswerphones>
								<Keypads>
									<Tone>1</Tone>
									<RouteNumber>64800123123</RouteNumber>
								</Keypads>
								<Keypads>
									<Tone>2</Tone>
									<RouteNumber>6498008000</RouteNumber>
								</Keypads>
								<CallRouteMessageToPeople>Connecting you now.</CallRouteMessageToPeople>
								<CallRouteMessageToOperators>Incoming Text To Speech call.</CallRouteMessageToOperators>
								<CallRouteMessageOnWrongKey>Sorry, you have pressed an invalid key. Please try again.</CallRouteMessageOnWrongKey>
								<NumberOfOperators>5</NumberOfOperators>
								<CallerID>6495005000</CallerID>
								<Voice>Female2</Voice>
								<Options></Options>
								<Destinations>
                  <Destination>
										<Recipient>6421000001</Recipient>
										<Custom1></Custom1>
										<Custom2></Custom2>
                  </Destination>
								</Destinations>
								<Recipients>6495005002</Recipients>
								<Recipients>6421000003</Recipients>
							</MessageData>
						</TTSRequest>  
            */
            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("TTSRequest");
            xmlDoc.AppendChild(rootNode);

            if (User.AuthToken.Equals(""))
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Sender", User.Sender));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "APIKey", User.APIKey));
            }

            XmlNode workingNode = xmlDoc.CreateElement("Sender");

            //
            // MessageData
            //
            XmlNode messageDataNode = xmlDoc.CreateElement("MessageData");

            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessageID", Entity.MessageID));
            if (Entity.SendMode == SendModeType.Test)
            {
                messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Mode", "Test"));
            }
            if (Entity.WebhookCallbackURL != "")
            {
                messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "WebhookCallbackURL", Entity.WebhookCallbackURL));

                if (Entity.WebhookCallbackFormat == WebhookCallbackType.JSON)
                {
                    messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "WebhookCallbackFormat", "JSON"));
                }
                if (Entity.WebhookCallbackFormat == WebhookCallbackType.XML)
                {
                    messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "WebhookCallbackFormat", "XML"));
                }
            }
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Reference", Entity.Reference));
            if (DateTime.Compare(Entity.SendTime, DateTime.Now) > 0) // send if future date
            {
                messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "SendTime", Entity.SendTime.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "TimeZone", Entity.Timezone));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "SubAccount", Entity.SubAccount));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Department", Entity.Department));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ChargeCode", Entity.ChargeCode));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessageToPeople", Entity.MessageToPeople));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessageToAnswerphones", Entity.MessageToPeople));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "CallRouteMessageToPeople", Entity.CallRouteMessageToPeople));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "CallRouteMessageToOperators", Entity.CallRouteMessageToOperators));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "CallRouteMessageOnWrongKey", Entity.CallRouteMessageOnWrongKey));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "NumberOfOperators", Entity.NumberOfOperators.ToString()));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "CallerID", Entity.CallerID));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Voice", Enum.GetName(typeof(TTSVoiceType), Entity.TTSVoice)));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Options", Entity.Options));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "NumberOfOperators", Entity.NumberOfOperators.ToString()));
            if (Entity.RetryAttempts > 0)
            {
                messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "RetryAttempts", Entity.RetryAttempts.ToString()));
                messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "RetryPeriod", Entity.RetryPeriod.ToString()));
            }
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ReportTo", Entity.ReportTo.ToString()));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ErrorEmailNotify", Entity.ErrorEmailNotify));

            // Set Keypads
            if (Entity.Keypads.Count > 0)
            {
                messageDataNode.AppendChild(XMLHelpers.BuildXmlKeypadsNode(xmlDoc, Entity.Keypads));
            }

            // Set Destinations
            messageDataNode.AppendChild(XMLHelpers.BuildXmlDestinationsNode(xmlDoc, Entity.Recipients, "TTS"));

            //
            // Set MessageData into root node
            //

            rootNode.AppendChild(messageDataNode);

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/send/tts";
        }

        //
        // Synchronous function for backward compatibility
        //
        private MessageApiResult SendXML()
        {
            try
            { 
                return Task.Run(() => HttpRequest.PostXMLAsync<MessageApiResult>(
                    new PostHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<MessageApiResult>(e.Message);
            }
        }

        // POST XML to TNZ REST API
        private async Task<MessageApiResult> SendXMLAsync()
        {
            try
            {
                return await HttpRequest.PostXMLAsync<MessageApiResult>(
                    new PostHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<MessageApiResult>(e.Message);
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

        #region SendMessage
        /// <summary>
        /// Send TextToSpeech Message
        /// </summary>
        /// <returns>MessageResult</returns>
        private MessageApiResult SendMessage()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<MessageApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<MessageApiResult>("Empty API key: Please specify APIKey");
                }
            }
            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty Enitity: Please set options");
            }
            if (Entity.Recipients.Count == 0)
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty recipient(s): Please add Recipient");
            }
            if (Entity.MessageToPeople.Equals(""))
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty message to people: Please specify MessageToPeople");
            }
            if (Entity.Keypads.Count > 0)
            {
                foreach (Keypad keypad in Entity.Keypads)
                {
                    if (keypad.RouteNumber.Equals("") && keypad.Play.Equals(""))
                    {
                        return ResultHelper.RespondError<MessageApiResult>("Empty Keypad " + keypad.Tone + " Data: Please specify RouteNumber OR Play");
                    }
                }
            }
            return SendXML();
        }

        /// <summary>
        /// Send TextToSpeech Message
        /// </summary>
        /// <param name="entity">TTSModel</param>
        /// <returns>MessageResult</returns>
        [ComVisible(false)]
        public MessageApiResult SendMessage(TTSModel entity)
        {
            Entity = Mapper.Update(new TTSModel(), entity);

            return SendMessage();
        }

        /// <summary>
        /// Send TextToSpeech Message
        /// </summary>
        /// <param name="messageID">A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
        /// <param name="reference">Tracking ID or message description</param>
        /// <param name="sendTime">Delay sending until the specified date/time (your local timezone, specified by your Sender setting or overridden using the Timezone)</param>
        /// <param name="timezone">Timezone specified using Windows timezone value (default set using Web Dashboard can be overridden here)</param>
        /// <param name="subaccount">Used for reporting, billing and Web Dashboard segmentation</param>
        /// <param name="department">Used for reporting, billing and Web Dashboard segmentation</param>
        /// <param name="chargeCode">Cost allocation for billing</param>
        /// <param name="messageToPeople">The voice file be played if the call is answered by a human</param>
        /// <param name="messageToAnswerphones">The voice file be played when the call is answered by an answering machine/voicemail service</param>
        /// <param name="callRouteMessageToPeople">The voice file be played when a keypad option is pressed</param>
        /// <param name="callRouteMessageToOperators">Text-to-speech message played to the call centre representative answering the connected call</param>
        /// <param name="callRouteMessageOnWrongKey">Text-to-speech message played when an unregistered keypad button is pressed</param>
        /// <param name="numberOfOperators">Limits the maximum simultaneous calls (where multiple 'Destinations' are listed)</param>
        /// <param name="callerID">Sets the Caller ID used on the call (must be E.164 format)</param>
        /// <param name="ttsVoiceType">Text-to-Speech voice to use (Male1, Female1, Female2, Female3, Female4)</param>
        /// <param name="options">Customisable field</param>
        /// <param name="keypads">Keypads - ICollection<Keypad>() object</param>
        /// <param name="destination">Destination - string value</param>
        /// <param name="destinations">Desitnations - ICollection<string>()</param>
        /// <param name="recipient">Destination - Recipient() object</param>
        /// <param name="recipients">Destinations - ICollection<Recipient>()</param>
        /// <param name="webhookCallbackURL">Webhook Callback URL</param>
        /// <param name="webhookCallbackFormat">Webhook Callback Format (XML/JSON)</param>
        /// <param name="sendMode">SendMode.Live or SendMode.Test</param>
        /// <returns>MessageApiResult</returns>
        [ComVisible(false)]
        public MessageApiResult SendMessage(
            string messageID = null,
            string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string messageToPeople = null,
            string messageToAnswerphones = null,
            string callRouteMessageToPeople = null,
            string callRouteMessageToOperators = null,
            string callRouteMessageOnWrongKey = null,
            int? numberOfOperators = null,
            string callerID = null,
            TTSVoiceType? ttsVoiceType = null,
            string options = null,
            ICollection<Keypad> keypads = null,
            string destination = null,
            ICollection<string> destinations = null,
            Recipient recipient = null,
            ICollection<Recipient> recipients = null,
            string webhookCallbackURL = null,
            WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        )
        {
            return SendMessage(new TTSModel()
            {
                WebhookCallbackURL = webhookCallbackURL,
                WebhookCallbackFormat = webhookCallbackFormat is not null ? (WebhookCallbackType)webhookCallbackFormat : WebhookCallbackType.JSON,

                MessageID = messageID,
                Reference = reference,
                SendTime = sendTime is not null ? (DateTime)sendTime : DateTime.Now,
                Timezone = timezone,
                SubAccount = subaccount,
                Department = department,
                ChargeCode = chargeCode,

                NumberOfOperators = numberOfOperators is not null ? (int)numberOfOperators : 0,
                CallerID = callerID,
                TTSVoice = ttsVoiceType is not null ? (TTSVoiceType)ttsVoiceType : TTSVoiceType.Female1,
                Options = options,

                MessageToPeople = messageToPeople,
                MessageToAnswerPhones = messageToAnswerphones,
                CallRouteMessageToPeople = callRouteMessageToPeople,
                CallRouteMessageToOperators = callRouteMessageToOperators,
                CallRouteMessageOnWrongKey = callRouteMessageOnWrongKey,

                Keypads = new KeypadList()
                            .Add(keypads)
                            .ToList(),

                Recipients = new RecipientList()
                            .Add(destination)
                            .Add(destinations)
                            .Add(recipient)
                            .Add(recipients)
                            .ToList(),

                SendMode = sendMode is not null ? (SendModeType)sendMode : SendModeType.Live
            });
        }
        #endregion SendMessage

        #region SendMessageAsync
        /// <summary>
        /// Send TextToSpeech Message Async
        /// </summary>
        /// <returns>IMessageResult</returns>

        [ComVisible(false)]
        public async Task<MessageApiResult> SendMessageAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<MessageApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<MessageApiResult>("Empty API key: Please specify APIKey");
                }
            }

            if (Entity is null || PropertyHelper.IsNewObject(Entity))
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty Enitity: Please set options");
            }
            if (Entity.Recipients.Count == 0)
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty recipient(s): Please add Recipient");
            }
            if (Entity.MessageToPeople.Equals(""))
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty message to people: Please specify MessageToPeople");
            }
            if (Entity.Keypads.Count > 0)
            {
                foreach (Keypad keypad in Entity.Keypads)
                {
                    if (keypad.RouteNumber.Equals("") && keypad.Play.Equals(""))
                    {
                        return ResultHelper.RespondError<MessageApiResult>("Empty Keypad " + keypad.Tone + " Data: Please specify RouteNumber OR Play");
                    }
                }
            }
            return await SendXMLAsync();
        }

        /// <summary>
        /// Send TextToSpeech Message
        /// </summary>
        /// <param name="entity">TTSModel</param>
        /// <returns>Task<MessageResult></returns>
        [ComVisible(false)]
        public async Task<MessageApiResult> SendMessageAsync(TTSModel entity)
        {
            Entity = Mapper.Update(new TTSModel(), entity);

            return await SendMessageAsync();
        }

        /// <summary>
        /// Send TextToSpeech Message
        /// </summary>
        /// <param name="messageID">A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
        /// <param name="reference">Tracking ID or message description</param>
        /// <param name="sendTime">Delay sending until the specified date/time (your local timezone, specified by your Sender setting or overridden using the Timezone)</param>
        /// <param name="timezone">Timezone specified using Windows timezone value (default set using Web Dashboard can be overridden here)</param>
        /// <param name="subaccount">Used for reporting, billing and Web Dashboard segmentation</param>
        /// <param name="department">Used for reporting, billing and Web Dashboard segmentation</param>
        /// <param name="chargeCode">Cost allocation for billing</param>
        /// <param name="messageToPeople">The voice file be played if the call is answered by a human</param>
        /// <param name="messageToAnswerphones">The voice file be played when the call is answered by an answering machine/voicemail service</param>
        /// <param name="callRouteMessageToPeople">The voice file be played when a keypad option is pressed</param>
        /// <param name="callRouteMessageToOperators">Text-to-speech message played to the call centre representative answering the connected call</param>
        /// <param name="callRouteMessageOnWrongKey">Text-to-speech message played when an unregistered keypad button is pressed</param>
        /// <param name="numberOfOperators">Limits the maximum simultaneous calls (where multiple 'Destinations' are listed)</param>
        /// <param name="callerID">Sets the Caller ID used on the call (must be E.164 format)</param>
        /// <param name="ttsVoiceType">Text-to-Speech voice to use (Male1, Female1, Female2, Female3, Female4)</param>
        /// <param name="options">Customisable field</param>
        /// <param name="keypads">Keypads - ICollection<Keypad>() object</param>
        /// <param name="destination">Destination - string value</param>
        /// <param name="destinations">Desitnations - ICollection<string>()</param>
        /// <param name="recipient">Destination - Recipient() object</param>
        /// <param name="recipients">Destinations - ICollection<Recipient>()</param>
        /// <param name="webhookCallbackURL">Webhook Callback URL</param>
        /// <param name="webhookCallbackFormat">Webhook Callback Format (XML/JSON)</param>
        /// <param name="sendMode">SendMode.Live or SendMode.Test</param>
        /// <returns>MessageApiResult</returns>
        [ComVisible(false)]
        public async Task<MessageApiResult> SendMessageAsync(
            string messageID = null,
            string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string messageToPeople = null,
            string messageToAnswerphones = null,
            string callRouteMessageToPeople = null,
            string callRouteMessageToOperators = null,
            string callRouteMessageOnWrongKey = null,
            int? numberOfOperators = null,
            string callerID = null,
            TTSVoiceType? ttsVoiceType = null,
            string options = null,
            ICollection<Keypad> keypads = null,
            string destination = null,
            ICollection<string> destinations = null,
            Recipient recipient = null,
            ICollection<Recipient> recipients = null,
            string webhookCallbackURL = null,
            WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        )
        {
            return await SendMessageAsync(new TTSModel()
            {
                WebhookCallbackURL = webhookCallbackURL,
                WebhookCallbackFormat = webhookCallbackFormat is not null ? (WebhookCallbackType)webhookCallbackFormat : WebhookCallbackType.JSON,

                MessageID = messageID,
                Reference = reference,
                SendTime = sendTime is not null ? (DateTime)sendTime : DateTime.Now,
                Timezone = timezone,
                SubAccount = subaccount,
                Department = department,
                ChargeCode = chargeCode,

                NumberOfOperators = numberOfOperators is not null ? (int)numberOfOperators : 0,
                CallerID = callerID,
                TTSVoice = ttsVoiceType is not null ? (TTSVoiceType)ttsVoiceType : TTSVoiceType.Female1,
                Options = options,

                MessageToPeople = messageToPeople,
                MessageToAnswerPhones = messageToAnswerphones,
                CallRouteMessageToPeople = callRouteMessageToPeople,
                CallRouteMessageToOperators = callRouteMessageToOperators,
                CallRouteMessageOnWrongKey = callRouteMessageOnWrongKey,

                Keypads = await new KeypadList()
                            .Add(keypads)
                            .ToListAsync(),

                Recipients = new RecipientList()
                            .Add(destination)
                            .Add(destinations)
                            .Add(recipient)
                            .Add(recipients)
                            .ToList(),

                SendMode = sendMode is not null ? (SendModeType)sendMode : SendModeType.Live
            });
        }
        #endregion
    }
}
