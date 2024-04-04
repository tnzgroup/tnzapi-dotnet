using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface ISMSApi
    {
        MessageApiResult SendMessage(SMSModel entity);
        MessageApiResult SendMessage(
            string messageID = null,
			MessageID MessageID = null,                 // MessageID object
			string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string smsEmailReply = null,
            string forceGSMChars = null,
            string messageText = null,
            string groupCode = null,
            ICollection<string> groupCodes = null,
			string groupID = null,
            ICollection<string> groupIDs = null,
			GroupID GroupID = null,                     // GroupID object
			ICollection<GroupID> GroupIDs = null,       // ICollection<GroupID>
			string contactID = null,
			ICollection<string> contactIDs = null,
			ContactID ContactID = null,                 // ContactID object
			ICollection<ContactID> ContactIDs = null,   // ICollection<ContactID>
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

        Task<MessageApiResult> SendMessageAsync(SMSModel entity);
        Task<MessageApiResult> SendMessageAsync(
            string messageID = null,
			MessageID MessageID = null,                 // MessageID object
			string reference = null,
            DateTime? sendTime = null,
            string timezone = null,
            string subaccount = null,
            string department = null,
            string chargeCode = null,
            string smsEmailReply = null,
            string forceGSMChars = null,
            string messageText = null,
			string groupCode = null,
			ICollection<string> groupCodes = null,
			string groupID = null,
			ICollection<string> groupIDs = null,
			GroupID GroupID = null,                     // GroupID object
			ICollection<GroupID> GroupIDs = null,       // ICollection<GroupID>
			string contactID = null,
			ICollection<string> contactIDs = null,
			ContactID ContactID = null,                 // ContactID object
			ICollection<ContactID> ContactIDs = null,   // ICollection<ContactID>
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
        );
    }
}