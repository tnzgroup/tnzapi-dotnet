using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.SMS
{
    public sealed class SMSBuilder : IDisposable
    {
        private SMSModel Entity { get; set; }
        private DestinationList Destinations { get; set; }
        private AttachmentList Attachments { get; set; }
        private List<Enums.SMSFallbackMode> FallbackModes { get; set; }

        public SMSBuilder()
        {
            Entity = new SMSModel();
            Destinations = new DestinationList();
            Attachments = new AttachmentList();
            FallbackModes = new List<Enums.SMSFallbackMode>();
        }

        public void Dispose()
        {
            Entity = new SMSModel();
            Destinations.Dispose();
            Attachments.Dispose();
        }

        // AddRecipient(Recipient)/AddRecipients(ICollection<Recipient>) still accept Recipient for
        // source compatibility, but are converted straight to Destination and routed through
        // Destinations — this Builder never populates Entity.Recipients any more, only
        // Entity.Destinations, so every recipient added through this Builder consistently sends the
        // wire's primary "Recipient" field rather than the legacy per-channel alternate (ToNumber).
        // MobileNumber is SMS's channel field, matching SMSApi.ToDestinationBody(Recipient)'s own
        // choice for the same conversion at the wire-serialization layer.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.MobileNumber);
        }

        #region General
        public SMSBuilder SetSendMode(Enums.SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        public SMSBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        public SMSBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        public SMSBuilder SetTemplateID(string templateID)
        {
            Entity.TemplateID = templateID;

            return this;
        }

        public SMSBuilder SetNotificationType(Enums.NotificationType type)
        {
            Entity.NotificationType = type;

            return this;
        }

        public SMSBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        public SMSBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        public SMSBuilder SetWebhookCallbackFormat(Enums.WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        public SMSBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        public SMSBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        public SMSBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        public SMSBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public SMSBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }
        #endregion

        #region SMS Specific
        public SMSBuilder SetFromNumber(string number)
        {
            Entity.FromNumber = number;

            return this;
        }

        public SMSBuilder SetSMSEmailReply(string emailAddress)
        {
            Entity.SMSEmailReply = emailAddress;

            return this;
        }

        public SMSBuilder SetCharacterConversion(bool truefalse)
        {
            Entity.CharacterConversion = truefalse;

            return this;
        }

        public SMSBuilder AddFallbackMode(Enums.SMSFallbackMode mode)
        {
            FallbackModes.Add(mode);

            return this;
        }

        public SMSBuilder SetMessageText(string messageText)
        {
            Entity.Message = messageText;

            return this;
        }
        #endregion

        #region Add Recipients
        public SMSBuilder AddRecipient(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddRecipient(Recipient recipient)
        {
            Destinations.Add(ToDestination(recipient));

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Destinations.Add(recipients, ToDestination);

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddRecipients(ICollection<string> recipients)
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
        public SMSBuilder AddRecipient(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddRecipients(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Destinations
        public SMSBuilder AddDestination(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddDestination(Destination destination)
        {
            Destinations.Add(destination);

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddDestinations(ICollection<Destination> destinations)
        {
            Destinations.Add(destinations);

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddDestinations(ICollection<string> destinations)
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
        public SMSBuilder AddDestination(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddDestinations(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region AddAttachment
        public SMSBuilder AddAttachment(string fileLocation)
        {
            Attachments.Add(fileLocation);

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddAttachment(string fileName, string fileContent)
        {
            Attachments.Add(new Attachment(fileName, fileContent));

            return this;
        }

        [ComVisible(false)]
        public SMSBuilder AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);

            return this;
        }
        #endregion

        #region Build / BuildAsync
        public SMSModel Build()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = Attachments.ToList();
            Entity.FallbackMode = FallbackModes.ToList();

            return Entity;
        }

        public async Task<SMSModel> BuildAsync()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = await Attachments.ToListAsync();
            Entity.FallbackMode = FallbackModes.ToList();

            return Entity;
        }
        #endregion
    }
}