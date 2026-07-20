using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface IVoiceApi
    {
        VoiceApiResult SendMessage(VoiceModel entity);
        Task<VoiceApiResult> SendMessageAsync(VoiceModel entity);

        VoiceApiResult SendMessage(
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
        );
        Task<VoiceApiResult> SendMessageAsync(
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
        );

        VoiceStatusApiResult Status(MessageID messageID);
        Task<VoiceStatusApiResult> StatusAsync(MessageID messageID);

        VoiceActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<VoiceActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        VoiceActionApiResult Abort(MessageID messageID);
        Task<VoiceActionApiResult> AbortAsync(MessageID messageID);
        VoiceActionApiResult Resubmit(MessageID messageID, DateTime sendTime);
        Task<VoiceActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime);
        VoiceActionApiResult Pacing(MessageID messageID, int numberOfOperators);
        Task<VoiceActionApiResult> PacingAsync(MessageID messageID, int numberOfOperators);
    }
}