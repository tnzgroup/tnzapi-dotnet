using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface IVoiceApi
    {
        MessageApiResult SendMessage(VoiceModel entity);
        MessageApiResult SendMessage(
            string messageID = null,
			MessageID MessageID = null,                 // MessageID object
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
            string webhookCallbackURL = null,
            Enums.WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        );

        Task<MessageApiResult> SendMessageAsync(VoiceModel entity);
        Task<MessageApiResult> SendMessageAsync(
            string messageID = null,
			MessageID MessageID = null,                 // MessageID object
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
            string webhookCallbackURL = null,
            Enums.WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        );
    }
}