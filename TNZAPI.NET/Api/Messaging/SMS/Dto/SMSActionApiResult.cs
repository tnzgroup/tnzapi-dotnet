using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Messaging.SMS.Dto
{
    public class SMSActionApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();

        public string? ActionResult { get; set; }
        public MessageID? MessageID { get; set; }
        public string? JobNum { get; set; }
        public Enums.JobStatus Status { get; set; }
        public string? Action { get; set; }
    }
}