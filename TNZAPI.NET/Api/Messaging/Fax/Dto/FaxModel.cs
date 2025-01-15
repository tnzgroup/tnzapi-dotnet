using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Fax.Dto
{
    public class FaxModel
    {
				[Obsolete("Use ReportTo instead of ErrorEmailNotify.")]
				public string ErrorEmailNotify { get; set; } = "";
				public string ReportTo { get; set; } = "";
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
				public string ServiceName { get; set; } = "";

				public string Resolution { get; set; } = "high";
        public string CSID { get; set; } = "";
        public string StampFormat { get; set; } = "";

        public string WatermarkFolder { get; set; } = "";
        public string WatermarkFirstPage { get; set; } = "No";
        public string WatermarkAllPages { get; set; } = "No";

        public int RetryAttempts { get; set; } = 3;
        public int RetryPeriod { get; set; } = 1;

        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
