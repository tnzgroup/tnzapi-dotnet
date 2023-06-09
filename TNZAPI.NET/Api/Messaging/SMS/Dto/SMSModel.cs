using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;

namespace TNZAPI.NET.Api.Messaging.SMS.Dto
{
    public class SMSModel
    {
        public string ErrorEmailNotify { get; set; } = "";
        public string WebhookCallbackURL { get; set; } = "";
        public Enums.WebhookCallbackType WebhookCallbackFormat { get; set; } = Enums.WebhookCallbackType.JSON;

        public Enums.SendModeType SendMode { get; set; } = Enums.SendModeType.Live;

        public string MessageID { get; set; } = "";
        public string Reference { get; set; } = "";
        public DateTime SendTime { get; set; } = new DateTime();
        public string Timezone { get; set; } = "";
        public string SubAccount { get; set; } = "";
        public string Department { get; set; } = "";
        public string ChargeCode { get; set; } = "";


        public string FromNumber { get; set; } = "";
        public string SMSEmailReply { get; set; } = "";
        public string ForceGSMChars { get; set; } = "";
        public string MessageText { get; set; } = "";

        public bool StripSlashes { get; set; } = true; // StripSlashes on MessageText

        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    }
}
