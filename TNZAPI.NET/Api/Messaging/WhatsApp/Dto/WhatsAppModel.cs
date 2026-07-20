using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.WhatsApp.Dto
{
    public class WhatsAppModel
    {
        public MessageID? MessageID { get; set; }
        public string? Reference { get; set; } = "";
        public string? Message { get; set; } = "";
        public string? TemplateID { get; set; } = "";
        public ICollection<Enums.WhatsAppFallbackMode>? FallbackMode { get; set; } = new List<Enums.WhatsAppFallbackMode>();
        public string? FromNumber { get; set; } = "";
        public ContactID? ContactID { get; set; }
        public GroupID? GroupID { get; set; }
        public ICollection<Recipient>? Recipients { get; set; } = new List<Recipient>();
        public ICollection<Destination>? Destinations { get; set; } = new List<Destination>();
        public ICollection<Attachment>? Files { get; set; } = new List<Attachment>();
        public Enums.NotificationType? NotificationType { get; set; }
        public string? WebhookCallbackURL { get; set; } = "";
        public Enums.WebhookCallbackType? WebhookCallbackFormat { get; set; }
        public string? ReportTo { get; set; } = "";
        public DateTime? SendTime { get; set; }
        public string? Timezone { get; set; } = "";
        public string? SubAccount { get; set; } = "";
        public string? Department { get; set; } = "";
        public string? ChargeCode { get; set; } = "";
        public Enums.SendModeType SendMode { get; set; } = Enums.SendModeType.Live;
    }
}