using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.TTS
{
    public sealed class TTSBuilder : IDisposable
    {
        private TTSModel Entity { get; set; }
        private DestinationList Destinations { get; set; }
        private List<KeypadModel> Keypads { get; set; }

        public TTSBuilder()
        {
            Entity = new TTSModel();
            Destinations = new DestinationList();
            Keypads = new List<KeypadModel>();
        }

        public void Dispose()
        {
            Entity = new TTSModel();
            Destinations.Dispose();
        }

        // AddRecipient(Recipient)/AddRecipients(ICollection<Recipient>) still accept Recipient for
        // source compatibility, but are converted straight to Destination and routed through
        // Destinations — this Builder never populates Entity.Recipients any more, only
        // Entity.Destinations, so every recipient added through this Builder consistently sends the
        // wire's primary "Recipient" field rather than the legacy per-channel alternate (MainPhone).
        // PhoneNumber is TTS's channel field, matching TTSApi.ToDestinationBody(Recipient)'s own
        // choice for the same conversion at the wire-serialization layer.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.PhoneNumber);
        }

        #region General
        public TTSBuilder SetSendMode(Enums.SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        public TTSBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        public TTSBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        public TTSBuilder SetTemplateID(string templateID)
        {
            Entity.TemplateID = templateID;

            return this;
        }

        public TTSBuilder SetNotificationType(Enums.NotificationType type)
        {
            Entity.NotificationType = type;

            return this;
        }

        public TTSBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        public TTSBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        public TTSBuilder SetWebhookCallbackFormat(Enums.WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        public TTSBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        public TTSBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        public TTSBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        public TTSBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public TTSBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }
        #endregion

        #region TTS Specific
        public TTSBuilder SetMessageToPeople(string messageToPeople)
        {
            Entity.MessageToPeople = messageToPeople;

            return this;
        }

        public TTSBuilder SetMessageToAnswerPhones(string messageToAnswerPhones)
        {
            Entity.MessageToAnswerPhones = messageToAnswerPhones;

            return this;
        }

        public TTSBuilder SetAnswerPhoneMode(Enums.AnswerPhoneMode mode)
        {
            Entity.AnswerPhoneMode = mode;

            return this;
        }

        public TTSBuilder SetKeypadOptionRequired(bool required)
        {
            Entity.KeypadOptionRequired = required;

            return this;
        }

        public TTSBuilder SetCallRouteMessageOnWrongKey(string message)
        {
            Entity.CallRouteMessageOnWrongKey = message;

            return this;
        }

        public TTSBuilder SetCallRouteMessageToPeople(string message)
        {
            Entity.CallRouteMessageToPeople = message;

            return this;
        }

        public TTSBuilder SetCallRouteMessageToOperators(string message)
        {
            Entity.CallRouteMessageToOperators = message;

            return this;
        }

        public TTSBuilder SetEndCallMessage(string message)
        {
            Entity.EndCallMessage = message;

            return this;
        }

        public TTSBuilder SetNumberOfOperators(int numberOfOperators)
        {
            Entity.NumberOfOperators = numberOfOperators;

            return this;
        }

        public TTSBuilder SetRetryAttempts(int retryAttempts)
        {
            Entity.RetryAttempts = retryAttempts;

            return this;
        }

        public TTSBuilder SetRetryPeriod(int retryPeriod)
        {
            Entity.RetryPeriod = retryPeriod;

            return this;
        }

        public TTSBuilder SetCallerID(string callerID)
        {
            Entity.CallerID = callerID;

            return this;
        }

        public TTSBuilder SetVoice(string voice)
        {
            Entity.Voice = voice;

            return this;
        }

        public TTSBuilder SetOptions(string options)
        {
            Entity.Options = options;

            return this;
        }
        #endregion

        #region Add Recipients
        public TTSBuilder AddRecipient(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddRecipient(Recipient recipient)
        {
            Destinations.Add(ToDestination(recipient));

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Destinations.Add(recipients, ToDestination);

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddRecipients(ICollection<string> recipients)
        {
            foreach (var recipient in recipients)
            {
                AddRecipient(recipient);
            }

            return this;
        }
        #endregion

        #region Add Recipients using TNZ Addressbook
        [ComVisible(false)]
        public TTSBuilder AddRecipient(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddRecipients(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Destinations
        public TTSBuilder AddDestination(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddDestination(Destination destination)
        {
            Destinations.Add(destination);

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddDestinations(ICollection<Destination> destinations)
        {
            Destinations.Add(destinations);

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddDestinations(ICollection<string> destinations)
        {
            foreach (var destination in destinations)
            {
                AddDestination(destination);
            }

            return this;
        }
        #endregion

        #region Add Destinations using TNZ Addressbook
        [ComVisible(false)]
        public TTSBuilder AddDestination(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddDestinations(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Keypads
        public TTSBuilder AddKeypad(int tone, string play)
        {
            Keypads.Add(new KeypadModel(tone, play));

            return this;
        }

        [ComVisible(false)]
        public TTSBuilder AddKeypad(KeypadModel keypad)
        {
            Keypads.Add(keypad);

            return this;
        }
        #endregion

        #region Build / BuildAsync
        public TTSModel Build()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Keypads = Keypads;

            return Entity;
        }

        public Task<TTSModel> BuildAsync()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Keypads = Keypads;

            return Task.FromResult(Entity);
        }
        #endregion
    }
}