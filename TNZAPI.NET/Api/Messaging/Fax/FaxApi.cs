using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Messaging;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.Fax
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class FaxApi : IFaxApi
    {
        private ITNZAuth User = new TNZApiUser();

        public FaxApi()
        {
        }

        public FaxApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        public FaxApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        public FaxApi(ITNZAuth auth)
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
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/fax";
        }

        private static FaxRequestBody BuildRequestBody(FaxModel entity)
        {
            return new FaxRequestBody
            {
                MessageID = entity.MessageID,
                Reference = entity.Reference,
                TemplateID = entity.TemplateID,
                ContactID = entity.ContactID,
                GroupID = entity.GroupID,
                Destinations = (entity.Recipients?.Select(ToDestinationBody) ?? Enumerable.Empty<FaxDestinationBody>())
                    .Concat(entity.Destinations?.Select(ToDestinationBody) ?? Enumerable.Empty<FaxDestinationBody>())
                    .ToList(),
                Files = entity.Files?.Select(ToFileBody).ToList() ?? new List<FaxFileBody>(),
                NotificationType = entity.NotificationType,
                WebhookCallbackURL = entity.WebhookCallbackURL,
                WebhookCallbackFormat = entity.WebhookCallbackFormat,
                ReportTo = entity.ReportTo,
                SendTime = entity.SendTime,
                Timezone = entity.Timezone,
                SubAccount = entity.SubAccount,
                Department = entity.Department,
                ChargeCode = entity.ChargeCode,
                CSID = entity.CSID,
                Resolution = entity.Resolution,
                WatermarkFolder = entity.WatermarkFolder,
                WatermarkFirstPage = entity.WatermarkFirstPage,
                WatermarkAllPages = entity.WatermarkAllPages,
                RetryAttempts = entity.RetryAttempts,
                RetryPeriod = entity.RetryPeriod,
                Mode = entity.SendMode == Enums.SendModeType.Test ? "Test" : null
            };
        }

        private static FaxDestinationBody ToDestinationBody(Recipient recipient)
        {
            return new FaxDestinationBody
            {
                ToNumber = recipient.FaxNumber,
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

        private static FaxDestinationBody ToDestinationBody(Destination destination)
        {
            return new FaxDestinationBody
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
        // (ToDestinationBody(Recipient) above) is untouched. FaxNumber is Fax's channel field,
        // matching ToDestinationBody(Recipient)'s own choice for the same conversion.
        private static Destination ToDestination(Recipient recipient)
        {
            return DestinationMapper.FromRecipient(recipient, recipient.FaxNumber);
        }

        private static FaxFileBody ToFileBody(Attachment attachment)
        {
            return new FaxFileBody
            {
                Name = attachment.FileName,
                Data = attachment.FileContent
            };
        }

        #region SendMessage
        public FaxApiResult SendMessage(FaxModel entity)
        {
            return Task.Run(() => SendMessageAsync(entity)).Result;
        }

        [ComVisible(false)]
        public FaxApiResult SendMessage(
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
            var model = new FaxModel
            {
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
        public async Task<FaxApiResult> SendMessageAsync(FaxModel entity)
        {
            if (entity is null)
            {
                return ResultHelper.RespondError<FaxApiResult>("Empty entity: Please specify FaxModel");
            }

            var model = Mapper.Update(new FaxModel(), entity);

            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<FaxApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if ((model.Recipients is null || model.Recipients.Count == 0) && (model.Destinations is null || model.Destinations.Count == 0))
            {
                return ResultHelper.RespondError<FaxApiResult>("Empty recipient(s): Please add Recipient or Destination");
            }
            if ((model.Files is null || model.Files.Count == 0) && string.IsNullOrEmpty(model.TemplateID))
            {
                return ResultHelper.RespondError<FaxApiResult>("Empty document: Please specify Files or TemplateID");
            }

            return await HttpRequest.PostAsync<FaxApiResult>(BuildAPIURL(), User, BuildRequestBody(model));
        }

        [ComVisible(false)]
        public async Task<FaxApiResult> SendMessageAsync(
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
            var model = new FaxModel
            {
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

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/fax/{Uri.EscapeDataString(messageID.Value!)}";
        }

        #region Status
        public FaxStatusApiResult Status(MessageID messageID)
        {
            return Task.Run(() => StatusAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<FaxStatusApiResult> StatusAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<FaxStatusApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<FaxStatusApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.GetAsync<FaxStatusApiResult>(BuildStatusURL(messageID), User);
        }
        #endregion

        private static string BuildRescheduleURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/fax/{Uri.EscapeDataString(messageID.Value!)}/reschedule";
        }

        private static string BuildAbortURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/fax/{Uri.EscapeDataString(messageID.Value!)}/abort";
        }

        private static string BuildResubmitURL(MessageID messageID)
        {
            IDGuard.EnsureProvided(messageID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/fax/{Uri.EscapeDataString(messageID.Value!)}/resubmit";
        }

        #region Reschedule
        public FaxActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => RescheduleAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<FaxActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<FaxActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<FaxActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<FaxActionApiResult>(BuildRescheduleURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion

        #region Abort
        public FaxActionApiResult Abort(MessageID messageID)
        {
            return Task.Run(() => AbortAsync(messageID)).Result;
        }

        [ComVisible(false)]
        public async Task<FaxActionApiResult> AbortAsync(MessageID messageID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<FaxActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<FaxActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<FaxActionApiResult>(BuildAbortURL(messageID), User, new { });
        }
        #endregion

        #region Resubmit
        public FaxActionApiResult Resubmit(MessageID messageID, DateTime sendTime)
        {
            return Task.Run(() => ResubmitAsync(messageID, sendTime)).Result;
        }

        [ComVisible(false)]
        public async Task<FaxActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<FaxActionApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (messageID is null || string.IsNullOrEmpty(messageID.Value))
            {
                return ResultHelper.RespondError<FaxActionApiResult>("Empty MessageID: Please specify MessageID");
            }

            return await HttpRequest.PatchAsync<FaxActionApiResult>(BuildResubmitURL(messageID), User, new { SendTime = sendTime });
        }
        #endregion
    }
}