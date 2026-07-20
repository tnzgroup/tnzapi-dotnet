using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.TTS
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class TTSApi : ITTSApi
    {
        private ITNZAuth User = new TNZApiUser();

        public TTSApi()
        {
        }

        public TTSApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        public TTSApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        public TTSApi(ITNZAuth auth)
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
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/tts";
        }

        private static TTSRequestBody BuildRequestBody(TTSModel entity)
        {
            return new TTSRequestBody
            {
                MessageID = entity.MessageID,
                Reference = entity.Reference,
                MessageToPeople = entity.MessageToPeople,
                TemplateID = entity.TemplateID,
                ContactID = entity.ContactID,
                GroupID = entity.GroupID,
                Destinations = (entity.Recipients?.Select(ToDestinationBody) ?? Enumerable.Empty<TTSDestinationBody>())
                    .Concat(entity.Destinations?.Select(ToDestinationBody) ?? Enumerable.Empty<TTSDestinationBody>())
                    .ToList(),
                NotificationType = entity.NotificationType,
                WebhookCallbackURL = entity.WebhookCallbackURL,
                WebhookCallbackFormat = entity.WebhookCallbackFormat,
                ReportTo = entity.ReportTo,
                SendTime = entity.SendTime,
                Timezone = entity.Timezone,
                SubAccount = entity.SubAccount,
                Department = entity.Department,
                ChargeCode = entity.ChargeCode,
                MessageToAnswerPhones = entity.MessageToAnswerPhones,
                AnswerPhoneMode = entity.AnswerPhoneMode,
                Keypads = entity.Keypads?.Select(ToKeypadBody).ToList() ?? new List<TTSKeypadBody>(),
                KeypadOptionRequired = entity.KeypadOptionRequired,
                CallRouteMessageOnWrongKey = entity.CallRouteMessageOnWrongKey,
                CallRouteMessageToPeople = entity.CallRouteMessageToPeople,
                CallRouteMessageToOperators = entity.CallRouteMessageToOperators,
                EndCallMessage = entity.EndCallMessage,
                NumberOfOperators = entity.NumberOfOperators,
                RetryAttempts = entity.RetryAttempts,
                RetryPeriod = entity.RetryPeriod,
                CallerID = entity.CallerID,
                Voice = entity.Voice,
                Options = entity.Options,
                Mode = entity.SendMode == Enums.SendModeType.Test ? "Test" : null
            };
        }

        private static TTSDestinationBody ToDestinationBody(Recipient recipient)
        {
            return new TTSDestinationBody
            {
                MainPhone = recipient.PhoneNumber,
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

        private static TTSDestinationBody ToDestinationBody(Destination destination)
        {
            return new TTSDestinationBody
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
        // (ToDestinationBody(Recipient) above) is untouched. PhoneNumber is TTS's channel field,
        // matching ToDestinationBody(Recipient)'s own choice for the same conversion.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.PhoneNumber);
        }

        private static TTSKeypadBody ToKeypadBody(KeypadModel keypad)
        {
            return new TTSKeypadBody
            {
                Tone = keypad.Tone,
                Play = keypad.Play,
                RouteNumber = keypad.RouteNumber,
                PlaySection = keypad.PlaySection,
                PlayFile = keypad.PlayFile,
                File = keypad.File
            };
        }

        #region SendMessage
        public TTSApiResult SendMessage(TTSModel entity)
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
        public TTSApiResult SendMessage(
            string? messageToPeople = null,
            object? destination = null,
            string? toNumber = null,
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
            var model = new TTSModel
            {
                MessageToPeople = messageToPeople,
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

            return SendMessage(model);
        }

        [ComVisible(false)]
        public async Task<TTSApiResult> SendMessageAsync(TTSModel entity)
        {
            if (entity is null)
            {
                return ResultHelper.RespondError<TTSApiResult>("Empty entity: Please specify TTSModel");
            }

            var model = Mapper.Update(new TTSModel(), entity);

            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<TTSApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if ((model.Recipients is null || model.Recipients.Count == 0) && (model.Destinations is null || model.Destinations.Count == 0))
            {
                return ResultHelper.RespondError<TTSApiResult>("Empty recipient(s): Please add Recipient or Destination");
            }
            if (string.IsNullOrEmpty(model.MessageToPeople) && string.IsNullOrEmpty(model.TemplateID))
            {
                return ResultHelper.RespondError<TTSApiResult>("Empty message: Please specify MessageToPeople or TemplateID");
            }

            return await HttpRequest.PostAsync<TTSApiResult>(BuildAPIURL(), User, BuildRequestBody(model));
        }

        [ComVisible(false)]
        public async Task<TTSApiResult> SendMessageAsync(
            string? messageToPeople = null,
            object? destination = null,
            string? toNumber = null,
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
            var model = new TTSModel
            {
                MessageToPeople = messageToPeople,
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

            return await SendMessageAsync(model);
        }
        #endregion

        private static string BuildStatusURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/tts/{Uri.EscapeDataString(messageID.Value!)}";
        }

        #region Status
        public TTSStatusApiResult Status(MessageID messageID)
        {
            return Task.Run(() => StatusAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<TTSStatusApiResult> StatusAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<TTSStatusApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<TTSStatusApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.GetAsync<TTSStatusApiResult>(BuildStatusURL(messageID), User);
        }
        #endregion

        private static string BuildRescheduleURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/tts/{Uri.EscapeDataString(messageID.Value!)}/reschedule";
        }

        private static string BuildAbortURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/tts/{Uri.EscapeDataString(messageID.Value!)}/abort";
        }

        private static string BuildResubmitURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/tts/{Uri.EscapeDataString(messageID.Value!)}/resubmit";
        }

        private static string BuildPacingURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/tts/{Uri.EscapeDataString(messageID.Value!)}/pacing";
        }

        #region Reschedule
        public TTSActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => RescheduleAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<TTSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<TTSActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<TTSActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<TTSActionApiResult>(BuildRescheduleURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion

        #region Abort
        public TTSActionApiResult Abort(MessageID messageID)
        {
            return Task.Run(() => AbortAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<TTSActionApiResult> AbortAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<TTSActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<TTSActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<TTSActionApiResult>(BuildAbortURL(messageID), User, new { });
        }
        #endregion

        #region Resubmit
        public TTSActionApiResult Resubmit(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => ResubmitAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<TTSActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<TTSActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<TTSActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<TTSActionApiResult>(BuildResubmitURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion

        #region Pacing
        public TTSActionApiResult Pacing(MessageID messageID, int numberOfOperators)
        {
            return Task.Run(() => PacingAsync(messageID, numberOfOperators)).Result;
        }

        [ComVisible(false)]
        public async Task<TTSActionApiResult> PacingAsync(MessageID messageID, int numberOfOperators)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<TTSActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<TTSActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<TTSActionApiResult>(BuildPacingURL(messageID), User, new { NumberOfOperators = numberOfOperators });
        }
        #endregion
    }
}