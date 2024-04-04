using System.Runtime.InteropServices;
using System.Xml;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.Email
{
	public class EmailApi : IEmailApi
    {
        private ITNZAuth User = new TNZApiUser();

        public EmailModel Entity { get; set; } = new EmailModel();

        /// <summary>
        /// Initiates Email message to send through TNZAPI
        /// </summary>
        public EmailApi()
        {
        }

        /// <summary>
        /// Initiates Email message to send through TNZAPI
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public EmailApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Initiates Email message to send through TNZAPI
        /// </summary>
        /// <param name="sender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        public EmailApi(string sender, string apiKey)
        {
            User.Sender = sender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Initiates Email message to send through TNZAPI
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public EmailApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Initiates Email message to send through TNZAPI
        /// </summary>
        public EmailApi(ITNZAuth auth)
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
              <MessageType>Email</MessageType>
              <APIVersion>1.02</APIVersion>
              <MessageID>ID123456</MessageID>
              <MessageData>
                <Reference>Test1</Reference>
                <SendTime></SendTime>
                <TimeZone>New Zealand</TimeZone>
                <SubAccount>SubAccount01</SubAccount>
                <Department>Department01</Department>
                <ChargeCode>BillingGroup01</ChargeCode>
                <EmailSubject>Test Email Subject</EmailSubject>
                <SMTPFrom>noreply@example.com</SMTPFrom>
                <From>noreply</From>
                <FromEmail>noreply</FromEmail>
                <ReplyTo>reply@example.com</ReplyTo>
                <MessagePlain>Hello,\r\nThis is a test message.\r\nThank you.</MessagePlain>
                <MessageHTML><![CDATA[<html>Hello,<br /><br />This is a test message.<br /><br />Thank you.</html>]]></MessageHTML>
                <Destinations>
                    <Recipient>recipient.one@example.com</Recipient>
                    <Attention>Recipient One</Attention>
                    <Company>Example Corp</Company>
                    <Custom1></Custom1>
                    <Custom2></Custom2>
                </Destinations>
                <Destinations>recipient.two@example.com</Destinations>
                <Destinations>recipient.three@example.com</Destinations>
                <Destinations>recipient.four@example.com</Destinations>
                <Files>
                    <Name>Sample.pdf</Name>
                    <Data>%%BASE-64 CONTENT%%</Data>
                </Files>
                <Files>
                    <Name>Sample2.pdf</Name>
                    <Data>%%BASE-64 CONTENT%%</Data>
                </Files>    
              </MessageData>
            </root>        
            */
            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("EmailRequest");
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
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ErrorEmailNotify", Entity.ErrorEmailNotify));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "EmailSubject", Entity.EmailSubject));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "SMTPFrom", Entity.SMTPFrom));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "From", Entity.From));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "FromEmail", Entity.FromEmail));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "CCEmail", Entity.CCEmail));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "ReplyTo", Entity.ReplyTo));
            messageDataNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "MessagePlain", Entity.MessagePlain));
            messageDataNode.AppendChild(XMLHelpers.addChildNodeCDATA(xmlDoc, "MessageHTML", Entity.MessageHTML));

            // Set Destinations
            messageDataNode.AppendChild(XMLHelpers.BuildXmlDestinationsNode(xmlDoc, Entity.Recipients, "Email"));

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
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/send/email";
        }

        // Synchronous function for backward compatibility
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
        /// Send Email Message
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
                return ResultHelper.RespondError<MessageApiResult>("Empty entity: Please set options");
            }
            if (Entity.Recipients.Count == 0)
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty recipient(s): Please add Recipient");
            }
            if (Entity.MessagePlain.Equals("") && Entity.MessageHTML.Equals(""))
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty message body: Please specify MessagePlain and/or MessageHTML");
            }

            if (Entity.StripSlashes == false)
            {
                Entity.MessagePlain = TextUtilities.AddSlashes(Entity.MessagePlain);
            }

            return SendXML();
        }

        /// <summary>
        /// Send Email Message
        /// </summary>
        /// <param name="entity">EmailModel</param>
        /// <returns>MessageResult</returns>
        public MessageApiResult SendMessage(EmailModel entity)
        {
            Entity = Mapper.Update(new EmailModel(), entity);

            return SendMessage();
        }


		/// <summary>
		/// Send Email Message
		/// </summary>
		/// <param name="messageID">MessageID object, A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
		/// <param name="emailSubject">Subject field used in the email</param>
		/// <param name="messagePlain">Content used for the message/plain section of the email (overrides 'Template')</param>
		/// <param name="messageHTML">Content used for the message/html section of the email (overrides 'Template' and 'MessagePlain')</param>
		/// <param name="reference">Tracking ID or message description</param>
		/// <param name="sendTime">Delay sending until the specified date/time (your local timezone, specified by your Sender setting or overridden using the Timezone)</param>
		/// <param name="timezone">Timezone specified using Windows timezone value (default set using Web Dashboard can be overridden here)</param>
		/// <param name="subaccount">Used for reporting, billing and Web Dashboard segmentation</param>
		/// <param name="department">Used for reporting, billing and Web Dashboard segmentation</param>
		/// <param name="chargeCode">Cost allocation for billing</param>
		/// <param name="smtpFrom">Sets the email Sender/Return-Path at the SMTP level (this address receives bounce-back emails and is used for SPF/DKIM type authentication; 'FromEmail' is used if not specified)</param>
		/// <param name="fromName">Sets the email sender's Friendly Name (seen by the email recipient)</param>
		/// <param name="fromEmail">Sets the email sender's Email Address (seen by the email recipient; API 'Sender' is used if not specified)</param>
		/// <param name="replyTo">Sets the email sender's Reply-To Address (if the recipient replies, the Reply To will receive the reply)</param>
		/// <param name="groupID">GroupID object, Sets the recipient group by group id (from TNZ Addressbook)</param>
		/// <param name="groupIDs">List of GroupID objects, Sets the list of recipient groups by list of group ids (from TNZ Addressbook)</param>
		/// <param name="contactID">ContactID object, Sets the recipient by contact id (from TNZ Addressbook)</param>
		/// <param name="contactIDs">List of ContactID objects, Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
		/// <param name="destination">Sets the email destination</param>
		/// <param name="destinations">Sets the list of email addresses</param>
		/// <param name="recipient">Sets the email recipient - Recipient() object</param>
		/// <param name="recipients">Sets the list of email recipients - List<Recipient>()</param>
		/// <param name="file">Sets the attachment (file location)</param>
		/// <param name="files">Sets the list of attachments (file locations)</param>
		/// <param name="attachment">Sets the attachment - Attachment() object</param>
		/// <param name="attachments">Sets the list of attachments</param>
		/// <param name="webhookCallbackURL">Webhook callback URL</param>
		/// <param name="webhookCallbackFormat">Webhool callback format - XML or JSON</param>
		/// <param name="sendMode">SendModeType.Live or SendModeType.Test</param>
		/// <returns>MessageApiResult</returns>
		public MessageApiResult SendMessage(
			MessageID messageID = null,                     // MessageID object
            string emailSubject = null,
            string messagePlain = null,
            string messageHTML = null,
			string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string smtpFrom = null,
            string fromName = null,
            string fromEmail = null,
            string replyTo = null,
            GroupID groupID = null,                         // GroupID object
            ICollection<GroupID> groupIDs = null,           // ICollection<GroupID>
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
            return SendMessage(
                new EmailModel()
                {
                    MessageID = messageID,

                    WebhookCallbackURL = webhookCallbackURL,
                    WebhookCallbackFormat = webhookCallbackFormat is not null ? (WebhookCallbackType)webhookCallbackFormat : WebhookCallbackType.JSON,

                    EmailSubject = emailSubject,
                    MessagePlain = messagePlain,
                    MessageHTML = messageHTML,
                    Reference = reference,
                    SendTime = sendTime is not null ? (DateTime)sendTime : DateTime.Now,
                    Timezone = timezone,
                    SubAccount = subaccount,
                    Department = department,
                    ChargeCode = chargeCode,
                    SMTPFrom = smtpFrom,
                    From = fromName,
                    FromEmail = fromEmail,
                    ReplyTo = replyTo,

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
		/// Send Email Message Async
		/// </summary>
		/// <returns>MessageResult</returns>
		private async Task<MessageApiResult> SendMessageAsync()
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
                return ResultHelper.RespondError<MessageApiResult>("Empty entity: Please set options");
            }
            if (Entity.Recipients.Count == 0)
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty recipient(s): Please add Recipient");
            }
            if (Entity.MessagePlain.Equals("") && Entity.MessageHTML.Equals(""))
            {
                return ResultHelper.RespondError<MessageApiResult>("Empty message body: Please specify MessagePlain and/or MessageHTML");
            }

            if (Entity.StripSlashes == false)
            {
                Entity.MessagePlain = TextUtilities.AddSlashes(Entity.MessagePlain);
            }

            return await SendXMLAsync();
        }

        /// <summary>
        /// Send Email Message (async)
        /// </summary>
        /// <param name="entity">EmailModel</param>
        /// <returns>Task<MessageResult></returns>
        [ComVisible(false)]
        public async Task<MessageApiResult> SendMessageAsync(EmailModel entity)
        {
            Entity = Mapper.Update(new EmailModel(), entity);

            return await SendMessageAsync();
        }

		/// <summary>
		/// Send Email Message Async
		/// </summary>
		/// <param name="messageID">MessageiD object, A message tracking identifier (maximum 40 characters, alphanumeric). If you do not supply this field, the API will return one for you in the response body (UUID v4 of 36 characters)</param>
		/// <param name="emailSubject">Subject field used in the email</param>
		/// <param name="messagePlain">Content used for the message/plain section of the email (overrides 'Template')</param>
		/// <param name="messageHTML">Content used for the message/html section of the email (overrides 'Template' and 'MessagePlain')</param>
		/// <param name="reference">Tracking ID or message description</param>
		/// <param name="sendTime">Delay sending until the specified date/time (your local timezone, specified by your Sender setting or overridden using the Timezone)</param>
		/// <param name="timezone">Timezone specified using Windows timezone value (default set using Web Dashboard can be overridden here)</param>
		/// <param name="subaccount">Used for reporting, billing and Web Dashboard segmentation</param>
		/// <param name="department">Used for reporting, billing and Web Dashboard segmentation</param>
		/// <param name="chargeCode">Cost allocation for billing</param>
		/// <param name="smtpFrom">Sets the email Sender/Return-Path at the SMTP level (this address receives bounce-back emails and is used for SPF/DKIM type authentication; 'FromEmail' is used if not specified)</param>
		/// <param name="fromName">Sets the email sender's Friendly Name (seen by the email recipient)</param>
		/// <param name="fromEmail">Sets the email sender's Email Address (seen by the email recipient; API 'Sender' is used if not specified)</param>
		/// <param name="replyTo">Sets the email sender's Reply-To Address (if the recipient replies, the Reply To will receive the reply)</param>
		/// <param name="groupID">GroupID object, Sets the recipient group by group id (from TNZ Addressbook)</param>
		/// <param name="groupIDs">List of GroupID objects, Sets the list of recipient groups by list of group ids (from TNZ Addressbook)</param>
		/// <param name="contactID">ContactID object, Sets the recipient by contact id (from TNZ Addressbook)</param>
		/// <param name="contactIDs">List of ContactID objects, Sets the list of recipient by list of contact ids (from TNZ Addressbook)</param>
		/// <param name="destination">Sets the email destination</param>
		/// <param name="destinations">Sets the list of email addresses</param>
		/// <param name="recipient">Sets the email recipient - Recipient() object</param>
		/// <param name="recipients">Sets the list of email recipients - List<Recipient>()</param>
		/// <param name="file">Sets the attachment (file location)</param>
		/// <param name="files">Sets the list of attachments (file locations)</param>
		/// <param name="attachment">Sets the attachment - Attachment() object</param>
		/// <param name="attachments">Sets the list of attachments</param>
		/// <param name="webhookCallbackURL">Webhook callback URL</param>
		/// <param name="webhookCallbackFormat">Webhool callback format - XML or JSON</param>
		/// <param name="sendMode">SendModeType.Live or SendModeType.Test</param>
		/// <returns>Task<MessageApiResult></returns>
		[ComVisible(false)]
        public async Task<MessageApiResult> SendMessageAsync(
            MessageID messageID = null,                     // MessageID object
            string emailSubject = null,
            string messagePlain = null,
            string messageHTML = null,
            string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string smtpFrom = null,
            string fromName = null,
            string fromEmail = null,
            string replyTo = null,
            GroupID groupID = null,                         // GroupID object
            ICollection<GroupID> groupIDs = null,           // ICollection<GroupID>
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
            return await SendMessageAsync(
                new EmailModel()
                {
					MessageID = messageID,

					WebhookCallbackURL = webhookCallbackURL,
                    WebhookCallbackFormat = webhookCallbackFormat is not null ? (WebhookCallbackType)webhookCallbackFormat : WebhookCallbackType.JSON,

                    EmailSubject = emailSubject,
                    MessagePlain = messagePlain,
                    MessageHTML = messageHTML,
                    Reference = reference,
                    SendTime = sendTime is not null ? (DateTime)sendTime : DateTime.Now,
                    Timezone = timezone,
                    SubAccount = subaccount,
                    Department = department,
                    ChargeCode = chargeCode,
                    SMTPFrom = smtpFrom,
                    From = fromName,
                    FromEmail = fromEmail,
                    ReplyTo = replyTo,

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
			string emailSubject = null,
			string messagePlain = null,
			string messageHTML = null,
			string reference = null,
			DateTime? sendTime = null,
			string timezone = null,
			string subaccount = null,
			string department = null,
			string chargeCode = null,
			string smtpFrom = null,
			string fromName = null,
			string fromEmail = null,
			string replyTo = null,
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
            =>
                SendMessage(
			        messageID: new MessageID(messageID),        // MessageID object
			        emailSubject: emailSubject,
					messagePlain: messagePlain,
					messageHTML: messageHTML,
					reference: reference,
					sendTime: sendTime,
			        timezone: timezone,
			        subaccount: subaccount,
			        department: department,
			        chargeCode: chargeCode,
			        smtpFrom: smtpFrom,
			        fromName: fromName,
		            fromEmail: fromEmail,
			        replyTo: replyTo,
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
			string emailSubject = null,
			string messagePlain = null,
			string messageHTML = null,
			string reference = null,
			DateTime? sendTime = null,
			string timezone = null,
			string subaccount = null,
			string department = null,
			string chargeCode = null,
			string smtpFrom = null,
			string fromName = null,
			string fromEmail = null,
			string replyTo = null,
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
            =>
				await SendMessageAsync(
					messageID: new MessageID(messageID),        // MessageID object
					emailSubject: emailSubject,
					messagePlain: messagePlain,
					messageHTML: messageHTML,
					reference: reference,
					sendTime: sendTime,
					timezone: timezone,
					subaccount: subaccount,
					department: department,
					chargeCode: chargeCode,
					smtpFrom: smtpFrom,
					fromName: fromName,
					fromEmail: fromEmail,
					replyTo: replyTo,
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
