using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Voice
{
    public sealed class VoiceBuilder : IDisposable
    {
        private VoiceModel Entity { get; set; }
        private DestinationList Destinations { get; set; }
        private List<KeypadModel> Keypads { get; set; }
        private List<VoiceFileModel> VoiceFiles { get; set; }

        public VoiceBuilder()
        {
            Entity = new VoiceModel();
            Destinations = new DestinationList();
            Keypads = new List<KeypadModel>();
            VoiceFiles = new List<VoiceFileModel>();
        }

        public void Dispose()
        {
            Entity = new VoiceModel();
            Destinations.Dispose();
        }

        // AddRecipient(Recipient)/AddRecipients(ICollection<Recipient>) still accept Recipient for
        // source compatibility, but are converted straight to Destination and routed through
        // Destinations — this Builder never populates Entity.Recipients any more, only
        // Entity.Destinations, so every recipient added through this Builder consistently sends the
        // wire's primary "Recipient" field rather than the legacy per-channel alternate (MainPhone).
        // PhoneNumber is Voice's channel field, matching VoiceApi.ToDestinationBody(Recipient)'s own
        // choice for the same conversion at the wire-serialization layer.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.PhoneNumber);
        }

        #region General
        public VoiceBuilder SetSendMode(Enums.SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        public VoiceBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        public VoiceBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        public VoiceBuilder SetTemplateID(string templateID)
        {
            Entity.TemplateID = templateID;

            return this;
        }

        public VoiceBuilder SetNotificationType(Enums.NotificationType type)
        {
            Entity.NotificationType = type;

            return this;
        }

        public VoiceBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        public VoiceBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        public VoiceBuilder SetWebhookCallbackFormat(Enums.WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        public VoiceBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        public VoiceBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        public VoiceBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        public VoiceBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public VoiceBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }
        #endregion

        #region Voice Specific
        public VoiceBuilder SetMessageToPeople(string messageToPeople)
        {
            Entity.MessageToPeople = messageToPeople;

            return this;
        }

        public VoiceBuilder SetMessageToAnswerPhones(string messageToAnswerPhones)
        {
            Entity.MessageToAnswerPhones = messageToAnswerPhones;

            return this;
        }

        public VoiceBuilder SetAnswerPhoneMode(Enums.AnswerPhoneMode mode)
        {
            Entity.AnswerPhoneMode = mode;

            return this;
        }

        public VoiceBuilder SetKeypadOptionRequired(bool required)
        {
            Entity.KeypadOptionRequired = required;

            return this;
        }

        public VoiceBuilder SetCallRouteMessageOnWrongKey(string message)
        {
            Entity.CallRouteMessageOnWrongKey = message;

            return this;
        }

        public VoiceBuilder SetCallRouteMessageToPeople(string message)
        {
            Entity.CallRouteMessageToPeople = message;

            return this;
        }

        public VoiceBuilder SetCallRouteMessageToOperators(string message)
        {
            Entity.CallRouteMessageToOperators = message;

            return this;
        }

        public VoiceBuilder SetEndCallMessage(string message)
        {
            Entity.EndCallMessage = message;

            return this;
        }

        public VoiceBuilder SetNumberOfOperators(int numberOfOperators)
        {
            Entity.NumberOfOperators = numberOfOperators;

            return this;
        }

        public VoiceBuilder SetRetryAttempts(int retryAttempts)
        {
            Entity.RetryAttempts = retryAttempts;

            return this;
        }

        public VoiceBuilder SetRetryPeriod(int retryPeriod)
        {
            Entity.RetryPeriod = retryPeriod;

            return this;
        }

        public VoiceBuilder SetCallerID(string callerID)
        {
            Entity.CallerID = callerID;

            return this;
        }

        public VoiceBuilder SetOptions(string options)
        {
            Entity.Options = options;

            return this;
        }
        #endregion

        #region Add Recipients
        public VoiceBuilder AddRecipient(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddRecipient(Recipient recipient)
        {
            Destinations.Add(ToDestination(recipient));

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Destinations.Add(recipients, ToDestination);

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddRecipients(ICollection<string> recipients)
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
        public VoiceBuilder AddRecipient(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddRecipients(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Destinations
        public VoiceBuilder AddDestination(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddDestination(Destination destination)
        {
            Destinations.Add(destination);

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddDestinations(ICollection<Destination> destinations)
        {
            Destinations.Add(destinations);

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddDestinations(ICollection<string> destinations)
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
        public VoiceBuilder AddDestination(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddDestinations(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Keypads
        public VoiceBuilder AddKeypad(int tone, string play)
        {
            Keypads.Add(new KeypadModel(tone, play));

            return this;
        }

        [ComVisible(false)]
        public VoiceBuilder AddKeypad(KeypadModel keypad)
        {
            Keypads.Add(keypad);

            return this;
        }
        #endregion

        #region Add Voice Files
        public VoiceBuilder AddVoiceFile(string name, string file)
        {
            VoiceFiles.Add(new VoiceFileModel { Name = name, File = file });

            return this;
        }
        #endregion

        #region Build / BuildAsync
        public VoiceModel Build()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Keypads = Keypads;
            Entity.VoiceFiles = VoiceFiles;

            return Entity;
        }

        public Task<VoiceModel> BuildAsync()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Keypads = Keypads;
            Entity.VoiceFiles = VoiceFiles;

            return Task.FromResult(Entity);
        }
        #endregion
    }
}