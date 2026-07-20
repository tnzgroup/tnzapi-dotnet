using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.Voice
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class VoiceApi : IVoiceApi
    {
        private ITNZAuth User = new TNZApiUser();

        public VoiceApi()
        {
        }

        public VoiceApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        public VoiceApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        public VoiceApi(ITNZAuth auth)
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
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/voice";
        }

        private static VoiceRequestBody BuildRequestBody(VoiceModel entity)
        {
            return new VoiceRequestBody
            {
                MessageID = entity.MessageID,
                Reference = entity.Reference,
                MessageToPeople = entity.MessageToPeople,
                TemplateID = entity.TemplateID,
                ContactID = entity.ContactID,
                GroupID = entity.GroupID,
                Destinations = (entity.Recipients?.Select(ToDestinationBody) ?? Enumerable.Empty<VoiceDestinationBody>())
                    .Concat(entity.Destinations?.Select(ToDestinationBody) ?? Enumerable.Empty<VoiceDestinationBody>())
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
                Keypads = entity.Keypads?.Select(ToKeypadBody).ToList() ?? new List<VoiceKeypadBody>(),
                VoiceFiles = entity.VoiceFiles?.Select(ToVoiceFileBody).ToList() ?? new List<VoiceFileBody>(),
                KeypadOptionRequired = entity.KeypadOptionRequired,
                CallRouteMessageOnWrongKey = entity.CallRouteMessageOnWrongKey,
                CallRouteMessageToPeople = entity.CallRouteMessageToPeople,
                CallRouteMessageToOperators = entity.CallRouteMessageToOperators,
                EndCallMessage = entity.EndCallMessage,
                NumberOfOperators = entity.NumberOfOperators,
                RetryAttempts = entity.RetryAttempts,
                RetryPeriod = entity.RetryPeriod,
                CallerID = entity.CallerID,
                Options = entity.Options,
                Mode = entity.SendMode == Enums.SendModeType.Test ? "Test" : null
            };
        }

        private static VoiceDestinationBody ToDestinationBody(Recipient recipient)
        {
            return new VoiceDestinationBody
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

        private static VoiceDestinationBody ToDestinationBody(Destination destination)
        {
            return new VoiceDestinationBody
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
        // (ToDestinationBody(Recipient) above) is untouched. PhoneNumber is Voice's channel field,
        // matching ToDestinationBody(Recipient)'s own choice for the same conversion.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.PhoneNumber);
        }

        private static VoiceKeypadBody ToKeypadBody(KeypadModel keypad)
        {
            return new VoiceKeypadBody
            {
                Tone = keypad.Tone,
                Play = keypad.Play,
                RouteNumber = keypad.RouteNumber,
                PlaySection = keypad.PlaySection,
                PlayFile = keypad.PlayFile,
                File = keypad.File
            };
        }

        private static VoiceFileBody ToVoiceFileBody(VoiceFileModel file)
        {
            return new VoiceFileBody
            {
                Name = file.Name,
                File = file.File
            };
        }

        #region SendMessage
        public VoiceApiResult SendMessage(VoiceModel entity)
        {
            return Task.Run(() => SendMessageAsync(entity)).Result;
        }

        [ComVisible(false)]
        public VoiceApiResult SendMessage(
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
            var model = new VoiceModel
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
        public async Task<VoiceApiResult> SendMessageAsync(VoiceModel entity)
        {
            if (entity is null)
            {
                return ResultHelper.RespondError<VoiceApiResult>("Empty entity: Please specify VoiceModel");
            }

            var model = Mapper.Update(new VoiceModel(), entity);

            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<VoiceApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if ((model.Recipients is null || model.Recipients.Count == 0) && (model.Destinations is null || model.Destinations.Count == 0))
            {
                return ResultHelper.RespondError<VoiceApiResult>("Empty recipient(s): Please add Recipient or Destination");
            }
            if (string.IsNullOrEmpty(model.MessageToPeople) && string.IsNullOrEmpty(model.TemplateID))
            {
                return ResultHelper.RespondError<VoiceApiResult>("Empty message: Please specify MessageToPeople or TemplateID");
            }

            return await HttpRequest.PostAsync<VoiceApiResult>(BuildAPIURL(), User, BuildRequestBody(model));
        }

        [ComVisible(false)]
        public async Task<VoiceApiResult> SendMessageAsync(
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
            var model = new VoiceModel
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

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/voice/{Uri.EscapeDataString(messageID.Value!)}";
        }

        #region Status
        public VoiceStatusApiResult Status(MessageID messageID)
        {
            return Task.Run(() => StatusAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<VoiceStatusApiResult> StatusAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<VoiceStatusApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<VoiceStatusApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.GetAsync<VoiceStatusApiResult>(BuildStatusURL(messageID), User);
        }
        #endregion

        private static string BuildRescheduleURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/voice/{Uri.EscapeDataString(messageID.Value!)}/reschedule";
        }

        private static string BuildAbortURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/voice/{Uri.EscapeDataString(messageID.Value!)}/abort";
        }

        private static string BuildResubmitURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/voice/{Uri.EscapeDataString(messageID.Value!)}/resubmit";
        }

        private static string BuildPacingURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/voice/{Uri.EscapeDataString(messageID.Value!)}/pacing";
        }

        #region Reschedule
        public VoiceActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => RescheduleAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<VoiceActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<VoiceActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<VoiceActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<VoiceActionApiResult>(BuildRescheduleURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion

        #region Abort
        public VoiceActionApiResult Abort(MessageID messageID)
        {
            return Task.Run(() => AbortAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<VoiceActionApiResult> AbortAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<VoiceActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<VoiceActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<VoiceActionApiResult>(BuildAbortURL(messageID), User, new { });
        }
        #endregion

        #region Resubmit
        public VoiceActionApiResult Resubmit(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => ResubmitAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<VoiceActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<VoiceActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<VoiceActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<VoiceActionApiResult>(BuildResubmitURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion

        #region Pacing
        public VoiceActionApiResult Pacing(MessageID messageID, int numberOfOperators)
        {
            return Task.Run(() => PacingAsync(messageID, numberOfOperators)).Result;
        }

        [ComVisible(false)]
        public async Task<VoiceActionApiResult> PacingAsync(MessageID messageID, int numberOfOperators)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<VoiceActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<VoiceActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<VoiceActionApiResult>(BuildPacingURL(messageID), User, new { NumberOfOperators = numberOfOperators });
        }
        #endregion
    }
}