using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Voice.Dto
{
    public class VoiceModel
    {
        public MessageID? MessageID { get; set; }
        public string? Reference { get; set; } = "";
        public string? MessageToPeople { get; set; } = "";
        public string? TemplateID { get; set; } = "";
        public ContactID? ContactID { get; set; }
        public GroupID? GroupID { get; set; }
        public ICollection<Recipient>? Recipients { get; set; } = new List<Recipient>();
        public ICollection<Destination>? Destinations { get; set; } = new List<Destination>();
        public Enums.NotificationType? NotificationType { get; set; }
        public string? WebhookCallbackURL { get; set; } = "";
        public Enums.WebhookCallbackType? WebhookCallbackFormat { get; set; }
        public string? ReportTo { get; set; } = "";
        public DateTime? SendTime { get; set; }
        public string? Timezone { get; set; } = "";
        public string? SubAccount { get; set; } = "";
        public string? Department { get; set; } = "";
        public string? ChargeCode { get; set; } = "";
        public string? MessageToAnswerPhones { get; set; } = "";
        public Enums.AnswerPhoneMode? AnswerPhoneMode { get; set; }
        public ICollection<KeypadModel>? Keypads { get; set; } = new List<KeypadModel>();
        public ICollection<VoiceFileModel>? VoiceFiles { get; set; } = new List<VoiceFileModel>();
        public bool KeypadOptionRequired { get; set; } = false;
        public string? CallRouteMessageOnWrongKey { get; set; } = "";
        public string? CallRouteMessageToPeople { get; set; } = "";
        public string? CallRouteMessageToOperators { get; set; } = "";
        public string? EndCallMessage { get; set; } = "";
        public int? NumberOfOperators { get; set; }
        public int? RetryAttempts { get; set; }
        public int? RetryPeriod { get; set; }
        public string? CallerID { get; set; } = "";
        public string? Options { get; set; } = "";
        public Enums.SendModeType SendMode { get; set; } = Enums.SendModeType.Live;
    }

    public class VoiceFileModel
    {
        public string? Name { get; set; } = "";
        public string? File { get; set; } = "";
    }
}