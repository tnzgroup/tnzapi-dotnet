using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces.Messaging
{
    public interface ITTSApi
    {
        TTSApiResult SendMessage(TTSModel entity);
        Task<TTSApiResult> SendMessageAsync(TTSModel entity);

        TTSApiResult SendMessage(
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
        Task<TTSApiResult> SendMessageAsync(
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

        TTSStatusApiResult Status(MessageID messageID);
        Task<TTSStatusApiResult> StatusAsync(MessageID messageID);

        TTSActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<TTSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        TTSActionApiResult Abort(MessageID messageID);
        Task<TTSActionApiResult> AbortAsync(MessageID messageID);
        TTSActionApiResult Resubmit(MessageID messageID, DateTime sendTime);
        Task<TTSActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime);
        TTSActionApiResult Pacing(MessageID messageID, int numberOfOperators);
        Task<TTSActionApiResult> PacingAsync(MessageID messageID, int numberOfOperators);
    }
}