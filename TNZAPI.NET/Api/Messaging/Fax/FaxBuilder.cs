using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Fax
{
    public sealed class FaxBuilder : IDisposable
    {
        private FaxModel Entity { get; set; }
        private DestinationList Destinations { get; set; }
        private AttachmentList Attachments { get; set; }

        public FaxBuilder()
        {
            Entity = new FaxModel();
            Destinations = new DestinationList();
            Attachments = new AttachmentList();
        }

        public void Dispose()
        {
            Entity = new FaxModel();
            Destinations.Dispose();
            Attachments.Dispose();
        }

        // AddRecipient(Recipient)/AddRecipients(ICollection<Recipient>) still accept Recipient for
        // source compatibility, but are converted straight to Destination and routed through
        // Destinations — this Builder never populates Entity.Recipients any more, only
        // Entity.Destinations, so every recipient added through this Builder consistently sends the
        // wire's primary "Recipient" field rather than the legacy per-channel alternate (FaxNumber).
        // FaxNumber is Fax's channel field, matching FaxApi.ToDestinationBody(Recipient)'s own
        // choice for the same conversion at the wire-serialization layer.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.FaxNumber);
        }

        #region General
        public FaxBuilder SetSendMode(Enums.SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        public FaxBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        public FaxBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        public FaxBuilder SetTemplateID(string templateID)
        {
            Entity.TemplateID = templateID;

            return this;
        }

        public FaxBuilder SetNotificationType(Enums.NotificationType type)
        {
            Entity.NotificationType = type;

            return this;
        }

        public FaxBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        public FaxBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        public FaxBuilder SetWebhookCallbackFormat(Enums.WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        public FaxBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        public FaxBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        public FaxBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        public FaxBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public FaxBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }
        #endregion

        #region Fax Specific
        public FaxBuilder SetCSID(string csid)
        {
            Entity.CSID = csid;

            return this;
        }

        public FaxBuilder SetResolution(Enums.FaxResolution resolution)
        {
            Entity.Resolution = resolution;

            return this;
        }

        public FaxBuilder SetWatermarkFolder(string folder)
        {
            Entity.WatermarkFolder = folder;

            return this;
        }

        public FaxBuilder SetWatermarkFirstPage(string watermarkFile)
        {
            Entity.WatermarkFirstPage = watermarkFile;

            return this;
        }

        public FaxBuilder SetWatermarkAllPages(string watermarkFile)
        {
            Entity.WatermarkAllPages = watermarkFile;

            return this;
        }

        public FaxBuilder SetRetryAttempts(int retryAttempts)
        {
            Entity.RetryAttempts = retryAttempts;

            return this;
        }

        public FaxBuilder SetRetryPeriod(int retryPeriod)
        {
            Entity.RetryPeriod = retryPeriod;

            return this;
        }
        #endregion

        #region Add Recipients
        public FaxBuilder AddRecipient(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddRecipient(Recipient recipient)
        {
            Destinations.Add(ToDestination(recipient));

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Destinations.Add(recipients, ToDestination);

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddRecipients(ICollection<string> recipients)
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
        public FaxBuilder AddRecipient(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddRecipients(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Destinations
        public FaxBuilder AddDestination(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddDestination(Destination destination)
        {
            Destinations.Add(destination);

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddDestinations(ICollection<Destination> destinations)
        {
            Destinations.Add(destinations);

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddDestinations(ICollection<string> destinations)
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
        public FaxBuilder AddDestination(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddDestinations(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region AddAttachment
        public FaxBuilder AddAttachment(string fileLocation)
        {
            Attachments.Add(fileLocation);

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddAttachment(string fileName, string fileContent)
        {
            Attachments.Add(new Attachment(fileName, fileContent));

            return this;
        }

        [ComVisible(false)]
        public FaxBuilder AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);

            return this;
        }
        #endregion

        #region Build / BuildAsync
        public FaxModel Build()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = Attachments.ToList();

            return Entity;
        }

        public async Task<FaxModel> BuildAsync()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = await Attachments.ToListAsync();

            return Entity;
        }
        #endregion
    }
}