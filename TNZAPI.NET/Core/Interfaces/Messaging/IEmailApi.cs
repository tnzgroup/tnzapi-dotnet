using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface IEmailApi
    {
        MessageApiResult SendMessage(EmailModel entity);
        MessageApiResult SendMessage(
            MessageID messageID = null,         // MessageID object
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
        );

        Task<MessageApiResult> SendMessageAsync(EmailModel entity);
        Task<MessageApiResult> SendMessageAsync(
            MessageID messageID = null,         // MessageID object
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
        );

        #region Deprecated
        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        MessageApiResult SendMessage(
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
        );

        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        Task<MessageApiResult> SendMessageAsync(
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
        );
        #endregion
    }
}