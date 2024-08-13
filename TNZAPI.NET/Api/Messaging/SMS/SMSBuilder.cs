using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Helpers;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.SMS
{
    public sealed class SMSBuilder : IDisposable
    {
        private SMSModel Entity { get; set; }

        private RecipientList Recipients { get; set; }

        private AttachmentList Attachments { get; set; }

        /// <summary>
        /// Builder builds sms message to send through TNZAPI
        /// </summary>
        public SMSBuilder()
        {
            Entity = new SMSModel();
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
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetErrorEmailNotify(string emailAddress)
        {
            Entity.ErrorEmailNotify = emailAddress;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback URL to receive webhooks
        /// </summary>
        /// <param name="url">Your URL to receive webhooks</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback Format - JSON/XML
        /// </summary>
        /// <param name="type">WebhookCallbackType</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetWebhookCallbackFormat(WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        /// <summary>
        /// Sets Send mode - Live/Text
        /// </summary>
        /// <param name="mode">SendModeType</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetSendMode(SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        /// <summary>
        /// Sets MessageID of the message
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        /// <summary>
        /// Sets MessageID of the message
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetMessageID(MessageID messageID)
        {
            Entity.MessageID = messageID;

            return this;
        }

        /// <summary>
        /// Sets Reference of the message
        /// </summary>
        /// <param name="reference"></param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        /// <summary>
        /// Sets Send Time of the message
        /// </summary>
        /// <param name="sendTime">DateTime</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        /// <summary>
        /// Sets Timezone with SendTime
        /// </summary>
        /// <param name="timezone">string</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        /// <summary>
        /// Sets SubAccount value of the message
        /// </summary>
        /// <param name="subaccount">SubAccount value</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        /// <summary>
        /// Sets Department value of the message
        /// </summary>
        /// <param name="department">Department value</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        /// <summary>
        /// Sets ChargeCode value of the message
        /// </summary>
        /// <param name="chargeCode">Charge Code Value</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }

        #endregion

        #region SMS Specific

        /// <summary>
        /// Sets origination number (to overseas only)
        /// </summary>
        /// <param name="number">From number</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetFromNumber(string number)
        {
            Entity.FromNumber = number;

            return this;
        }

        /// <summary>
        /// Sets reply email address
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetSMSEmailReply(string emailAddress)
        {
            Entity.SMSEmailReply = emailAddress;

            return this;
        }

        /// <summary>
        /// Convert multi-byte characters into normalised GSM character format. ie. © to (C)
        /// </summary>
        /// <param name="truefalse">True/False</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetCharacterConversion(string truefalse)
        {
            Entity.ForceGSMChars = truefalse.ToLower() == "true" ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Convert multi-byte characters into normalised GSM character format. ie. © to (C)
        /// </summary>
        /// <param name="truefalse">True/False</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetCharacterConversion(bool truefalse)
        {
            Entity.ForceGSMChars = truefalse == true ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Sets message body
        /// </summary>
        /// <param name="messageText">SMS message</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder SetMessageText(string messageText)
        {
            Entity.MessageText = messageText;

            return this;
        }

        #endregion

        #region Add Recipients
        /// <summary>
        /// Adding recipient
        /// </summary>
        /// <param name="recipient">Fax number</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder AddRecipient(string recipient)
        {
            Recipients.Add(new Recipient()
            {
                MobileNumber = recipient
            });

            return this;
        }

        /// <summary>
        /// Adding recipient object
        /// </summary>
        /// <param name="recipient">Recipient object</param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddRecipient(Recipient recipient)
        {
            Recipients.Add(recipient);

            return this;
        }

        /// <summary>
        /// Adding list of Recipient
        /// </summary>
        /// <param name="recipients">Recipient collection</param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Recipients.Add(recipients);

            return this;
        }

        /// <summary>
        /// Adding list of Recipient
        /// </summary>
        /// <param name="recipients">Fax number list</param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddRecipients(ICollection<string> recipients)
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
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddRecipient(ContactModel contact)
        {
            Recipients.Add(contact);

            return this;
        }

        /// <summary>
        /// Adding recipient using ContactID (TNZ Addressbook)
        /// </summary>
        /// <param name="contactID">ContactID</param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddRecipient(ContactID contactID)
        {
            Recipients.Add(contactID);

            return this;
        }

        /// <summary>
        /// Adding recipient using GroupModel (TNZ Addressbook)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddRecipients(GroupModel group)
        {
            Recipients.Add(group);

            return this;
        }

        /// <summary>
        /// Adding recipients using GroupID (TNZ Addressbook)
        /// </summary>
        /// <param name="groupID">GroupID</param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddRecipients(GroupID groupID)
        {
            Recipients.Add(groupID);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of ContactModels (TNZ Addressbook)
        /// </summary>
        /// <param name="contacts">ICollection<ContactModel></param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddRecipients(ICollection<ContactModel> contacts)
        {
            Recipients.Add(contacts);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of ContactIDs (TNZ Addressbook)
        /// </summary>
        /// <param name="contactIDs">ICollection<ContactID></param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder AddRecipients(ICollection<ContactID> contactIDs)
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
        public SMSBuilder AddRecipients(ICollection<GroupModel> groups)
        {
            Recipients.Add(groups);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of GroupIDs (TNZ Addressbook)
        /// </summary>
        /// <param name="groupIDs">ICollection<GroupID></param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder AddRecipients(ICollection<GroupID> groupIDs)
        {
            Recipients.Add(groupIDs);

            return this;
        }

        #endregion Add Recipients using TNZ Addressbook

        #region AddAttachment
        /// <summary>
        /// Adding attachment
        /// </summary>
        /// <param name="fileLocation">File location</param>
        /// <returns>SMSBuilder</returns>
        public SMSBuilder AddAttachment(string fileLocation)
        {
            Attachments.Add(fileLocation);

            return this;
        }

        /// <summary>
        /// Adding attachment
        /// </summary>
        /// <param name="attachment">Attachment for your fax</param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);

            return this;
        }

        /// <summary>
        /// Adding attachment)
        /// </summary>
        /// <param name="fileLocations">List of location of your attachments</param>
        /// <returns>SMSBuilder</returns>
        [ComVisible(false)]
        public SMSBuilder AddAttachments(ICollection<string> fileLocations)
        {
            Attachments.Add(fileLocations);

            return this;
        }

        #endregion AddAttachment

        #region Build / BuildAsync

        /// <summary>
        /// Build SMS Message
        /// </summary>
        /// <returns>SMSModel</returns>
        public SMSModel Build()
        {
            Entity.Recipients = Recipients.ToList();
            Entity.Attachments = Attachments.ToList();

            return Entity;
        }

        /// <summary>
        /// Build SMS Message Async
        /// </summary>
        /// <returns>IMessage</returns>
        public async Task<SMSModel> BuildAsync()
        {
            Entity.Recipients = Recipients.ToList();
            Entity.Attachments = await Attachments.ToListAsync();

            return Entity;
        }
        #endregion Build / BuildAsync
    }
}
