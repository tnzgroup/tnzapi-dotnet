using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Actions.Reschedule.Dto
{
    public class RescheduleRequestOptions
    {
        public MessageID MessageID { get; set; }

        public DateTime SendTime { get; set; } = DateTime.Now;

        public string Timezone { get; set; }
    }
}
