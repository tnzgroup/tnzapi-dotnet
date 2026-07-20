using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.SMS
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class SMSApi : ISMSApi
    {
        private ITNZAuth User = new TNZApiUser();

        public SMSApi()
        {
        }

        public SMSApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        public SMSApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        public SMSApi(ITNZAuth auth)
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
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/sms";
        }

        private static SMSRequestBody BuildRequestBody(SMSModel entity)
        {
            return new SMSRequestBody
            {
                MessageID = entity.MessageID,
                Reference = entity.Reference,
                Message = entity.Message,
                TemplateID = entity.TemplateID,
                ContactID = entity.ContactID,
                GroupID = entity.GroupID,
                Destinations = (entity.Recipients?.Select(ToDestinationBody) ?? Enumerable.Empty<SMSDestinationBody>())
                    .Concat(entity.Destinations?.Select(ToDestinationBody) ?? Enumerable.Empty<SMSDestinationBody>())
                    .ToList(),
                Files = entity.Files?.Select(ToFileBody).ToList() ?? new List<SMSFileBody>(),
                NotificationType = entity.NotificationType,
                WebhookCallbackURL = entity.WebhookCallbackURL,
                WebhookCallbackFormat = entity.WebhookCallbackFormat,
                ReportTo = entity.ReportTo,
                SendTime = entity.SendTime,
                Timezone = entity.Timezone,
                SubAccount = entity.SubAccount,
                Department = entity.Department,
                ChargeCode = entity.ChargeCode,
                FromNumber = entity.FromNumber,
                SMSEmailReply = entity.SMSEmailReply,
                CharacterConversion = entity.CharacterConversion,
                FallbackMode = EnumListHelper.ToCommaSeparatedString(entity.FallbackMode, EnumListHelper.ToFallbackModeWireToken),
                Mode = entity.SendMode == Enums.SendModeType.Test ? "Test" : null
            };
        }

        private static SMSDestinationBody ToDestinationBody(Recipient recipient)
        {
            return new SMSDestinationBody
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

        private static SMSDestinationBody ToDestinationBody(Destination destination)
        {
            return new SMSDestinationBody
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

        // Used only by the named-parameter SendMessage(...)/SendMessageAsync(...) overloads below, to
        // convert an incoming Recipient (from the recipients collection param) into a Destination
        // sending the wire's primary "Recipient" field instead of the legacy per-channel alternate
        // (ToNumber). MobileNumber is SMS's channel field, matching ToDestinationBody(Recipient)
        // above.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.MobileNumber);
        }

        private static SMSFileBody ToFileBody(Attachment attachment)
        {
            return new SMSFileBody
            {
                Name = attachment.FileName,
                Data = attachment.FileContent
            };
        }

        #region SendMessage
        public SMSApiResult SendMessage(SMSModel entity)
        {
            return Task.Run(() => SendMessageAsync(entity)).Result;
        }

        // destination accepts either a string (a bare address, same as toNumber) or a Destination
        // object (the full wire-accurate shape) — kept as one parameter name/position for ergonomics
        // rather than a second overload, since two overloads differing only in this one parameter's
        // type are ambiguous for any call that omits it entirely: the compiler cannot decide which
        // overload's default value to use for an omitted argument, because neither type is more
        // specific than the other. A single object? parameter sidesteps the ambiguity entirely.
        // [ComVisible(false)] since object isn't a COM-friendly parameter type.
        [ComVisible(false)]
        public SMSApiResult SendMessage(
            string? messageText = null,
            string? reference = null,
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
            var model = new SMSModel
            {
                Message = messageText,
                Reference = reference,
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
        public async Task<SMSApiResult> SendMessageAsync(SMSModel entity)
        {
            if (entity is null)
            {
                return ResultHelper.RespondError<SMSApiResult>("Empty entity: Please specify SMSModel");
            }

            var model = Mapper.Update(new SMSModel(), entity);

            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<SMSApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if ((model.Recipients is null || model.Recipients.Count == 0) && (model.Destinations is null || model.Destinations.Count == 0))
            {
                return ResultHelper.RespondError<SMSApiResult>("Empty recipient(s): Please add Recipient or Destination");
            }
            if (string.IsNullOrEmpty(model.Message))
            {
                return ResultHelper.RespondError<SMSApiResult>("Empty message_text: Please specify MessageText");
            }

            return await HttpRequest.PostAsync<SMSApiResult>(BuildAPIURL(), User, BuildRequestBody(model));
        }

        [ComVisible(false)]
        public async Task<SMSApiResult> SendMessageAsync(
            string? messageText = null,
            string? reference = null,
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
            var model = new SMSModel
            {
                Message = messageText,
                Reference = reference,
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

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/sms/{Uri.EscapeDataString(messageID.Value!)}";
        }

        #region Status
        public SMSStatusApiResult Status(MessageID messageID)
        {
            return Task.Run(() => StatusAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<SMSStatusApiResult> StatusAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<SMSStatusApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<SMSStatusApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.GetAsync<SMSStatusApiResult>(BuildStatusURL(messageID), User);
        }
        #endregion

        private static string BuildReceivedURL(int timePeriod, int recordsPerPage, int page)
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/sms/received?timePeriod={timePeriod}&recordsPerPage={recordsPerPage}&page={page}";
        }

        private static string BuildReceivedURL(DateTime dateFrom, DateTime dateTo, int recordsPerPage, int page)
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/sms/received?dateFrom={Uri.EscapeDataString(dateFrom.ToString("yyyy-MM-dd HH:mm:ss"))}&dateTo={Uri.EscapeDataString(dateTo.ToString("yyyy-MM-dd HH:mm:ss"))}&recordsPerPage={recordsPerPage}&page={page}";
        }

        #region Received
        public SMSReceivedApiResult Received(int timePeriod, int recordsPerPage = 100, int page = 1)
        {
            return Task.Run(() => ReceivedAsync(timePeriod, recordsPerPage, page)).Result;
        }

        [ComVisible(false)]
        public SMSReceivedApiResult Received(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1)
        {
            return Task.Run(() => ReceivedAsync(dateFrom, dateTo, recordsPerPage, page)).Result;
        }

        [ComVisible(false)]
        public async Task<SMSReceivedApiResult> ReceivedAsync(int timePeriod, int recordsPerPage = 100, int page = 1)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (timePeriod < 1 || timePeriod > 1440)
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>("Invalid timePeriod: Must be between 1 and 1440 minutes");
            }

            return await HttpRequest.GetAsync<SMSReceivedApiResult>(BuildReceivedURL(timePeriod, recordsPerPage, page), User);
        }

        // dateTo defaults to DateTime.Now (not UtcNow) deliberately — dateFrom/dateTo are sent as
        // naive, unzoned timestamps with no accompanying Timezone field (unlike SendTime elsewhere in
        // this SDK), and the TNZ backend interprets them per the account's own configured timezone
        // setting, converting to UTC server-side. Defaulting dateTo to UTC here would be inconsistent
        // with that convention and with an explicitly-supplied dateFrom, which is never converted.
        [ComVisible(false)]
        public async Task<SMSReceivedApiResult> ReceivedAsync(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            var effectiveDateTo = dateTo ?? DateTime.Now;
            if (dateFrom > effectiveDateTo)
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>("Invalid date range: dateFrom must not be after dateTo");
            }

            return await HttpRequest.GetAsync<SMSReceivedApiResult>(BuildReceivedURL(dateFrom, effectiveDateTo, recordsPerPage, page), User);
        }
        #endregion

        private static string BuildSMSReplyURL(MessageID messageID, int recordsPerPage, int page)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/sms/{Uri.EscapeDataString(messageID.Value!)}?recordsPerPage={recordsPerPage}&page={page}";
        }

        #region SMSReply
        public SMSStatusApiResult SMSReply(MessageID messageID, int recordsPerPage = 100, int page = 1)
        {
            return Task.Run(() => SMSReplyAsync(messageID, recordsPerPage, page)).Result;
        }

        [ComVisible(false)]
        public async Task<SMSStatusApiResult> SMSReplyAsync(MessageID messageID, int recordsPerPage = 100, int page = 1)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<SMSStatusApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<SMSStatusApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.GetAsync<SMSStatusApiResult>(BuildSMSReplyURL(messageID, recordsPerPage, page), User);
        }
        #endregion

        private static string BuildRescheduleURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/sms/{Uri.EscapeDataString(messageID.Value!)}/reschedule";
        }

        private static string BuildAbortURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/sms/{Uri.EscapeDataString(messageID.Value!)}/abort";
        }

        #region Reschedule
        public SMSActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => RescheduleAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<SMSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<SMSActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<SMSActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<SMSActionApiResult>(BuildRescheduleURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion

        #region Abort
        public SMSActionApiResult Abort(MessageID messageID)
        {
            return Task.Run(() => AbortAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<SMSActionApiResult> AbortAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<SMSActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<SMSActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<SMSActionApiResult>(BuildAbortURL(messageID), User, new { });
        }
        #endregion
    }
}