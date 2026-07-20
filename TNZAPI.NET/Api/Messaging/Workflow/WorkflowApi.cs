using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Workflow.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.Workflow
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class WorkflowApi : IWorkflowApi
    {
        private ITNZAuth User = new TNZApiUser();

        public WorkflowApi()
        {
        }

        public WorkflowApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        public WorkflowApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        public WorkflowApi(ITNZAuth auth)
        {
            User = auth;
        }

        #region Set API User
        public void SetAPIUser(ITNZAuth apiUser)
        {
            User = apiUser;
        }

        public void SetAuthToken(string authToken)
        {
            User.AuthToken = authToken;
        }
        #endregion

        private static string BuildAPIURL()
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/workflow";
        }

        private static WorkflowRequestBody BuildRequestBody(WorkflowModel entity)
        {
            return new WorkflowRequestBody
            {
                WorkflowTemplateID = entity.WorkflowTemplateID,
                MessageID = entity.MessageID,
                Reference = entity.Reference,
                ContactID = entity.ContactID,
                GroupID = entity.GroupID,
                Destinations = (entity.Recipients?.Select(ToDestinationBody) ?? Enumerable.Empty<WorkflowDestinationBody>())
                    .Concat(entity.Destinations?.Select(ToDestinationBody) ?? Enumerable.Empty<WorkflowDestinationBody>())
                    .ToList(),
                NotificationType = entity.NotificationType,
                WebhookCallbackURL = entity.WebhookCallbackURL,
                WebhookCallbackFormat = entity.WebhookCallbackFormat,
                SubAccount = entity.SubAccount,
                Department = entity.Department,
                ChargeCode = entity.ChargeCode,
                SendTime = entity.SendTime,
                Timezone = entity.Timezone,
                Mode = entity.SendMode == Enums.SendModeType.Test ? "Test" : null
            };
        }

        private static WorkflowDestinationBody ToDestinationBody(Recipient recipient)
        {
            return new WorkflowDestinationBody
            {
                ToNumber = recipient.MobileNumber,
                MainPhone = recipient.PhoneNumber,
                EmailAddress = recipient.EmailAddress,
                Attention = recipient.Attention,
                Company = recipient.CompanyName,
                FirstName = recipient.FirstName,
                LastName = recipient.LastName,
                Custom1 = recipient.Custom1,
                Custom2 = recipient.Custom2,
                Custom3 = recipient.Custom3,
                Custom4 = recipient.Custom4,
                Custom5 = recipient.Custom5,
                Custom6 = recipient.Custom6,
                Custom7 = recipient.Custom7,
                Custom8 = recipient.Custom8,
                Custom9 = recipient.Custom9,
                ContactID = recipient.ContactID,
                GroupID = recipient.GroupID,
                GroupCode = recipient.GroupCode
            };
        }

        private static WorkflowDestinationBody ToDestinationBody(Destination destination)
        {
            return new WorkflowDestinationBody
            {
                Recipient = destination.Recipient,
                ToNumber = destination.ToNumber,
                MobilePhone = destination.MobilePhone,
                MainPhone = destination.MainPhone,
                FaxNumber = destination.FaxNumber,
                EmailAddress = destination.EmailAddress,
                Attention = destination.Attention,
                Company = destination.Company,
                FirstName = destination.FirstName,
                LastName = destination.LastName,
                Custom1 = destination.Custom1,
                Custom2 = destination.Custom2,
                Custom3 = destination.Custom3,
                Custom4 = destination.Custom4,
                Custom5 = destination.Custom5,
                Custom6 = destination.Custom6,
                Custom7 = destination.Custom7,
                Custom8 = destination.Custom8,
                Custom9 = destination.Custom9,
                ContactID = destination.ContactID,
                GroupID = destination.GroupID,
                GroupCode = destination.GroupCode
            };
        }

        // Used only by the named-param SendMessage/SendMessageAsync overloads (including
        // BuildOmniChannelRecipients' output below) to funnel Recipient-typed input through
        // Destinations instead of Recipients — the direct-model path (ToDestinationBody(Recipient)
        // above) is untouched. Unlike other modules' single-channel helpers, this maps onto
        // Destination's ToNumber/MainPhone/EmailAddress alternates (not the single primary Recipient
        // field) since Workflow's Recipient carries three independent channel fields simultaneously —
        // matching ToDestinationBody(Recipient)'s own field choices for the same conversion.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.MobileNumber, recipient.PhoneNumber, recipient.EmailAddress);
        }

        // toNumber/mainPhone/emailAddress combine into one Recipient per position when they carry the
        // same number of comma-separated values (a field with exactly one value broadcasts to every
        // position) — this preserves the single-recipient-with-multiple-channels shape for the common
        // "one value per field" case while still supporting independent bulk lists per field.
        private static IEnumerable<Recipient> BuildOmniChannelRecipients(List<string> toNumbers, List<string> mainPhones, List<string> emailAddresses)
        {
            var count = Math.Max(toNumbers.Count, Math.Max(mainPhones.Count, emailAddresses.Count));

            for (var i = 0; i < count; i++)
            {
                yield return new Recipient
                {
                    MobileNumber = toNumbers.Count == 1 ? toNumbers[0] : toNumbers.ElementAtOrDefault(i),
                    PhoneNumber = mainPhones.Count == 1 ? mainPhones[0] : mainPhones.ElementAtOrDefault(i),
                    EmailAddress = emailAddresses.Count == 1 ? emailAddresses[0] : emailAddresses.ElementAtOrDefault(i)
                };
            }
        }

        // A field with 0 or 1 values always lines up (broadcast or absent); two fields with >1 values
        // each must carry the SAME count, or BuildOmniChannelRecipients would silently pad the shorter
        // one with nulls instead of pairing them meaningfully.
        private static string? ValidateOmniChannelCounts(List<string> toNumbers, List<string> mainPhones, List<string> emailAddresses)
        {
            var distinctMultiValueCounts = new[] { toNumbers.Count, mainPhones.Count, emailAddresses.Count }
                .Where(count => count > 1)
                .Distinct()
                .Count();

            return distinctMultiValueCounts > 1
                ? "Mismatched recipient counts: toNumber, mainPhone, and emailAddress must each have either 0, 1, or the same number of comma-separated values."
                : null;
        }

        #region SendMessage
        public WorkflowApiResult SendMessage(WorkflowModel entity)
        {
            return Task.Run(() => SendMessageAsync(entity)).Result;
        }

        [ComVisible(false)]
        public WorkflowApiResult SendMessage(
            string? workflowTemplateId = null,
            object? destination = null,
            string? toNumber = null,
            string? mainPhone = null,
            string? emailAddress = null,
            string? contactID = null,
            string? groupID = null,
            ICollection<ContactID>? contactIDs = null,
            ICollection<GroupID>? groupIDs = null,
            ICollection<Recipient>? recipients = null,
            ICollection<Destination>? destinations = null,
            Enums.NotificationType? notificationType = null,
            Enums.SendModeType? sendMode = null
        )
        {
            var model = new WorkflowModel
            {
                WorkflowTemplateID = workflowTemplateId,
                NotificationType = notificationType,
                SendMode = sendMode ?? Enums.SendModeType.Live
            };

            var destinationList = new DestinationList();
            destinationList.Add(destination);

            var toNumbers = RecipientList.SplitAddresses(toNumber).ToList();
            var mainPhones = RecipientList.SplitAddresses(mainPhone).ToList();
            var emailAddresses = RecipientList.SplitAddresses(emailAddress).ToList();

            var mismatchError = ValidateOmniChannelCounts(toNumbers, mainPhones, emailAddresses);
            if (mismatchError is not null)
            {
                return ResultHelper.RespondError<WorkflowApiResult>(mismatchError);
            }

            foreach (var recipient in BuildOmniChannelRecipients(toNumbers, mainPhones, emailAddresses))
            {
                destinationList.Add(ToDestination(recipient));
            }
            foreach (var id in RecipientList.SplitAddresses(contactID))
            {
                destinationList.Add(new ContactID(id));
            }
            foreach (var id in RecipientList.SplitAddresses(groupID))
            {
                destinationList.Add(new GroupID(id));
            }
            destinationList.Add(contactIDs);
            destinationList.Add(groupIDs);
            destinationList.Add(recipients, ToDestination);
            destinationList.Add(destinations);
            model.Destinations = destinationList.ToList();

            return SendMessage(model);
        }

        [ComVisible(false)]
        public async Task<WorkflowApiResult> SendMessageAsync(WorkflowModel entity)
        {
            if (entity is null)
            {
                return ResultHelper.RespondError<WorkflowApiResult>("Empty entity: Please specify WorkflowModel");
            }

            var model = Mapper.Update(new WorkflowModel(), entity);

            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<WorkflowApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (string.IsNullOrEmpty(model.WorkflowTemplateID))
            {
                return ResultHelper.RespondError<WorkflowApiResult>("Empty WorkflowTemplateID: Please specify WorkflowTemplateID");
            }
            if ((model.Recipients is null || model.Recipients.Count == 0) && (model.Destinations is null || model.Destinations.Count == 0))
            {
                return ResultHelper.RespondError<WorkflowApiResult>("Empty recipient(s): Please add Recipient or Destination");
            }

            return await HttpRequest.PostAsync<WorkflowApiResult>(BuildAPIURL(), User, BuildRequestBody(model));
        }

        [ComVisible(false)]
        public async Task<WorkflowApiResult> SendMessageAsync(
            string? workflowTemplateId = null,
            object? destination = null,
            string? toNumber = null,
            string? mainPhone = null,
            string? emailAddress = null,
            string? contactID = null,
            string? groupID = null,
            ICollection<ContactID>? contactIDs = null,
            ICollection<GroupID>? groupIDs = null,
            ICollection<Recipient>? recipients = null,
            ICollection<Destination>? destinations = null,
            Enums.NotificationType? notificationType = null,
            Enums.SendModeType? sendMode = null
        )
        {
            var model = new WorkflowModel
            {
                WorkflowTemplateID = workflowTemplateId,
                NotificationType = notificationType,
                SendMode = sendMode ?? Enums.SendModeType.Live
            };

            var destinationList = new DestinationList();
            destinationList.Add(destination);

            var toNumbers = RecipientList.SplitAddresses(toNumber).ToList();
            var mainPhones = RecipientList.SplitAddresses(mainPhone).ToList();
            var emailAddresses = RecipientList.SplitAddresses(emailAddress).ToList();

            var mismatchError = ValidateOmniChannelCounts(toNumbers, mainPhones, emailAddresses);
            if (mismatchError is not null)
            {
                return ResultHelper.RespondError<WorkflowApiResult>(mismatchError);
            }

            foreach (var recipient in BuildOmniChannelRecipients(toNumbers, mainPhones, emailAddresses))
            {
                destinationList.Add(ToDestination(recipient));
            }
            foreach (var id in RecipientList.SplitAddresses(contactID))
            {
                destinationList.Add(new ContactID(id));
            }
            foreach (var id in RecipientList.SplitAddresses(groupID))
            {
                destinationList.Add(new GroupID(id));
            }
            destinationList.Add(contactIDs);
            destinationList.Add(groupIDs);
            destinationList.Add(recipients, ToDestination);
            destinationList.Add(destinations);
            model.Destinations = destinationList.ToList();

            return await SendMessageAsync(model);
        }
        #endregion
    }
}