namespace TNZAPI.NET.Api.Reports.Status.Dto
{
    public class StatusRequestOptions
    {
        public string MessageID { get; set; } = "";
        public int RecordsPerPage { get; set; } = 100;
        public int Page { get; set; } = 1;
    }
}
