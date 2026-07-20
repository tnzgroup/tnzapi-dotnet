using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.RCS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.RCS
{
    public sealed class RCSBuilder : IDisposable
    {
        private RCSModel Entity { get; set; }
        private DestinationList Destinations { get; set; }
        private AttachmentList Attachments { get; set; }
        private List<Enums.RCSFallbackMode> FallbackModes { get; set; }

        public RCSBuilder()
        {
            Entity = new RCSModel();
            Destinations = new DestinationList();
            Attachments = new AttachmentList();
            FallbackModes = new List<Enums.RCSFallbackMode>();
        }

        public void Dispose()
        {
            Entity = new RCSModel();
            Destinations.Dispose();
            Attachments.Dispose();
        }

        // AddRecipient(Recipient)/AddRecipients(ICollection<Recipient>) still accept Recipient for
        // source compatibility, but are converted straight to Destination and routed through
        // Destinations — this Builder never populates Entity.Recipients any more, only
        // Entity.Destinations, so every recipient added through this Builder consistently sends the
        // wire's primary "Recipient" field rather than the legacy per-channel alternate (ToNumber).
        // MobileNumber is RCS's channel field, matching RCSApi.ToDestinationBody(Recipient)'s own
        // choice for the same conversion at the wire-serialization layer.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.MobileNumber);
        }

        #region General
        public RCSBuilder SetSendMode(Enums.SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        public RCSBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        public RCSBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        public RCSBuilder SetTemplateID(string templateID)
        {
            Entity.TemplateID = templateID;

            return this;
        }

        public RCSBuilder SetNotificationType(Enums.NotificationType type)
        {
            Entity.NotificationType = type;

            return this;
        }

        public RCSBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        public RCSBuilder SetWebhookCallbackFormat(Enums.WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        public RCSBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        public RCSBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        public RCSBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        public RCSBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        public RCSBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public RCSBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }
        #endregion

        #region RCS Specific
        public RCSBuilder SetMessageText(string messageText)
        {
            Entity.Message = messageText;

            return this;
        }

        public RCSBuilder SetFromNumber(string fromNumber)
        {
            Entity.FromNumber = fromNumber;

            return this;
        }

        public RCSBuilder SetSMSEmailReply(string emailAddress)
        {
            Entity.SMSEmailReply = emailAddress;

            return this;
        }

        public RCSBuilder SetCharacterConversion(bool truefalse)
        {
            Entity.CharacterConversion = truefalse;

            return this;
        }
        #endregion

        #region Fallback Mode
        public RCSBuilder AddFallbackMode(Enums.RCSFallbackMode mode)
        {
            FallbackModes.Add(mode);

            return this;
        }
        #endregion

        #region Add Recipients
        public RCSBuilder AddRecipient(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddRecipient(Recipient recipient)
        {
            Destinations.Add(ToDestination(recipient));

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Destinations.Add(recipients, ToDestination);

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddRecipients(ICollection<string> recipients)
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
        public RCSBuilder AddRecipient(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddRecipients(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Destinations
        public RCSBuilder AddDestination(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddDestination(Destination destination)
        {
            Destinations.Add(destination);

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddDestinations(ICollection<Destination> destinations)
        {
            Destinations.Add(destinations);

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddDestinations(ICollection<string> destinations)
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
        public RCSBuilder AddDestination(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddDestinations(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region AddAttachment
        public RCSBuilder AddAttachment(string fileLocation)
        {
            Attachments.Add(fileLocation);

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddAttachment(string fileName, string fileContent)
        {
            Attachments.Add(new Attachment(fileName, fileContent));

            return this;
        }

        [ComVisible(false)]
        public RCSBuilder AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);

            return this;
        }
        #endregion

        #region Build / BuildAsync
        public RCSModel Build()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = Attachments.ToList();
            Entity.FallbackMode = FallbackModes.ToList();

            return Entity;
        }

        public async Task<RCSModel> BuildAsync()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = await Attachments.ToListAsync();
            Entity.FallbackMode = FallbackModes.ToList();

            return Entity;
        }
        #endregion
    }
}