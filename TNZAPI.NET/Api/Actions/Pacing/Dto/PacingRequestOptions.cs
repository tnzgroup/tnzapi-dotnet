using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Actions.Pacing.Dto
{
    public class PacingRequestOptions
    {
        public MessageID MessageID { get; set; }

        public int NumberOfOperators { get; set; } = 1;
    }
}
