using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.Voice.Dto
{
    public class VoiceModel
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

        

        public string CallerID { get; set; } = "";
        public string Options { get; set; } = "";
        public string ReportTo { get; set; } = "";

        public int NumberOfOperators { get; set; } = 0;
        public int RetryAttempts { get; set; } = 0; // number of retries
        public int RetryPeriod { get; set; } = 1; // minutes

        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();

        [ComVisible(false)]
        public ICollection<Keypad> Keypads { get; set; } = new List<Keypad>();

        [ComVisible(false)]
        public bool KeypadOptionRequired { get; set; } = false;

        // MessageData
        public IDictionary<MessageDataType, string> MessageData { get; set; } = new Dictionary<MessageDataType, string>();
        public IDictionary<MessageDataType, Attachment> MessageDataAttachments { get; set; } = new Dictionary<MessageDataType, Attachment>();
    }
}
