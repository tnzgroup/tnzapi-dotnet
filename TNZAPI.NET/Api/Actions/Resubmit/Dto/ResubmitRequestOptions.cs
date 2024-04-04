using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Actions.Resubmit.Dto
{
    public class ResubmitRequestOptions
    {
        public MessageID MessageID { get; set; }

        public DateTime SendTime { get; set; } = DateTime.Now;

        public string Timezone { get; set; }
    }
}
