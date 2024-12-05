using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Common.Dto
{
		public abstract class MessageModel
		{
				[Obsolete("Use ReportTo instead of ErrorEmailNotify.")]
				public string ErrorEmailNotify { get; set; } = "";
				public string ReportTo { get; set; } = "";
				public string WebhookCallbackURL { get; set; } = "";
				public Enums.WebhookCallbackType WebhookCallbackFormat { get; set; } = Enums.WebhookCallbackType.JSON;

				public Enums.SendModeType SendMode { get; set; } = Enums.SendModeType.Live;

				public MessageID MessageID { get; set; } = new();
				public string Reference { get; set; } = "";
				public DateTime SendTime { get; set; } = new();
				public string Timezone { get; set; } = "";
				public string SubAccount { get; set; } = "";
				public string Department { get; set; } = "";
				public string ChargeCode { get; set; } = "";

				public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
		}
}
