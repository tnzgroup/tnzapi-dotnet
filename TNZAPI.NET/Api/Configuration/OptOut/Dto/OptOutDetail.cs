using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Configuration.OptOut.Dto
{
    public class OptOutDetail
    {
        public OptOutID? ID { get; set; }
        public string? DestType { get; set; }
        public string? Destination { get; set; }
        public ContactID? ContactID { get; set; }
        public string? SubAccount { get; set; }
        public string? Department { get; set; }
        public string? StopMessage { get; set; }
        public string? Notes { get; set; }
        public string? OriginalMessage { get; set; }
        public DateTime? CreatedTimeLocal { get; set; }
        public DateTime? CreatedTimeUTC { get; set; }
        public DateTime? CreatedTimeUTC_RFC3339 { get; set; }
        public DateTime? UpdatedTimeLocal { get; set; }
        public DateTime? UpdatedTimeUTC { get; set; }
        public DateTime? UpdatedTimeUTC_RFC3339 { get; set; }
        public string? Timezone { get; set; }
    }
}