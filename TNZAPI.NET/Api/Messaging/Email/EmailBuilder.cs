using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.Email
{
		public sealed class EmailBuilder : IDisposable
    {

        private EmailModel Entity { get; set; }

        private RecipientList Recipients { get; set; }

        private AttachmentList Attachments { get; set; }


        /// <summary>
        /// Builder builds email message to send through TNZAPI
        /// </summary>
        public EmailBuilder()
        {
            Entity = new EmailModel();
            Recipients = new RecipientList();
            Attachments = new AttachmentList();
        }

        /// <summary>
        /// Dispose Builder
        /// </summary>
        public void Dispose()
        {
            Entity = null;
            Recipients = null;
            Attachments = null;
        }

				#region General

				/// <summary>
				/// Sets ErrorEmailNotify, email address to get error notifications
				/// </summary>
				/// <param name="emailAddress">Your email address</param>
				/// <returns>EmailBuilder</returns>
				[Obsolete("Use SetReportTo() instead of SetErrorEmailNotify()")]
				public EmailBuilder SetErrorEmailNotify(string emailAddress)
        {
            Entity.ErrorEmailNotify = emailAddress;

            return this;
        }

        /// <summary>
        /// Sets ReportTo, email address to get reports
        /// </summary>
        /// <param name="emailAddress">Your email address</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback URL to receive webhooks
        /// </summary>
        /// <param name="url">Your URL to receive webhooks</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback Format - JSON/XML
        /// </summary>
        /// <param name="type">WebhookCallbackType</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetWebhookCallbackFormat(WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        /// <summary>
        /// Sets Send mode - Live/Text
        /// </summary>
        /// <param name="mode">SendModeType</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetSendMode(SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        /// <summary>
        /// Sets MessageID of the message
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        /// <summary>
        /// Sets MessageID of the message
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetMessageID(MessageID messageID)
        {
            Entity.MessageID = messageID;

            return this;
        }

        /// <summary>
        /// Sets Reference of the message
        /// </summary>
        /// <param name="reference"></param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        /// <summary>
        /// Sets Send Time of the message
        /// </summary>
        /// <param name="sendTime">DateTime</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        /// <summary>
        /// Sets Timezone with SendTime
        /// </summary>
        /// <param name="timezone">string</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        /// <summary>
        /// Sets SubAccount value of the message
        /// </summary>
        /// <param name="subaccount">SubAccount value</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        /// <summary>
        /// Sets Department value of the message
        /// </summary>
        /// <param name="department">Department value</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        /// <summary>
        /// Sets ChargeCode value of the message
        /// </summary>
        /// <param name="chargeCode">Charge Code Value</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }

        #endregion

        #region Email Specific

        /// <summary>
        /// Setting Email Subjects
        /// </summary>
        /// <param name="emailSubject">Subject of email</param>
        /// <returns>EmailBuilder</returns>

        public EmailBuilder SetEmailSubject(string emailSubject)
        {
            Entity.EmailSubject = emailSubject;

            return this;
        }

        /// <summary>
        /// Setting Message Plain
        /// </summary>
        /// <param name="messagePlain">Plain (text) email body</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetMessagePlain(string messagePlain)
        {
            Entity.MessagePlain = messagePlain;

            return this;
        }

        /// <summary>
        /// Setting Message HTML
        /// </summary>
        /// <param name="messageHTML">HTML email body</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder SetMessageHTML(string messageHTML)
        {
            Entity.MessageHTML = messageHTML;

            return this;
        }

        #endregion

        #region SetEmailFrom
        /// <summary>
        /// Setting email sender
        /// </summary>
        /// <param name="fromEmail">Sets the email sender's Email Address (seen by the email recipient; API 'Sender' is used if not specified)</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder SetEmailFrom(string fromEmail)
        {
            Entity.FromEmail = fromEmail;

            return this;
        }

        /// <summary>
        /// Setting email sender
        /// </summary>
        /// <param name="fromName">Sets the email sender's Friendly Name (seen by the email recipient)</param>
        /// <param name="fromEmail">Sets the email sender's Email Address (seen by the email recipient; API 'Sender' is used if not specified)</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder SetEmailFrom(string fromName, string fromEmail)
        {
            Entity.From = fromName;
            Entity.FromEmail = fromEmail;

            return this;
        }

        /// <summary>
        /// Setting email sender
        /// </summary>
        /// <param name="fromName">Sets the email sender's Friendly Name (seen by the email recipient)</param>
        /// <param name="fromEmail">Sets the email sender's Email Address (seen by the email recipient; API 'Sender' is used if not specified)</param>
        /// <param name="replyTo">	Sets the email sender's Reply-To Address (if the recipient replies, the Reply To will receive the reply)</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder SetEmailFrom(string fromName, string fromEmail, string replyTo)
        {
            Entity.From = fromName;
            Entity.FromEmail = fromEmail;
            Entity.ReplyTo = replyTo;

            return this;
        }

        /// <summary>
        /// Setting email sender
        /// </summary>
        /// <param name="smtpFrom">Sets the email Sender/Return-Path at the SMTP level (this address receives bounce-back emails and is used for SPF/DKIM type authentication; 'FromEmail' is used if not specified)</param>
        /// <param name="fromName">Sets the email sender's Friendly Name (seen by the email recipient)</param>
        /// <param name="fromEmail">Sets the email sender's Email Address (seen by the email recipient; API 'Sender' is used if not specified)</param>
        /// <param name="replyTo">	Sets the email sender's Reply-To Address (if the recipient replies, the Reply To will receive the reply)</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder SetEmailFrom(string smtpFrom = null, string fromName = null, string fromEmail = null, string replyTo = null)
        {
            Entity.SMTPFrom = smtpFrom;
            Entity.From = fromName;
            Entity.FromEmail = fromEmail;
            Entity.ReplyTo = replyTo;

            return this;
        }
        #endregion SetEmailFrom

        #region SetCCEmail
        /// <summary>
        /// Setting email cc
        /// </summary>
        /// <param name="emailAddress">Sets the cc email address</param>
        /// <returns></returns>
        public EmailBuilder SetCCEmail(string emailAddress)
        {
            Entity.CCEmail = emailAddress;

            return this;
        }
        #endregion

        #region Add Recipients
        /// <summary>
        /// Adding recipient
        /// </summary>
        /// <param name="recipient">Email Address</param>
        public EmailBuilder AddRecipient(string recipient)
        {
            Recipients.Add(new Recipient()
            {
                EmailAddress = recipient
            });

            return this;
        }

        /// <summary>
        /// Adding recipient object
        /// </summary>
        /// <param name="recipient">Recipient object</param>
        [ComVisible(false)]
        public EmailBuilder AddRecipient(Recipient recipient)
        {
            Recipients.Add(recipient);

            return this;
        }

        /// <summary>
        /// Adding list of Recipient
        /// </summary>
        /// <param name="recipients">Recipient collection</param>
        [ComVisible(false)]
        public EmailBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Recipients.Add(recipients);

            return this;
        }

        /// <summary>
        /// Adding list of Recipient
        /// </summary>
        /// <param name="recipients">Email address list</param>
        [ComVisible(false)]
        public EmailBuilder AddRecipients(ICollection<string> recipients)
        {
            foreach (var recipient in recipients)
            {
                AddRecipient(recipient);
            }

            return this;
        }

        #endregion Add Recipients

        #region Add Recipients using TNZ Addressbook

        /// <summary>
        /// Adding recipient using ContactModel (TNZ Addressbook)
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddRecipient(ContactModel contact)
        {
            Recipients.Add(contact);

            return this;
        }

        /// <summary>
        /// Adding recipient using ContactID (TNZ Addressbook)
        /// </summary>
        /// <param name="contactID">ContactID</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddRecipient(ContactID contactID)
        {
            Recipients.Add(contactID);

            return this;
        }

        /// <summary>
        /// Adding recipients using GroupModel (TNZ Addressbook)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddRecipients(GroupModel group)
        {
            Recipients.Add(group);

            return this;
        }

        /// <summary>
        /// Adding recipients using GroupID (TNZ Addressbook)
        /// </summary>
        /// <param name="groupID">GroupID</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddRecipients(GroupID groupID)
        {
            Recipients.Add(groupID);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of ContactModels (TNZ Addressbook)
        /// </summary>
        /// <param name="contacts">ICollection<ContactModel></param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddRecipients(ICollection<ContactModel> contacts)
        {
            Recipients.Add(contacts);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of ContactIDs (TNZ Addressbook)
        /// </summary>
        /// <param name="contactIDs">ICollection<ContactID></param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddRecipients(ICollection<ContactID> contactIDs)
        {
            Recipients.Add(contactIDs);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of GroupModels (TNZ Addressbook)
        /// </summary>
        /// <param name="groups">ICollection<GroupModel></param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddRecipients(ICollection<GroupModel> groups)
        {
            Recipients.Add(groups);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of GroupIDs (TNZ Addressbook)
        /// </summary>
        /// <param name="groupIDs">ICollection<GroupID></param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddRecipients(ICollection<GroupID> groupIDs)
        {
            Recipients.Add(groupIDs);

            return this;
        }

        #endregion Add Recipients using TNZ Addressbook

        #region AddAttachment
        /// <summary>
        /// Adding attachment
        /// </summary>
        /// <param name="fileLocation">File location of your attachment</param>
        /// <returns>EmailBuilder</returns>
        public EmailBuilder AddAttachment(string fileLocation)
        {
            Attachments.Add(fileLocation);

            return this;
        }

        /// <summary>
        /// Adding attachment
        /// </summary>
        /// <param name="attachment">Attachment for your email message</param>
        /// <returns></returns>
        [ComVisible(false)]
        public EmailBuilder AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);

            return this;
        }

        /// <summary>
        /// Adding attachment)
        /// </summary>
        /// <param name="fileLocations">List of location of your attachments</param>
        /// <returns>EmailBuilder</returns>
        [ComVisible(false)]
        public EmailBuilder AddAttachments(ICollection<string> fileLocations)
        {
            Attachments.Add(fileLocations);

            return this;
        }

        #endregion AddAttachment

        #region Build / BuildAsync

        /// <summary>
        /// Build Email Message
        /// </summary>
        /// <returns>IMessage</returns>
        public EmailModel Build()
        {
            Entity.Recipients = Recipients.ToList();
            Entity.Attachments = Attachments.ToList();

            return Entity;
        }

        /// <summary>
        /// Build Email Message Async
        /// </summary>
        /// <returns>IMessage</returns>
        public async Task<EmailModel> BuildAsync()
        {
            Entity.Recipients = Recipients.ToList();
            Entity.Attachments = await Attachments.ToListAsync();

            return Entity;
        }

        #endregion Build / BuildAsync
    }
}
