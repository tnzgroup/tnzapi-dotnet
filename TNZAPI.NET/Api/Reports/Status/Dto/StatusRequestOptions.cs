using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core.Interfaces.Reports;

namespace TNZAPI.NET.Api.Reports.Status.Dto
{
    public class StatusRequestOptions : IReportRequestOptions
    {
        public MessageID MessageID { get; set; }
        public int RecordsPerPage { get; set; } = 100;
        public int Page { get; set; } = 1;
    }
}
