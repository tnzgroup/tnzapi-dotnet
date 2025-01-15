using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.SMS
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class SMSApi : ISMSApi
    {
        private ITNZAuth User = new TNZApiUser();

        private SMSModel Entity { get; set; } = new SMSModel();

        /// <summary>
        /// Initiates SMS message to send through TNZAPI
        /// </summary>
        /// <returns></returns>
        public SMSApi()
        {
        }

        /// <summary>
        /// Initiates SMS message to send through TNZAPI
        /// </summary>
        /// <param name="authToken">Auth Token for TNZ API</param>
        /// <returns></returns>
        public SMSApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Initiates SMS message to send through TNZAPI
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public SMSApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Initiates SMS message to send through TNZAPI
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public SMSApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Initiates SMS message to send through TNZAPI
        /// </summary>
        /// <param name="auth">IAuth</param>
        public SMSApi(ITNZAuth auth)
        {
            User = auth;
        }

        public void SetSendMode(SendModeType mode)
        {
            Entity.SendMode = mode;
        }

        public void SetMessageProperty<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<SMSModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, SMSModel>(propertyExpression);
            PropertyHelper.SetProperty(Entity, convertedExpression, value);
        }

        private XmlDocument BuildXmlDocument()
        {
            #region XML Sample
            /*
                <?xml version="1.0" encoding="UTF-8"?>
                <root>
                  <Sender>application@domain.com</Sender>
                  <APIKey>ta8wr7ymd</APIKey>
                  <MessageType>SMS</MessageType>
                  <APIVersion>1.00</APIVersion>
                  <MessageData>
                    <Reference>Test1</Reference>
                    <SendTime></SendTime>
                    <TimeZone>New Zealand</TimeZone>
                    <SubAccount>SubAccount01</SubAccount>
                    <Department>Department01</Department>
                    <ChargeCode>BillingGroup01</ChargeCode>
                    <MessageID>js82hn8n</MessageID>
                    <SMSEmailReply>person.one@domain.com</SMSEmailReply>
                    <Message>Hello, this is a test message from Department01. Thank you.</Message>
                    <Destinations>
                        <Recipient>6421000001</Recipient>
                        <Attention>Recipient One</Attention>
                        <Company>Example Corp</Company>
                        <Custom1></Custom1>
                        <Custom2></Custom2>
                    </Destinations>
                    <Destinations>6421000002</Destinations>
                    <Destinations>6421000003</Destinations>
                    <Destinations>6421000004</Destinations>
                  </MessageData>
                </root>
            */
            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("SMSRequest");
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
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "FromNumber", Entity.FromNumber));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "SMSEmailReply", Entity.SMSEmailReply));
						messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ServiceName", Entity.ServiceName));
						messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ReportTo", Entity.ReportTo));
						messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Message", Entity.MessageText));

            // Set Destinations
            messageDataNode.AppendChild(XMLHelpers.BuildXmlDestinationsNode(xmlDoc, Entity.Recipients, "SMS"));

            // Set Attachments
            if (Entity.Attachments.Count > 0)
            {
                messageDataNode.AppendChild(XMLHelpers.BuildXmlFilesNode(xmlDoc, Entity.Attachments));
            }

            //
            // Set MessageData into root node
            //

            rootNode.AppendChild(messageDataNode);

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/send/sms";
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
        /// Send SMS Message
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
            if (Entity.MessageText == null || Entity.MessageText.Equals(""))
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty message_text: Please specify MessageText");
            }
            if (Entity.StripSlashes == false)
            {
                Entity.MessageText = TextUtilities.AddSlashes(Entity.MessageText);
            }

            return SendXML();
        }

        /// <summary>
        /// Send SMS Message
        /// </summary>
        /// <param name="entity">SMSModel</param>
        /// <returns>MessageResult</returns>
        [ComVisible(false)]
        public MessageApiResult SendMessage(SMSModel entity)
        {
            Entity = Mapper.Update(new SMSModel(), entity);

            return SendMessage();
        }

				/// <summary>
				/// Send SMS Message
				/// </summary>
				/// <param name="messageID">A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
				/// <param name="messageID">MessageID object, A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
				/// <param name="reference">Tracking ID or message description</param>
				/// <param name="sendTime">Delay sending until the specified date/time (your local timezone, specified by your Sender setting or overridden using the Timezone)</param>
				/// <param name="timezone">Timezone specified using Windows timezone value (default set using Web Dashboard can be overridden here)</param>
				/// <param name="subaccount">Used for reporting, billing and Web Dashboard segmentation</param>
				/// <param name="department">Used for reporting, billing and Web Dashboard segmentation</param>
				/// <param name="chargeCode">Cost allocation for billing</param>
				/// <param name="serviceName">Service name for your app</param>
				/// <param name="smsEmailReply">For email (SMTP) reply receipt notifications</param>
				/// <param name="forceGSMChars">Convert multi-byte characters into normalised GSM character format. ie. © to (C)</param>
				/// <param name="messageText">Plain or UTF-8 formatted SMS message</param>
				/// <param name="group">GroupModel object, Sets the recipient group by group id (from TNZ Addressbook)</param>
				/// <param name="groups">List of GroupModel objects, Sets the recipient groups by group ids (from TNZ Addressbook)</param>
				/// <param name="groupID">GroupID object, Sets the recipient group by group id (from TNZ Addressbook)</param>
				/// <param name="groupIDs">List of GroupID objects, Sets the list of recipient groups by list of group ids (from TNZ Addressbook)</param>
				/// <param name="contact">ContactModel object, Sets the recipient by contact id (from TNZ Addressbook)</param>
				/// <param name="contacts">List of ContactModel objects, Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
				/// <param name="contactID">ContactID object, Sets the recipient by contact id (from TNZ Addressbook)</param>
				/// <param name="contactIDs">List of ContactID objects, Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
				/// <param name="destination">Sets the SMS destination</param>
				/// <param name="destinations">Sets the list of SMS destinations</param>
				/// <param name="recipient">Sets the SMS recipient - Recipient() object</param>
				/// <param name="recipients">Sets the list of SMS recipients - List<Recipient>()</param>
				/// <param name="file">Sets the attachment (file location)</param>
				/// <param name="files">Sets the list of attachments (file locations)</param>
				/// <param name="attachment">Sets the attachment - Attachment() object</param>
				/// <param name="attachments">Sets the list of attachments</param>
				/// <param name="webhookCallbackURL">Webhook callback URL</param>
				/// <param name="webhookCallbackFormat">Webhool callback format - XML or JSON</param>
				/// <param name="sendMode">SendModeType.Live or SendModeType.Test</param>
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
                string serviceName = null,
                string smsEmailReply = null,
                string forceGSMChars = null,
                string messageText = null,
                GroupModel group = null,                        // GroupModel object
                ICollection<GroupModel> groups = null,          // ICollection<GroupModel>
                GroupID groupID = null,                         // GroupID object
                ICollection<GroupID> groupIDs = null,           // ICollection<GroupID>
                ContactModel contact = null,                    // ContactModel object
                ICollection<ContactModel> contacts = null,      // ICollection<ContactModel>
                ContactID contactID = null,                     // ContactID object
                ICollection<ContactID> contactIDs = null,       // ICollection<ContactID>
                string destination = null,
                ICollection<string> destinations = null,
                Recipient recipient = null,
                ICollection<Recipient> recipients = null,
                string file = null,
                ICollection<string> files = null,
                Attachment attachment = null,
                ICollection<Attachment> attachments = null,
                string webhookCallbackURL = null,
                WebhookCallbackType? webhookCallbackFormat = null,
                SendModeType? sendMode = null
            )
        {
            return SendMessage(new SMSModel()
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
								ServiceName = serviceName,

								SMSEmailReply = smsEmailReply,
                ForceGSMChars = forceGSMChars,
                MessageText = messageText,

                Recipients = new RecipientList()
                    .Add(group)
                    .Add(groups)
                    .Add(groupID)
                    .Add(groupIDs)
                    .Add(contact)
                    .Add(contacts)
                    .Add(contactID)
                    .Add(contactIDs)
                    .Add(destination)
                    .Add(destinations)
                    .Add(recipient)
                    .Add(recipients)
                    .ToList(),

                Attachments = new AttachmentList()
                    .Add(file)
                    .Add(files)
                    .Add(attachment)
                    .Add(attachments)
                    .ToList(),

                SendMode = sendMode is not null ? (SendModeType)sendMode : SendModeType.Live
            });
        }

        #endregion SendMessage

        #region SendMessageAsync

        /// <summary>
        /// Send SMS Message Async
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
            if (Entity.MessageText == null || Entity.MessageText.Equals(""))
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty message_text: Please specify MessageText");
            }
            if (Entity.StripSlashes == false)
            {
                Entity.MessageText = await TextUtilities.AddSlashesAsync(Entity.MessageText);
            }

            return await SendXMLAsync();
        }

        /// <summary>
        /// Send SMS Message (async)
        /// </summary>
        /// <param name="entity">SMSModel</param>
        /// <returns>Task<MessageResult></returns>
        [ComVisible(false)]
        public async Task<MessageApiResult> SendMessageAsync(SMSModel entity)
        {
            Entity = Mapper.Update(new SMSModel(), entity);

            return await SendMessageAsync();
        }

				/// <summary>
				/// Send SMS Message (async)
				/// </summary>
				/// <param name="messageID">A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
				/// <param name="messageID">MessageID object, A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
				/// <param name="reference">Tracking ID or message description</param>
				/// <param name="sendTime">Delay sending until the specified date/time (your local timezone, specified by your Sender setting or overridden using the Timezone)</param>
				/// <param name="timezone">Timezone specified using Windows timezone value (default set using Web Dashboard can be overridden here)</param>
				/// <param name="subaccount">Used for reporting, billing and Web Dashboard segmentation</param>
				/// <param name="department">Used for reporting, billing and Web Dashboard segmentation</param>
				/// <param name="chargeCode">Cost allocation for billing</param>
				/// <param name="serviceName">Service name for your app</param>
				/// <param name="smsEmailReply">For email (SMTP) reply receipt notifications</param>
				/// <param name="forceGSMChars">Convert multi-byte characters into normalised GSM character format. ie. © to (C)</param>
				/// <param name="messageText">Plain or UTF-8 formatted SMS message</param>
				/// <param name="group">GroupModel object, Sets the recipient group by group id (from TNZ Addressbook)</param>
				/// <param name="groups">List of GroupModel objects, Sets the recipient groups by group ids (from TNZ Addressbook)</param>
				/// <param name="groupID">GroupID object, Sets the recipient group by group id (from TNZ Addressbook)</param>
				/// <param name="groupIDs">List of GroupID objects, Sets the list of recipient groups by list of group ids (from TNZ Addressbook)</param>
				/// <param name="contact">ContactModel object, Sets the recipient by contact id (from TNZ Addressbook)</param>
				/// <param name="contacts">List of ContactModel objects, Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
				/// <param name="contactID">ContactID object, Sets the recipient by contact id (from TNZ Addressbook)</param>
				/// <param name="contactIDs">List of ContactID objects, Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
				/// <param name="destination">Sets the SMS destination</param>
				/// <param name="destinations">Sets the list of SMS destinations</param>
				/// <param name="recipient">Sets the SMS recipient - Recipient() object</param>
				/// <param name="recipients">Sets the list of SMS recipients - List<Recipient>()</param>
				/// <param name="file">Sets the attachment (file location)</param>
				/// <param name="files">Sets the list of attachments (file locations)</param>
				/// <param name="attachment">Sets the attachment - Attachment() object</param>
				/// <param name="attachments">Sets the list of attachments</param>
				/// <param name="webhookCallbackURL">Webhook callback URL</param>
				/// <param name="webhookCallbackFormat">Webhool callback format - XML or JSON</param>
				/// <param name="sendMode">SendModeType.Live or SendModeType.Test</param>
				/// <returns>Task<MessageApiResult></returns>
				public async Task<MessageApiResult> SendMessageAsync(
                MessageID messageID = null,                     // MessageID object
                string reference = null,
                DateTime? sendTime = null,
                string timezone = null,
                string subaccount = null,
                string department = null,
                string chargeCode = null,
								string serviceName = null,
								string smsEmailReply = null,
                string forceGSMChars = null,
                string messageText = null,
                GroupModel group = null,                        // GroupModel object
                ICollection<GroupModel> groups = null,          // ICollection<GroupModel>
                GroupID groupID = null,                         // GroupID object
                ICollection<GroupID> groupIDs = null,           // ICollection<GroupID>
                ContactModel contact = null,                    // ContactModel object
                ICollection<ContactModel> contacts = null,      // ICollection<ContactModel>
                ContactID contactID = null,                     // ContactID object
                ICollection<ContactID> contactIDs = null,       // ICollection<ContactID>
                string destination = null,
                ICollection<string> destinations = null,
                Recipient recipient = null,
                ICollection<Recipient> recipients = null,
                string file = null,
                ICollection<string> files = null,
                Attachment attachment = null,
                ICollection<Attachment> attachments = null,
                string webhookCallbackURL = null,
                WebhookCallbackType? webhookCallbackFormat = null,
                SendModeType? sendMode = null
            )
        {
            return await SendMessageAsync(new SMSModel()
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
                ServiceName = serviceName,

                SMSEmailReply = smsEmailReply,
                ForceGSMChars = forceGSMChars,
                MessageText = messageText,

                Recipients = new RecipientList()
                    .Add(group)
                    .Add(groups)
                    .Add(groupID)
                    .Add(groupIDs)
                    .Add(contact)
                    .Add(contacts)
                    .Add(contactID)
                    .Add(contactIDs)
                    .Add(destination)
                    .Add(destinations)
                    .Add(recipient)
                    .Add(recipients)
                    .ToList(),

                Attachments = await new AttachmentList()
                    .Add(file)
                    .Add(files)
                    .Add(attachment)
                    .Add(attachments)
                    .ToListAsync(),

                SendMode = sendMode is not null ? (SendModeType)sendMode : SendModeType.Live
            });
        }
        #endregion

        #region Deprecated
        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        public MessageApiResult SendMessage(
            string messageID,
            string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string smsEmailReply = null,
            string forceGSMChars = null,
            string messageText = null,
            string destination = null,
            ICollection<string> destinations = null,
            Recipient recipient = null,
            ICollection<Recipient> recipients = null,
            string file = null,
            ICollection<string> files = null,
            Attachment attachment = null,
            ICollection<Attachment> attachments = null,
            string webhookCallbackURL = null,
            Enums.WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        )
            =>
                SendMessage(
          messageID: new MessageID(messageID),        // MessageID object
          reference: reference,
          sendTime: sendTime,
          timezone: timezone,
          subaccount: subaccount,
          department: department,
          chargeCode: chargeCode,
          smsEmailReply: smsEmailReply,
          forceGSMChars: forceGSMChars,
          messageText: messageText,
          groupID: null,                              // GroupID object
          groupIDs: null,                             // ICollection<GroupID>
          contactID: null,                            // ContactID object
          contactIDs: null,                           // ICollection<ContactID>
          destination: destination,
          destinations: destinations,
          recipient: recipient,
          recipients: recipients,
          file: file,
          files: files,
          attachment: attachment,
          attachments: attachments,
          webhookCallbackURL: webhookCallbackURL,
          webhookCallbackFormat: webhookCallbackFormat,
          sendMode: sendMode
        );

        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        public async Task<MessageApiResult> SendMessageAsync(
          string messageID,
          string reference = null,
          DateTime? sendTime = null,
          string timezone = null,
          string subaccount = null,
          string department = null,
          string chargeCode = null,
          string smsEmailReply = null,
          string forceGSMChars = null,
          string messageText = null,
          string destination = null,
          ICollection<string> destinations = null,
          Recipient recipient = null,
          ICollection<Recipient> recipients = null,
          string file = null,
          ICollection<string> files = null,
          Attachment attachment = null,
          ICollection<Attachment> attachments = null,
          string webhookCallbackURL = null,
          Enums.WebhookCallbackType? webhookCallbackFormat = null,
          SendModeType? sendMode = null
        )
                =>
            await SendMessageAsync(
              messageID: new MessageID(messageID),        // MessageID object
              reference: reference,
              sendTime: sendTime,
              timezone: timezone,
              subaccount: subaccount,
              department: department,
              chargeCode: chargeCode,
              smsEmailReply: smsEmailReply,
              forceGSMChars: forceGSMChars,
              messageText: messageText,
              groupID: null,                              // GroupID object
              groupIDs: null,                             // ICollection<GroupID>
              contactID: null,                            // ContactID object
              contactIDs: null,                           // ICollection<ContactID>
              destination: destination,
              destinations: destinations,
              recipient: recipient,
              recipients: recipients,
              file: file,
              files: files,
              attachment: attachment,
              attachments: attachments,
              webhookCallbackURL: webhookCallbackURL,
              webhookCallbackFormat: webhookCallbackFormat,
              sendMode: sendMode
            );
        #endregion
    }
}
