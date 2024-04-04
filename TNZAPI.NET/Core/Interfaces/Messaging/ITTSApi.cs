﻿using TNZAPI.NET.Api.Addressbook.Contact.Dto;
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
            Enums.TTSVoiceType? ttsVoiceType = null,
            string options = null,
            ICollection<Keypad> keypads = null,
			GroupID groupID = null,                     // GroupID object
			ICollection<GroupID> groupIDs = null,       // ICollection<GroupID>
			ContactID contactID = null,                 // ContactID object
			ICollection<ContactID> contactIDs = null,   // ICollection<ContactID>
			string destination = null,
            ICollection<string> destinations = null,
            Recipient recipient = null,
            ICollection<Recipient> recipients = null,
            string webhookCallbackURL = null,
            Enums.WebhookCallbackType? webhookCallbackFormat = null,
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
            Enums.TTSVoiceType? ttsVoiceType = null,
            string options = null,
            ICollection<Keypad> keypads = null,
			GroupID groupID = null,                     // GroupID object
			ICollection<GroupID> groupIDs = null,       // ICollection<GroupID>
			ContactID contactID = null,                 // ContactID object
			ICollection<ContactID> contactIDs = null,   // ICollection<ContactID>
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