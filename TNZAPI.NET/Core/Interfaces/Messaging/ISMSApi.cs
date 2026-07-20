using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface ISMSApi
    {
        SMSApiResult SendMessage(SMSModel entity);
        Task<SMSApiResult> SendMessageAsync(SMSModel entity);

        SMSApiResult SendMessage(
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
        );
        Task<SMSApiResult> SendMessageAsync(
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
        );

        SMSStatusApiResult Status(MessageID messageID);
        Task<SMSStatusApiResult> StatusAsync(MessageID messageID);

        SMSReceivedApiResult Received(int timePeriod, int recordsPerPage = 100, int page = 1);
        SMSReceivedApiResult Received(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1);
        Task<SMSReceivedApiResult> ReceivedAsync(int timePeriod, int recordsPerPage = 100, int page = 1);
        Task<SMSReceivedApiResult> ReceivedAsync(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1);

        SMSStatusApiResult SMSReply(MessageID messageID, int recordsPerPage = 100, int page = 1);
        Task<SMSStatusApiResult> SMSReplyAsync(MessageID messageID, int recordsPerPage = 100, int page = 1);

        SMSActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<SMSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        SMSActionApiResult Abort(MessageID messageID);
        Task<SMSActionApiResult> AbortAsync(MessageID messageID);
    }
}