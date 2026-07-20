using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.RCS.Dto;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface IRCSApi
    {
        RCSApiResult SendMessage(RCSModel entity);
        Task<RCSApiResult> SendMessageAsync(RCSModel entity);

        RCSApiResult SendMessage(
            string? messageText = null,
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
        Task<RCSApiResult> SendMessageAsync(
            string? messageText = null,
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

        RCSStatusApiResult Status(MessageID messageID);
        Task<RCSStatusApiResult> StatusAsync(MessageID messageID);

        RCSReceivedApiResult Received(int timePeriod, int recordsPerPage = 100, int page = 1);
        RCSReceivedApiResult Received(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1);
        Task<RCSReceivedApiResult> ReceivedAsync(int timePeriod, int recordsPerPage = 100, int page = 1);
        Task<RCSReceivedApiResult> ReceivedAsync(DateTime dateFrom, DateTime? dateTo = null, int recordsPerPage = 100, int page = 1);

        RCSActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<RCSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        RCSActionApiResult Abort(MessageID messageID);
        Task<RCSActionApiResult> AbortAsync(MessageID messageID);
    }
}