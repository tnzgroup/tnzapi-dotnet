using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface IEmailApi
    {
        EmailApiResult SendMessage(EmailModel entity);
        Task<EmailApiResult> SendMessageAsync(EmailModel entity);

        EmailApiResult SendMessage(
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
        );
        Task<EmailApiResult> SendMessageAsync(
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
        );

        EmailStatusApiResult Status(MessageID messageID);
        Task<EmailStatusApiResult> StatusAsync(MessageID messageID);

        EmailActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<EmailActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        EmailActionApiResult Abort(MessageID messageID);
        Task<EmailActionApiResult> AbortAsync(MessageID messageID);
        EmailActionApiResult Resubmit(MessageID messageID, DateTime sendTime);
        Task<EmailActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime);
    }
}