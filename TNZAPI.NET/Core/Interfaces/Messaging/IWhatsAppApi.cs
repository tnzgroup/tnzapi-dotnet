using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.WhatsApp.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface IWhatsAppApi
    {
        WhatsAppApiResult SendMessage(WhatsAppModel entity);
        Task<WhatsAppApiResult> SendMessageAsync(WhatsAppModel entity);

        WhatsAppApiResult SendMessage(
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
        );
        Task<WhatsAppApiResult> SendMessageAsync(
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
        );

        WhatsAppStatusApiResult Status(MessageID messageID);
        Task<WhatsAppStatusApiResult> StatusAsync(MessageID messageID);

        WhatsAppReceivedApiResult Received(int timePeriod, int recordsPerPage = 100, int page = 1);
        WhatsAppReceivedApiResult Received(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1);
        Task<WhatsAppReceivedApiResult> ReceivedAsync(int timePeriod, int recordsPerPage = 100, int page = 1);
        Task<WhatsAppReceivedApiResult> ReceivedAsync(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1);

        WhatsAppActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<WhatsAppActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        WhatsAppActionApiResult Abort(MessageID messageID);
        Task<WhatsAppActionApiResult> AbortAsync(MessageID messageID);
    }
}