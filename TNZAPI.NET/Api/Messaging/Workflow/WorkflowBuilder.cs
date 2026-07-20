using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Workflow.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Workflow
{
    public sealed class WorkflowBuilder : IDisposable
    {
        private WorkflowModel Entity { get; set; }
        private DestinationList Destinations { get; set; }

        public WorkflowBuilder()
        {
            Entity = new WorkflowModel();
            Destinations = new DestinationList();
        }

        public void Dispose()
        {
            Entity = new WorkflowModel();
            Destinations.Dispose();
        }

        // AddRecipient(Recipient)/AddRecipients(ICollection<Recipient>) still accept Recipient for
        // source compatibility, but are converted straight to Destination and routed through
        // Destinations — this Builder never populates Entity.Recipients any more, only
        // Entity.Destinations. Unlike other modules' single-channel ToDestination helpers, Workflow's
        // Recipient carries three independent channel fields simultaneously (MobileNumber/
        // PhoneNumber/EmailAddress), so this maps onto Destination's ToNumber/MainPhone/EmailAddress
        // alternates rather than the single primary Recipient field — mirroring
        // WorkflowApi.ToDestinationBody(Recipient)'s own field choices for the same conversion at the
        // wire-serialization layer.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.MobileNumber, recipient.PhoneNumber, recipient.EmailAddress);
        }

        #region General
        public WorkflowBuilder SetSendMode(Enums.SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        public WorkflowBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        public WorkflowBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        public WorkflowBuilder SetNotificationType(Enums.NotificationType type)
        {
            Entity.NotificationType = type;

            return this;
        }

        public WorkflowBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        public WorkflowBuilder SetWebhookCallbackFormat(Enums.WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        public WorkflowBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        public WorkflowBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        public WorkflowBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        public WorkflowBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public WorkflowBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }
        #endregion

        #region Workflow Specific
        public WorkflowBuilder SetWorkflowTemplateID(string workflowTemplateID)
        {
            Entity.WorkflowTemplateID = workflowTemplateID;

            return this;
        }
        #endregion

        #region Add Recipients
        public WorkflowBuilder AddRecipient(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public WorkflowBuilder AddRecipient(Recipient recipient)
        {
            Destinations.Add(ToDestination(recipient));

            return this;
        }

        [ComVisible(false)]
        public WorkflowBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Destinations.Add(recipients, ToDestination);

            return this;
        }

        [ComVisible(false)]
        public WorkflowBuilder AddRecipients(ICollection<string> recipients)
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
        public WorkflowBuilder AddRecipient(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public WorkflowBuilder AddRecipients(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Add Destinations
        public WorkflowBuilder AddDestination(string recipient)
        {
            Destinations.Add(recipient);

            return this;
        }

        [ComVisible(false)]
        public WorkflowBuilder AddDestination(Destination destination)
        {
            Destinations.Add(destination);

            return this;
        }

        [ComVisible(false)]
        public WorkflowBuilder AddDestinations(ICollection<Destination> destinations)
        {
            Destinations.Add(destinations);

            return this;
        }

        [ComVisible(false)]
        public WorkflowBuilder AddDestinations(ICollection<string> destinations)
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
        public WorkflowBuilder AddDestination(ContactID contactID)
        {
            Destinations.Add(contactID);

            return this;
        }

        [ComVisible(false)]
        public WorkflowBuilder AddDestinations(GroupID groupID)
        {
            Destinations.Add(groupID);

            return this;
        }
        #endregion

        #region Build / BuildAsync
        public WorkflowModel Build()
        {
            Entity.Destinations = Destinations.ToList();

            return Entity;
        }

        public Task<WorkflowModel> BuildAsync()
        {
            Entity.Destinations = Destinations.ToList();

            return Task.FromResult(Entity);
        }
        #endregion
    }
}