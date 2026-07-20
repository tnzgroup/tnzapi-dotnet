using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.WhatsApp.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.WhatsApp
{
    public sealed class WhatsAppBuilder : IDisposable
    {
        private WhatsAppModel Entity { get; set; }
        private DestinationList Destinations { get; set; }
        private AttachmentList Attachments { get; set; }
        private List<Enums.WhatsAppFallbackMode> FallbackModes { get; set; }

        public WhatsAppBuilder()
        {
            Entity = new WhatsAppModel();
            Destinations = new DestinationList();
            Attachments = new AttachmentList();
            FallbackModes = new List<Enums.WhatsAppFallbackMode>();
        }

        public void Dispose()
        {
            Entity = new WhatsAppModel();
            Destinations.Dispose();
            Attachments.Dispose();
        }

        // AddRecipient(Recipient)/AddRecipients(ICollection<Recipient>) still accept Recipient for
        // source compatibility, but are converted straight to Destination and routed through
        // Destinations — this Builder never populates Entity.Recipients any more, only
        // Entity.Destinations, so every recipient added through this Builder consistently sends the
        // wire's primary "Recipient" field rather than the legacy per-channel alternate (ToNumber).
        // MobileNumber is WhatsApp's channel field, matching
        // WhatsAppApi.ToDestinationBody(Recipient)'s own choice for the same conversion at the
        // wire-serialization layer.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.MobileNumber);
        }

        #region General
        public WhatsAppBuilder SetSendMode(Enums.SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        public WhatsAppBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        public WhatsAppBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        public WhatsAppBuilder SetTemplateID(string templateID)
        {
            Entity.TemplateID = templateID;

            return this;
        }

        public WhatsAppBuilder SetNotificationType(Enums.NotificationType type)
        {
            Entity.NotificationType = type;

            return this;
        }

        public WhatsAppBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        public WhatsAppBuilder SetWebhookCallbackFormat(Enums.WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        public WhatsAppBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        public WhatsAppBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        public WhatsAppBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        public WhatsAppBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        public WhatsAppBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public WhatsAppBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }
        #endregion

        #region WhatsApp Specific
        public WhatsAppBuilder SetMessageText(string messageText)
        {
            Entity.Message = messageText;

            return this;
        }

        public WhatsAppBuilder SetFromNumber(string fromNumber)
        {
            Entity.FromNumber = fromNumber;

            return this;
        }

        public WhatsAppBuilder AddFallbackMode(Enums.WhatsAppFallbackMode mode)
        {
            FallbackModes.Add(mode);

            return this;
        }
        #endregion

        #region Add Recipients
        public WhatsAppBuilder AddRecipient(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddRecipient(Recipient recipient)
        {
            Destinations.Add(ToDestination(recipient));

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Destinations.Add(recipients, ToDestination);

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddRecipients(ICollection<string> recipients)
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
        public WhatsAppBuilder AddRecipient(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddRecipients(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Destinations
        public WhatsAppBuilder AddDestination(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddDestination(Destination destination)
        {
            Destinations.Add(destination);

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddDestinations(ICollection<Destination> destinations)
        {
            Destinations.Add(destinations);

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddDestinations(ICollection<string> destinations)
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
        public WhatsAppBuilder AddDestination(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddDestinations(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region AddAttachment
        public WhatsAppBuilder AddAttachment(string fileLocation)
        {
            Attachments.Add(fileLocation);

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddAttachment(string fileName, string fileContent)
        {
            Attachments.Add(new Attachment(fileName, fileContent));

            return this;
        }

        [ComVisible(false)]
        public WhatsAppBuilder AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);

            return this;
        }
        #endregion

        #region Build / BuildAsync
        public WhatsAppModel Build()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = Attachments.ToList();
            Entity.FallbackMode = FallbackModes.ToList();

            return Entity;
        }

        public async Task<WhatsAppModel> BuildAsync()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = await Attachments.ToListAsync();
            Entity.FallbackMode = FallbackModes.ToList();

            return Entity;
        }
        #endregion
    }
}