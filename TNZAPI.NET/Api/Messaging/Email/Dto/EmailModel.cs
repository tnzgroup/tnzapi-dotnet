using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Email.Dto
{
    public class EmailModel
    {
        public MessageID? MessageID { get; set; }
        public string? Reference { get; set; } = "";
        public string? MessagePlain { get; set; } = "";
        public string? MessageHTML { get; set; } = "";
        public string? TemplateID { get; set; } = "";
        public ContactID? ContactID { get; set; }
        public GroupID? GroupID { get; set; }
        public ICollection<Recipient>? Recipients { get; set; } = new List<Recipient>();
        public ICollection<Destination>? Destinations { get; set; } = new List<Destination>();
        public ICollection<Attachment>? Files { get; set; } = new List<Attachment>();
        public string? SMTPFrom { get; set; } = "";
        public string? From { get; set; } = "";
        public string? FromEmail { get; set; } = "";
        public string? CCEmail { get; set; } = "";
        public string? ReplyTo { get; set; } = "";
        public string? EmailSubject { get; set; } = "";
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