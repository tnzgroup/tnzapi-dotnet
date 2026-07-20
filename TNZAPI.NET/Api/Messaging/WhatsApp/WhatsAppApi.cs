using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.WhatsApp.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.WhatsApp
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class WhatsAppApi : IWhatsAppApi
    {
        private ITNZAuth User = new TNZApiUser();

        public WhatsAppApi()
        {
        }

        public WhatsAppApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        public WhatsAppApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        public WhatsAppApi(ITNZAuth auth)
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
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/whatsapp";
        }

        private static WhatsAppRequestBody BuildRequestBody(WhatsAppModel entity)
        {
            return new WhatsAppRequestBody
            {
                MessageID = entity.MessageID,
                Reference = entity.Reference,
                Message = entity.Message,
                TemplateID = entity.TemplateID,
                FallbackMode = EnumListHelper.ToCommaSeparatedString(entity.FallbackMode, EnumListHelper.ToFallbackModeWireToken),
                FromNumber = entity.FromNumber,
                ContactID = entity.ContactID,
                GroupID = entity.GroupID,
                Destinations = (entity.Recipients?.Select(ToDestinationBody) ?? Enumerable.Empty<WhatsAppDestinationBody>())
                    .Concat(entity.Destinations?.Select(ToDestinationBody) ?? Enumerable.Empty<WhatsAppDestinationBody>())
                    .ToList(),
                Files = entity.Files?.Select(ToFileBody).ToList() ?? new List<WhatsAppFileBody>(),
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

        private static WhatsAppDestinationBody ToDestinationBody(Recipient recipient)
        {
            return new WhatsAppDestinationBody
            {
                ToNumber = recipient.MobileNumber,
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

        private static WhatsAppDestinationBody ToDestinationBody(Destination destination)
        {
            return new WhatsAppDestinationBody
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
        // (ToDestinationBody(Recipient) above) is untouched. MobileNumber is WhatsApp's channel
        // field, matching ToDestinationBody(Recipient)'s own choice for the same conversion.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.MobileNumber);
        }

        private static WhatsAppFileBody ToFileBody(Attachment attachment)
        {
            return new WhatsAppFileBody
            {
                Name = attachment.FileName,
                Data = attachment.FileContent
            };
        }

        #region SendMessage
        public WhatsAppApiResult SendMessage(WhatsAppModel entity)
        {
            return Task.Run(() => SendMessageAsync(entity)).Result;
        }

        [ComVisible(false)]
        public WhatsAppApiResult SendMessage(
            string? messageText = null,
            string? templateId = null,
            object? destination = null,
            string? toNumber = null,
            string? contactID = null,
            string? groupID = null,
            ICollection<ContactID>? contactIDs = null,
            ICollection<GroupID>? groupIDs = null,
            ICollection<Recipient>? recipients = null,
            ICollection<Destination>? destinations = null,
            string? file = null,
            ICollection<Attachment>? attachments = null,
            Enums.NotificationType? notificationType = null,
            Enums.SendModeType? sendMode = null
        )
        {
            var model = new WhatsAppModel
            {
                Message = messageText,
                TemplateID = templateId,
                NotificationType = notificationType,
                SendMode = sendMode ?? Enums.SendModeType.Live
            };

            var destinationList = new DestinationList();
            destinationList.Add(destination);
            foreach (var number in RecipientList.SplitAddresses(toNumber))
            {
                destinationList.Add(new Destination { Recipient = number });
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
        public async Task<WhatsAppApiResult> SendMessageAsync(WhatsAppModel entity)
        {
            if (entity is null)
            {
                return ResultHelper.RespondError<WhatsAppApiResult>("Empty entity: Please specify WhatsAppModel");
            }

            var model = Mapper.Update(new WhatsAppModel(), entity);

            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<WhatsAppApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if ((model.Recipients is null || model.Recipients.Count == 0) && (model.Destinations is null || model.Destinations.Count == 0))
            {
                return ResultHelper.RespondError<WhatsAppApiResult>("Empty recipient(s): Please add Recipient or Destination");
            }
            if (string.IsNullOrEmpty(model.TemplateID))
            {
                return ResultHelper.RespondError<WhatsAppApiResult>("Empty TemplateID: Please specify TemplateID");
            }
            if (string.IsNullOrEmpty(model.Message))
            {
                return ResultHelper.RespondError<WhatsAppApiResult>("Empty message: Please specify Message");
            }

            return await HttpRequest.PostAsync<WhatsAppApiResult>(BuildAPIURL(), User, BuildRequestBody(model));
        }

        [ComVisible(false)]
        public async Task<WhatsAppApiResult> SendMessageAsync(
            string? messageText = null,
            string? templateId = null,
            object? destination = null,
            string? toNumber = null,
            string? contactID = null,
            string? groupID = null,
            ICollection<ContactID>? contactIDs = null,
            ICollection<GroupID>? groupIDs = null,
            ICollection<Recipient>? recipients = null,
            ICollection<Destination>? destinations = null,
            string? file = null,
            ICollection<Attachment>? attachments = null,
            Enums.NotificationType? notificationType = null,
            Enums.SendModeType? sendMode = null
        )
        {
            var model = new WhatsAppModel
            {
                Message = messageText,
                TemplateID = templateId,
                NotificationType = notificationType,
                SendMode = sendMode ?? Enums.SendModeType.Live
            };

            var destinationList = new DestinationList();
            destinationList.Add(destination);
            foreach (var number in RecipientList.SplitAddresses(toNumber))
            {
                destinationList.Add(new Destination { Recipient = number });
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

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/whatsapp/{Uri.EscapeDataString(messageID.Value!)}";
        }

        #region Status
        public WhatsAppStatusApiResult Status(MessageID messageID)
        {
            return Task.Run(() => StatusAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<WhatsAppStatusApiResult> StatusAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<WhatsAppStatusApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<WhatsAppStatusApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.GetAsync<WhatsAppStatusApiResult>(BuildStatusURL(messageID), User);
        }
        #endregion

        private static string BuildReceivedURL(int timePeriod, int recordsPerPage, int page)
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/whatsapp/received?timePeriod={timePeriod}&recordsPerPage={recordsPerPage}&page={page}";
        }

        private static string BuildReceivedURL(DateTime dateFrom, DateTime dateTo, int recordsPerPage, int page)
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/whatsapp/received?dateFrom={Uri.EscapeDataString(dateFrom.ToString("yyyy-MM-dd HH:mm:ss"))}&dateTo={Uri.EscapeDataString(dateTo.ToString("yyyy-MM-dd HH:mm:ss"))}&recordsPerPage={recordsPerPage}&page={page}";
        }

        #region Received
        public WhatsAppReceivedApiResult Received(int timePeriod, int recordsPerPage = 100, int page = 1)
        {
            return Task.Run(() => ReceivedAsync(timePeriod, recordsPerPage, page)).Result;
        }

        [ComVisible(false)]
        public WhatsAppReceivedApiResult Received(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1)
        {
            return Task.Run(() => ReceivedAsync(dateFrom, dateTo, recordsPerPage, page)).Result;
        }

        [ComVisible(false)]
        public async Task<WhatsAppReceivedApiResult> ReceivedAsync(int timePeriod, int recordsPerPage = 100, int page = 1)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<WhatsAppReceivedApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (timePeriod < 1 || timePeriod > 1440)
            {
                return ResultHelper.RespondError<WhatsAppReceivedApiResult>("Invalid timePeriod: Must be between 1 and 1440 minutes");
            }

            return await HttpRequest.GetAsync<WhatsAppReceivedApiResult>(BuildReceivedURL(timePeriod, recordsPerPage, page), User);
        }

        // dateTo defaults to DateTime.Now (not UtcNow) deliberately — see the equivalent comment on
        // SMSApi.ReceivedAsync(DateTime, ...) for the reasoning (account-timezone conversion is the
        // backend's responsibility, matching how an explicitly-supplied dateFrom is never converted).
        [ComVisible(false)]
        public async Task<WhatsAppReceivedApiResult> ReceivedAsync(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<WhatsAppReceivedApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            var effectiveDateTo = dateTo ?? DateTime.Now;
            if (dateFrom > effectiveDateTo)
            {
                return ResultHelper.RespondError<WhatsAppReceivedApiResult>("Invalid date range: dateFrom must not be after dateTo");
            }

            return await HttpRequest.GetAsync<WhatsAppReceivedApiResult>(BuildReceivedURL(dateFrom, effectiveDateTo, recordsPerPage, page), User);
        }
        #endregion

        private static string BuildRescheduleURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/whatsapp/{Uri.EscapeDataString(messageID.Value!)}/reschedule";
        }

        private static string BuildAbortURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/whatsapp/{Uri.EscapeDataString(messageID.Value!)}/abort";
        }

        #region Reschedule
        public WhatsAppActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => RescheduleAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<WhatsAppActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<WhatsAppActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<WhatsAppActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<WhatsAppActionApiResult>(BuildRescheduleURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion

        #region Abort
        public WhatsAppActionApiResult Abort(MessageID messageID)
        {
            return Task.Run(() => AbortAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<WhatsAppActionApiResult> AbortAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<WhatsAppActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<WhatsAppActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<WhatsAppActionApiResult>(BuildAbortURL(messageID), User, new { });
        }
        #endregion
    }
}