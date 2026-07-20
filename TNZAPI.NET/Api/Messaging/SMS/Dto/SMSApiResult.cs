using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Messaging.SMS.Dto
{
    public class SMSApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();
        public MessageID? MessageID { get; set; }
    }
}