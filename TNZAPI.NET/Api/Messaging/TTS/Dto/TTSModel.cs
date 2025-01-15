using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.TTS.Dto
{
    public class TTSModel
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

				public string CallerID { get; set; } = "";
        public TTSVoiceType TTSVoice { get; set; } = TTSVoiceType.Female1;
        public string Options { get; set; } = "";

        public int NumberOfOperators { get; set; } = 0;
        public int RetryAttempts { get; set; } = 0; // number of retries
        public int RetryPeriod { get; set; } = 1; // minutes

        public string MessageToPeople { get; set; } = "";
        public string MessageToAnswerPhones { get; set; } = "";
        public string CallRouteMessageToPeople { get; set; } = "";
        public string CallRouteMessageToOperators { get; set; } = "";
        public string CallRouteMessageOnWrongKey { get; set; } = "";

        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();

        [ComVisible(false)]
        public ICollection<Keypad> Keypads { get; set; } = new List<Keypad>();

        [ComVisible(false)]
        public bool KeypadOptionRequired { get; set; } = false;
    }
}
