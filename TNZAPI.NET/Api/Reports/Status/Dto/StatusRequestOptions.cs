using TNZAPI.NET.Core.Interfaces.Reports;

namespace TNZAPI.NET.Api.Reports.Status.Dto
{
    public class StatusRequestOptions : IReportRequestOptions
    {
        public string MessageID { get; set; } = "";
        public int RecordsPerPage { get; set; } = 100;
        public int Page { get; set; } = 1;
    }
}
