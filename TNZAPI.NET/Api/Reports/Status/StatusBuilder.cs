using TNZAPI.NET.Api.Reports.Status.Dto;
using TNZAPI.NET.Core.Builders;

namespace TNZAPI.NET.Api.Reports.SMSReply
{
    public class StatusBuilder : ReportRequestOptionBuilder<StatusRequestOptions>
    {
        public StatusBuilder()
        {
            
        }

        public StatusBuilder(string messageID) : base(messageID)
        {

        }
    }
}
