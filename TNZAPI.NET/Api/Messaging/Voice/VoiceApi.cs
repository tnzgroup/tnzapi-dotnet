using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.Voice
{
	[ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class VoiceApi : IVoiceApi
    {
        private ITNZAuth User = new TNZApiUser();

        public VoiceModel Entity { get; set; } = new VoiceModel();

        /// <summary>
        /// Initiates Voice message to send through TNZAPI
        /// </summary>
        /// <returns></returns>
        public VoiceApi()
        {
        }

        /// <summary>
        /// Initiates Voice message to send through TNZAPI
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        /// <returns></returns>
        public VoiceApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Initiates Voice message to send through TNZAPI
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public VoiceApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Initiates Voice message to send through TNZAPI
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public VoiceApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Initiates Voice message to send through TNZAPI
        /// </summary>
        /// <param name="auth">IAuth</param>
        public VoiceApi(ITNZAuth auth)
        {
            User = auth;
        }

        public void SetSendMode(SendModeType mode)
        {
            Entity.SendMode = mode;
        }

        public void SetMessageProperty<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<VoiceModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, VoiceModel>(propertyExpression);
            PropertyHelper.SetProperty(Entity, convertedExpression, value);
        }

        private XmlDocument BuildXmlDocument()
        {
            #region XML Sample
            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <VoiceRequest>
                <Sender>application@domain.com</Sender>
                <APIKey>ta8wr7ymd</APIKey>
                <MessageType>Voice</MessageType>
                <APIVersion>1.02</APIVersion>
                <MessageID>ID123456</MessageID>
                <MessageData>
                    <Reference>Test1</Reference>
                    <SendTime></SendTime>
                    <TimeZone>New Zealand</TimeZone>
                    <SubAccount>SubAccount01</SubAccount>
                    <Department>Department01</Department>
                    <ChargeCode>BillingGroup01</ChargeCode>
                    <MessageToPeople>[base64 encoded WAV audio data, 16-bit, 8000hz]</MessageToPeople>
                    <MessageToAnswerphones>[base64 encoded WAV audio data, 16-bit, 8000hz]</MessageToAnswerphones>
                    <Keypads>
                        <Tone>1</Tone>
                        <RouteNumber>64800123123</RouteNumber>
                    </Keypads>
                    <Keypads>
                        <Tone>2</Tone>
                        <RouteNumber>6498008000</RouteNumber>
                    </Keypads>
                    <CallRouteMessageToPeople>[base64 encoded WAV audio data, 16-bit, 8000hz]</CallRouteMessageToPeople>
                    <CallRouteMessageToOperators>[base64 encoded WAV audio data, 16-bit, 8000hz]</CallRouteMessageToOperators>
                    <CallRouteMessageOnWrongKey>[base64 encoded WAV audio data, 16-bit, 8000hz]</CallRouteMessageOnWrongKey>
                    <NumberOfOperators>5</NumberOfOperators>
                    <CallerID>6495005000</CallerID>
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
            </VoiceRequest>     
*/
            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("VoiceRequest");
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

            // MessageToPeople, MessageToAnswerphones, CallRouteMessageToPeople, CallRouteMessageToOperators, CallRouteMessageOnWrongKey
            foreach (var attachment in Entity.MessageDataAttachments)
            {
                messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, attachment.Key.ToString(), attachment.Value.FileContent));
            }

            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "CallerID", Entity.CallerID));
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

                if (Entity.KeypadOptionRequired)
                {
                    messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "KeypadOptionRequired", "1"));
                }
            }

            // Set Destinations
            messageDataNode.AppendChild(XMLHelpers.BuildXmlDestinationsNode(xmlDoc, Entity.Recipients, "Voice"));

            //
            // Set MessageData into root node
            //

            rootNode.AppendChild(messageDataNode);

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/send/voice";
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
        /// Send Voice Message
        /// </summary>
        /// <returns>MessageResult</returns>
        public MessageApiResult SendMessage()
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
            // TODO: Need to proof if the logic is right
            if ((Entity.MessageData.TryGetValue(MessageDataType.MessageToPeople, out var value) && !string.IsNullOrEmpty(value)) == false)
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty message to people: Please specify MessageToPeople wave file");
            }

            foreach (var data in Entity.MessageData)
            {
                if (data.Value is null)
                {
                    continue;
                }
                if (!File.Exists(data.Value))
                {
                    return ResultHelper.RespondError<MessageApiResult>($"Unable to file attachment: Could not find the file '{data.Value}'");
                }

                using (var attachment = Attachment.GetAttachment(data.Value))
                {
                    if (Entity.MessageDataAttachments.ContainsKey(data.Key))
                    {
                        Entity.MessageDataAttachments[data.Key] = Mapper.Update(new Attachment(), attachment);
                    }
                    else
                    {
                        Entity.MessageDataAttachments.Add(data.Key, Mapper.Update(new Attachment(), attachment));
                    }

                }
            }

            if (Entity.Keypads.Count > 0)
            {
                foreach (Keypad keypad in Entity.Keypads)
                {
                    if (keypad.RouteNumber.Equals("") && keypad.PlayFile.Equals("") && keypad.PlayFileData is null && keypad.PlaySection == KeypadPlaySection.None)
                    {
                        return ResultHelper.RespondError<MessageApiResult>("Empty Keypad " + keypad.Tone + " Data: Please specify RouteNumber OR PlayFile OR PlayFileData OR PlaySection.");
                    }
                }
            }

            return SendXML();
        }

        /// <summary>
        /// Send Voice Message
        /// </summary>
        /// <param name="entity">VoiceModel</param>
        /// <returns>MessageResult</returns>
        [ComVisible(false)]
        public MessageApiResult SendMessage(VoiceModel entity)
        {
            Entity = Mapper.Update(new VoiceModel(), entity);

            return SendMessage();
        }

		/// <summary>
		/// Send Voice Message
		/// </summary>
		/// <param name="messageID">A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
		/// <param name="reference">Tracking ID or message description</param>
		/// <param name="sendTime">Delay sending until the specified date/time (your local timezone, specified by your Sender setting or overridden using the Timezone)</param>
		/// <param name="timezone">Timezone specified using Windows timezone value (default set using Web Dashboard can be overridden here)</param>
		/// <param name="subaccount">Used for reporting, billing and Web Dashboard segmentation</param>
		/// <param name="department">Used for reporting, billing and Web Dashboard segmentation</param>
		/// <param name="chargeCode">Cost allocation for billing</param>
		/// <param name="numberOfOperators">Limits the maximum simultaneous calls (where multiple 'Destinations' are listed)</param>
		/// <param name="retryAttempts">Number of retries (retry_period required)</param>
		/// <param name="retryPeriod">Minutes between retries (retry_attempts required)</param>
		/// <param name="messageToPeople">The recorded message content played if the call is answered by a human</param>
		/// <param name="messageToAnswerPhones">The recorded message content played when the call is answered by an answering machine/voicemail service</param>
		/// <param name="callRouteMessageToPeople">recorded message content message played when a keypad option is pressed</param>
		/// <param name="callRouteMessageToOperators">recorded message content message played to the call centre representative answering the connected call</param>
		/// <param name="callRouteMessageOnWrongKey">recorded message content message played when an unregistered keypad button is pressed</param>
		/// <param name="callerID">Sets the Caller ID used on the call (must be E.164 format)</param>
		/// <param name="options">Customisable field</param>
		/// <param name="keypads">Keypads - ICollection<Keypad>() object</param>
		/// <param name="groupCode">Sets the recipient group by group code (from TNZ Addressbook)</param>
		/// <param name="groupCodes">Sets the list of recipient groups by list of group codes (from TNZ Addressbook)</param>
		/// <param name="GroupCode">GroupCode object, Sets the recipient group by group code (from TNZ Addressbook)</param>
		/// <param name="GroupCodes">List of GroupCode objects, Sets the list of recipient groups by list of group codes (from TNZ Addressbook)</param>
		/// <param name="groupID">Sets the recipient group by group id (from TNZ Addressbook)</param>
		/// <param name="groupIDs">Sets the list of recipient groups by list of group ids (from TNZ Addressbook)</param>
		/// <param name="groupID">GroupID object, Sets the recipient group by group id (from TNZ Addressbook)</param>
		/// <param name="groupIDs">List of GroupID objects, Sets the list of recipient groups by list of group ids (from TNZ Addressbook)</param>
		/// <param name="contactID">Sets the recipient by contact id (from TNZ Addressbook)</param>
		/// <param name="contactIDs">Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
		/// <param name="contactID">ContactID object, Sets the recipient by contact id (from TNZ Addressbook)</param>
		/// <param name="contactIDs">List of ContactID objects, Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
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
			MessageID messageID = null,                     // MessageID object
			string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string messageToPeople = null,
            string messageToAnswerPhones = null,
            string callRouteMessageToPeople = null,
            string callRouteMessageToOperators = null,
            string callRouteMessageOnWrongKey = null,
            int? numberOfOperators = null,
			int? retryAttempts = null,
			int? retryPeriod = null,
			string callerID = null,
            string options = null,
            ICollection<Keypad> keypads = null,
			GroupID groupID = null,                         // GroupID object
			ICollection<GroupID> groupIDs = null,           // ICollection<GroupID>
			ContactID contactID = null,                     // ContactID object
			ICollection<ContactID> contactIDs = null,       // ICollection<ContactID>
			string destination = null,
            ICollection<string> destinations = null,
            Recipient recipient = null,
            ICollection<Recipient> recipients = null,
            string webhookCallbackURL = null,
            WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        )
        {
            return SendMessage(new VoiceModel()
            {
				MessageID = messageID,

				WebhookCallbackURL = webhookCallbackURL,
                WebhookCallbackFormat = webhookCallbackFormat is not null ? (WebhookCallbackType)webhookCallbackFormat : WebhookCallbackType.JSON,

                Reference = reference,
                SendTime = sendTime is not null ? (DateTime)sendTime : DateTime.Now,
                Timezone = timezone,
                SubAccount = subaccount,
                Department = department,
                ChargeCode = chargeCode,

                NumberOfOperators = numberOfOperators is not null ? (int)numberOfOperators : 0,
				RetryAttempts = retryAttempts is not null ? (int)retryAttempts : 0,
				RetryPeriod = retryPeriod is not null ? (int)retryPeriod : 1,

				CallerID = callerID,
                Options = options,

                MessageData = new Dictionary<MessageDataType, string>()
                {
                    { MessageDataType.MessageToPeople, messageToPeople },
                    { MessageDataType.MessageToAnswerPhones, messageToAnswerPhones },
                    { MessageDataType.CallRouteMessageToPeople, callRouteMessageToPeople },
                    { MessageDataType.CallRouteMessageToOperators, callRouteMessageToOperators },
                    { MessageDataType.CallRouteMessageOnWrongKey, callRouteMessageOnWrongKey }
                },

                Keypads = new KeypadList()
                            .Add(keypads)
                            .ToList(),

                Recipients = new RecipientList()
							.Add(groupID)
							.Add(groupIDs)
							.Add(contactID)
							.Add(contactIDs)
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
        /// Send Voice Message Async
        /// </summary>
        /// <returns>Task<MessageResult></returns>
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
            // TODO: Need to proof if the logic is right
            if ((Entity.MessageData.TryGetValue(MessageDataType.MessageToPeople, out var value) && !string.IsNullOrEmpty(value)) == false)
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty message to people: Please specify MessageToPeople wave file");
            }

            foreach (var data in Entity.MessageData)
            {
                using (var attachment = await Attachment.GetAttachmentAsync(data.Value))
                {
                    Entity.MessageDataAttachments.Add(data.Key, attachment);
                }
            }

            return await SendXMLAsync();
        }

        /// <summary>
        /// Send Voice Message Async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Task<MessageResult></returns>
        [ComVisible(false)]
        public async Task<MessageApiResult> SendMessageAsync(VoiceModel entity)
        {
            Entity = Mapper.Update(new VoiceModel(), entity);

            return await SendMessageAsync();
        }

		/// <summary>
		/// Send Voice Message
		/// </summary>
		/// <param name="messageID">A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
		/// <param name="reference">Tracking ID or message description</param>
		/// <param name="sendTime">Delay sending until the specified date/time (your local timezone, specified by your Sender setting or overridden using the Timezone)</param>
		/// <param name="timezone">Timezone specified using Windows timezone value (default set using Web Dashboard can be overridden here)</param>
		/// <param name="subaccount">Used for reporting, billing and Web Dashboard segmentation</param>
		/// <param name="department">Used for reporting, billing and Web Dashboard segmentation</param>
		/// <param name="chargeCode">Cost allocation for billing</param>
		/// <param name="numberOfOperators">Limits the maximum simultaneous calls (where multiple 'Destinations' are listed)</param>
		/// <param name="retryAttempts">Number of retries (retry_period required)</param>
		/// <param name="retryPeriod">Minutes between retries (retry_attempts required)</param>
		/// <param name="messageToPeople">The recorded message content played if the call is answered by a human</param>
		/// <param name="messageToAnswerPhones">The recorded message content played when the call is answered by an answering machine/voicemail service</param>
		/// <param name="callRouteMessageToPeople">recorded message content message played when a keypad option is pressed</param>
		/// <param name="callRouteMessageToOperators">recorded message content message played to the call centre representative answering the connected call</param>
		/// <param name="callRouteMessageOnWrongKey">recorded message content message played when an unregistered keypad button is pressed</param>
		/// <param name="callerID">Sets the Caller ID used on the call (must be E.164 format)</param>
		/// <param name="options">Customisable field</param>
		/// <param name="keypads">Keypads - ICollection<Keypad>() object</param>
		/// <param name="groupID">GroupID object, Sets the recipient group by group id (from TNZ Addressbook)</param>
		/// <param name="groupIDs">List of GroupID objects, Sets the list of recipient groups by list of group ids (from TNZ Addressbook)</param>
		/// <param name="contactID">ContactID object, Sets the recipient by contact id (from TNZ Addressbook)</param>
		/// <param name="contactIDs">List of ContactID objects, Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
		/// <param name="destination">Destination - string value</param>
		/// <param name="destinations">Desitnations - ICollection<string>()</param>
		/// <param name="recipient">Destination - Recipient() object</param>
		/// <param name="recipients">Destinations - ICollection<Recipient>()</param>
		/// <param name="webhookCallbackURL">Webhook Callback URL</param>
		/// <param name="webhookCallbackFormat">Webhook Callback Format (XML/JSON)</param>
		/// <param name="sendMode">SendMode.Live or SendMode.Test</param>
		/// <returns>MessageApiResult</returns>
		public async Task<MessageApiResult> SendMessageAsync(
			MessageID messageID = null,                     // MessageID object
			string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string messageToPeople = null,
            string messageToAnswerPhones = null,
            string callRouteMessageToPeople = null,
            string callRouteMessageToOperators = null,
            string callRouteMessageOnWrongKey = null,
            int? numberOfOperators = null,
			int? retryAttempts = null,
			int? retryPeriod = null,
			string callerID = null,
            string options = null,
            ICollection<Keypad> keypads = null,
			GroupID groupID = null,                         // GroupID object
			ICollection<GroupID> groupIDs = null,           // ICollection<GroupID>
			ContactID contactID = null,                     // ContactID object
			ICollection<ContactID> contactIDs = null,       // ICollection<ContactID>
			string destination = null,
            ICollection<string> destinations = null,
            Recipient recipient = null,
            ICollection<Recipient> recipients = null,
            string webhookCallbackURL = null,
            WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        )
        {
            return await SendMessageAsync(new VoiceModel()
            {
				MessageID = messageID ?? new MessageID(messageID),

				WebhookCallbackURL = webhookCallbackURL,
                WebhookCallbackFormat = webhookCallbackFormat is not null ? (WebhookCallbackType)webhookCallbackFormat : WebhookCallbackType.JSON,

                Reference = reference,
                SendTime = sendTime is not null ? (DateTime)sendTime : DateTime.Now,
                Timezone = timezone,
                SubAccount = subaccount,
                Department = department,
                ChargeCode = chargeCode,

                NumberOfOperators = numberOfOperators is not null ? (int)numberOfOperators : 0,
				RetryAttempts = retryAttempts is not null ? (int)retryAttempts : 0,
				RetryPeriod = retryPeriod is not null ? (int)retryPeriod : 1,

				CallerID = callerID,
                Options = options,

                MessageData = new Dictionary<MessageDataType, string>()
                {
                    { MessageDataType.MessageToPeople, messageToPeople },
                    { MessageDataType.MessageToAnswerPhones, messageToAnswerPhones },
                    { MessageDataType.CallRouteMessageToPeople, callRouteMessageToPeople },
                    { MessageDataType.CallRouteMessageToOperators, callRouteMessageToOperators },
                    { MessageDataType.CallRouteMessageOnWrongKey, callRouteMessageOnWrongKey }
                },

                Keypads = await new KeypadList()
                            .Add(keypads)
                            .ToListAsync(),

                Recipients = new RecipientList()
							.Add(groupID)
							.Add(groupIDs)
							.Add(contactID)
							.Add(contactIDs)
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
