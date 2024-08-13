using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface ITTSApi
    {
        MessageApiResult SendMessage(TTSModel entity);
        MessageApiResult SendMessage(
            MessageID messageID = null,                 // MessageID object
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
            int? retryAttempts = null,
            int? retryPeriod = null,
            string callerID = null,
            TTSVoiceType? ttsVoiceType = null,
            string options = null,
            ICollection<Keypad> keypads = null,
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
            string webhookCallbackURL = null,
            WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        );

        Task<MessageApiResult> SendMessageAsync(TTSModel entity);
        Task<MessageApiResult> SendMessageAsync(
            MessageID messageID = null,                 // MessageID object
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
            int? retryAttempts = null,
            int? retryPeriod = null,
            string callerID = null,
            TTSVoiceType? ttsVoiceType = null,
            string options = null,
            ICollection<Keypad> keypads = null,
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
            string webhookCallbackURL = null,
            WebhookCallbackType? webhookCallbackFormat = null,
            SendModeType? sendMode = null
        );

        #region Deprecated
        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        MessageApiResult SendMessage(
          string messageID,
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
          int? retryAttempts = null,
          int? retryPeriod = null,
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
        );

        [Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
        Task<MessageApiResult> SendMessageAsync(
          string messageID,
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
          int? retryAttempts = null,
          int? retryPeriod = null,
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
        );
        #endregion
    }
}