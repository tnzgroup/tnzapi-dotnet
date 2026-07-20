using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.Email
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class EmailApi : IEmailApi
    {
        private ITNZAuth User = new TNZApiUser();

        public EmailApi()
        {
        }

        public EmailApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        public EmailApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        public EmailApi(ITNZAuth auth)
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
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/email";
        }

        private static EmailRequestBody BuildRequestBody(EmailModel entity)
        {
            return new EmailRequestBody
            {
                MessageID = entity.MessageID,
                Reference = entity.Reference,
                MessagePlain = entity.MessagePlain,
                MessageHTML = entity.MessageHTML,
                TemplateID = entity.TemplateID,
                ContactID = entity.ContactID,
                GroupID = entity.GroupID,
                Destinations = (entity.Recipients?.Select(ToDestinationBody) ?? Enumerable.Empty<EmailDestinationBody>())
                    .Concat(entity.Destinations?.Select(ToDestinationBody) ?? Enumerable.Empty<EmailDestinationBody>())
                    .ToList(),
                Files = entity.Files?.Select(ToFileBody).ToList() ?? new List<EmailFileBody>(),
                SMTPFrom = entity.SMTPFrom,
                From = entity.From,
                FromEmail = entity.FromEmail,
                CCEmail = entity.CCEmail,
                ReplyTo = entity.ReplyTo,
                EmailSubject = entity.EmailSubject,
                NotificationType = entity.NotificationType,
                WebhookCallbackURL = entity.WebhookCallbackURL,
                WebhookCallbackFormat = entity.WebhookCallbackFormat,
                ReportTo = entity.ReportTo,
                SendTime = entity.SendTime,
                Timezone = entity.Timezone,
                SubAccount = entity.SubAccount,
                Department = entity.Department,
                ChargeCode = entity.ChargeCode,
                Mode = entity.SendMode == Enums.SendModeType.Test ? "Test" : null
            };
        }

        private static EmailDestinationBody ToDestinationBody(Recipient recipient)
        {
            return new EmailDestinationBody
            {
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

        private static EmailDestinationBody ToDestinationBody(Destination destination)
        {
            return new EmailDestinationBody
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

        // Used only by the named-param SendMessage/SendMessageAsync overloads to funnel
        // Recipient-typed input through Destinations instead of Recipients — the direct-model path
        // (ToDestinationBody(Recipient) above) is untouched. EmailAddress is Email's channel field,
        // matching ToDestinationBody(Recipient)'s own choice for the same conversion.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.EmailAddress);
        }

        private static EmailFileBody ToFileBody(Attachment attachment)
        {
            return new EmailFileBody
            {
                Name = attachment.FileName,
                Data = attachment.FileContent
            };
        }

        #region SendMessage
        public EmailApiResult SendMessage(EmailModel entity)
        {
            return Task.Run(() => SendMessageAsync(entity)).Result;
        }

        [ComVisible(false)]
        public EmailApiResult SendMessage(
            string? messagePlain = null,
            string? messageHTML = null,
            object? destination = null,
            string? emailAddress = null,
            string? contactID = null,
            string? groupID = null,
            ICollection<ContactID>? contactIDs = null,
            ICollection<GroupID>? groupIDs = null,
            ICollection<Recipient>? recipients = null,
            ICollection<Destination>? destinations = null,
            string? emailSubject = null,
            string? fromEmail = null,
            string? file = null,
            ICollection<Attachment>? attachments = null,
            Enums.NotificationType? notificationType = null,
            Enums.SendModeType? sendMode = null
        )
        {
            var model = new EmailModel
            {
                MessagePlain = messagePlain,
                MessageHTML = messageHTML,
                EmailSubject = emailSubject,
                FromEmail = fromEmail,
                NotificationType = notificationType,
                SendMode = sendMode ?? Enums.SendModeType.Live
            };

            var destinationList = new DestinationList();
            destinationList.Add(destination);
            foreach (var address in RecipientList.SplitAddresses(emailAddress))
            {
                destinationList.Add(new Destination { Recipient = address });
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

            var attachmentList = new AttachmentList();
            attachmentList.Add(file);
            attachmentList.Add(attachments);
            model.Files = attachmentList.ToList();

            return SendMessage(model);
        }

        [ComVisible(false)]
        public async Task<EmailApiResult> SendMessageAsync(EmailModel entity)
        {
            if (entity is null)
            {
                return ResultHelper.RespondError<EmailApiResult>("Empty entity: Please specify EmailModel");
            }

            var model = Mapper.Update(new EmailModel(), entity);

            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<EmailApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if ((model.Recipients is null || model.Recipients.Count == 0) && (model.Destinations is null || model.Destinations.Count == 0))
            {
                return ResultHelper.RespondError<EmailApiResult>("Empty recipient(s): Please add Recipient or Destination");
            }
            if (string.IsNullOrEmpty(model.MessagePlain) && string.IsNullOrEmpty(model.MessageHTML) && string.IsNullOrEmpty(model.TemplateID))
            {
                return ResultHelper.RespondError<EmailApiResult>("Empty message: Please specify MessagePlain, MessageHTML, or TemplateID");
            }

            return await HttpRequest.PostAsync<EmailApiResult>(BuildAPIURL(), User, BuildRequestBody(model));
        }

        [ComVisible(false)]
        public async Task<EmailApiResult> SendMessageAsync(
            string? messagePlain = null,
            string? messageHTML = null,
            object? destination = null,
            string? emailAddress = null,
            string? contactID = null,
            string? groupID = null,
            ICollection<ContactID>? contactIDs = null,
            ICollection<GroupID>? groupIDs = null,
            ICollection<Recipient>? recipients = null,
            ICollection<Destination>? destinations = null,
            string? emailSubject = null,
            string? fromEmail = null,
            string? file = null,
            ICollection<Attachment>? attachments = null,
            Enums.NotificationType? notificationType = null,
            Enums.SendModeType? sendMode = null
        )
        {
            var model = new EmailModel
            {
                MessagePlain = messagePlain,
                MessageHTML = messageHTML,
                EmailSubject = emailSubject,
                FromEmail = fromEmail,
                NotificationType = notificationType,
                SendMode = sendMode ?? Enums.SendModeType.Live
            };

            var destinationList = new DestinationList();
            destinationList.Add(destination);
            foreach (var address in RecipientList.SplitAddresses(emailAddress))
            {
                destinationList.Add(new Destination { Recipient = address });
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

            var attachmentList = new AttachmentList();
            attachmentList.Add(file);
            attachmentList.Add(attachments);
            model.Files = await attachmentList.ToListAsync();

            return await SendMessageAsync(model);
        }
        #endregion

        private static string BuildStatusURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/email/{Uri.EscapeDataString(messageID.Value!)}";
        }

        #region Status
        public EmailStatusApiResult Status(MessageID messageID)
        {
            return Task.Run(() => StatusAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<EmailStatusApiResult> StatusAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<EmailStatusApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<EmailStatusApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.GetAsync<EmailStatusApiResult>(BuildStatusURL(messageID), User);
        }
        #endregion

        private static string BuildRescheduleURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/email/{Uri.EscapeDataString(messageID.Value!)}/reschedule";
        }

        private static string BuildAbortURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/email/{Uri.EscapeDataString(messageID.Value!)}/abort";
        }

        private static string BuildResubmitURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/email/{Uri.EscapeDataString(messageID.Value!)}/resubmit";
        }

        #region Reschedule
        public EmailActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => RescheduleAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<EmailActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<EmailActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<EmailActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<EmailActionApiResult>(BuildRescheduleURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion

        #region Abort
        public EmailActionApiResult Abort(MessageID messageID)
        {
            return Task.Run(() => AbortAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<EmailActionApiResult> AbortAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<EmailActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<EmailActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<EmailActionApiResult>(BuildAbortURL(messageID), User, new { });
        }
        #endregion

        #region Resubmit
        public EmailActionApiResult Resubmit(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => ResubmitAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<EmailActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<EmailActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<EmailActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<EmailActionApiResult>(BuildResubmitURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion
    }
}