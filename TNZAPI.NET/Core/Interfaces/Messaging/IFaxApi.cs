using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface IFaxApi
    {
        FaxApiResult SendMessage(FaxModel entity);
        Task<FaxApiResult> SendMessageAsync(FaxModel entity);

        FaxApiResult SendMessage(
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
        Task<FaxApiResult> SendMessageAsync(
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

        FaxStatusApiResult Status(MessageID messageID);
        Task<FaxStatusApiResult> StatusAsync(MessageID messageID);

        FaxActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<FaxActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        FaxActionApiResult Abort(MessageID messageID);
        Task<FaxActionApiResult> AbortAsync(MessageID messageID);
        FaxActionApiResult Resubmit(MessageID messageID, DateTime sendTime);
        Task<FaxActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime);
    }
}