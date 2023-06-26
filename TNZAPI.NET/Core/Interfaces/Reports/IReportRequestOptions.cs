namespace TNZAPI.NET.Core.Interfaces.Reports
{
    public interface IReportRequestOptions
    {
        public string MessageID { get; set; }

        public int RecordsPerPage { get; set; }

        public int Page { get; set; }
    }
}
