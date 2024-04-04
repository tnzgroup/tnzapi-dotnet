using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Email.Dto
{
    public class EmailModel
    {
        public string ErrorEmailNotify { get; set; } = "";
        public string WebhookCallbackURL { get; set; } = "";
        public Enums.WebhookCallbackType WebhookCallbackFormat { get; set; } = Enums.WebhookCallbackType.JSON;

        public Enums.SendModeType SendMode { get; set; } = Enums.SendModeType.Live;

        public MessageID MessageID { get; set; }
        public string Reference { get; set; } = "";
        public DateTime SendTime { get; set; } = new DateTime();
        public string Timezone { get; set; } = "";
        public string SubAccount { get; set; } = "";
        public string Department { get; set; } = "";
        public string ChargeCode { get; set; } = "";

        public string EmailSubject { get; set; } = "";
        public string MessagePlain { get; set; } = "";
        public string MessageHTML { get; set; } = "";

        public string SMTPFrom { get; set; } = "";
        public string From { get; set; } = "";
        public string FromEmail { get; set; } = "";
        public string ReplyTo { get; set; } = "";
        public string CCEmail { get; set; } = "";

        public bool StripSlashes { get; set; } = true; // StripSlashes on MessageText

        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
