using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Email
{
    public sealed class EmailBuilder : IDisposable
    {
        private EmailModel Entity { get; set; }
        private DestinationList Destinations { get; set; }
        private AttachmentList Attachments { get; set; }

        public EmailBuilder()
        {
            Entity = new EmailModel();
            Destinations = new DestinationList();
            Attachments = new AttachmentList();
        }

        public void Dispose()
        {
            Entity = new EmailModel();
            Destinations.Dispose();
            Attachments.Dispose();
        }

        // AddRecipient(Recipient)/AddRecipients(ICollection<Recipient>) still accept Recipient for
        // source compatibility, but are converted straight to Destination and routed through
        // Destinations — this Builder never populates Entity.Recipients any more, only
        // Entity.Destinations, so every recipient added through this Builder consistently sends the
        // wire's primary "Recipient" field rather than the legacy per-channel alternate
        // (EmailAddress). EmailAddress is Email's channel field, matching
        // EmailApi.ToDestinationBody(Recipient)'s own choice for the same conversion at the
        // wire-serialization layer.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.EmailAddress);
        }

        #region General
        public EmailBuilder SetSendMode(Enums.SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        public EmailBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        public EmailBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        public EmailBuilder SetTemplateID(string templateID)
        {
            Entity.TemplateID = templateID;

            return this;
        }

        public EmailBuilder SetNotificationType(Enums.NotificationType type)
        {
            Entity.NotificationType = type;

            return this;
        }

        public EmailBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        public EmailBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        public EmailBuilder SetWebhookCallbackFormat(Enums.WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        public EmailBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        public EmailBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        public EmailBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        public EmailBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public EmailBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }
        #endregion

        #region Email Specific
        public EmailBuilder SetMessagePlain(string messagePlain)
        {
            Entity.MessagePlain = messagePlain;

            return this;
        }

        public EmailBuilder SetMessageHTML(string messageHTML)
        {
            Entity.MessageHTML = messageHTML;

            return this;
        }

        public EmailBuilder SetSMTPFrom(string emailAddress)
        {
            Entity.SMTPFrom = emailAddress;

            return this;
        }

        public EmailBuilder SetFrom(string friendlyName)
        {
            Entity.From = friendlyName;

            return this;
        }

        public EmailBuilder SetFromEmail(string emailAddress)
        {
            Entity.FromEmail = emailAddress;

            return this;
        }

        public EmailBuilder SetCCEmail(string emailAddress)
        {
            Entity.CCEmail = emailAddress;

            return this;
        }

        public EmailBuilder SetReplyTo(string emailAddress)
        {
            Entity.ReplyTo = emailAddress;

            return this;
        }

        public EmailBuilder SetEmailSubject(string subject)
        {
            Entity.EmailSubject = subject;

            return this;
        }
        #endregion

        #region Add Recipients
        public EmailBuilder AddRecipient(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddRecipient(Recipient recipient)
        {
            Destinations.Add(ToDestination(recipient));

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Destinations.Add(recipients, ToDestination);

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddRecipients(ICollection<string> recipients)
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
        public EmailBuilder AddRecipient(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddRecipients(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Destinations
        public EmailBuilder AddDestination(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddDestination(Destination destination)
        {
            Destinations.Add(destination);

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddDestinations(ICollection<Destination> destinations)
        {
            Destinations.Add(destinations);

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddDestinations(ICollection<string> destinations)
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
        public EmailBuilder AddDestination(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddDestinations(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region AddAttachment
        public EmailBuilder AddAttachment(string fileLocation)
        {
            Attachments.Add(fileLocation);

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddAttachment(string fileName, string fileContent)
        {
            Attachments.Add(new Attachment(fileName, fileContent));

            return this;
        }

        [ComVisible(false)]
        public EmailBuilder AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);

            return this;
        }
        #endregion

        #region Build / BuildAsync
        public EmailModel Build()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = Attachments.ToList();

            return Entity;
        }

        public async Task<EmailModel> BuildAsync()
        {
            Entity.Destinations = Destinations.ToList();
            Entity.Files = await Attachments.ToListAsync();

            return Entity;
        }
        #endregion
    }
}