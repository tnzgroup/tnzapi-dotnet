using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Configuration.OptOut.Dto
{
    public class OptOutModel
    {
        public string? DestType { get; set; }
        public string? Destination { get; set; }
        public ContactID? ContactID { get; set; }
        public string? SubAccount { get; set; }
        public string? Department { get; set; }
        public string? StopMessage { get; set; }
        public string? Notes { get; set; }
    }
}