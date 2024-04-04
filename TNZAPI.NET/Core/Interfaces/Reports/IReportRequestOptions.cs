using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Reports
{
    public interface IReportRequestOptions
    {
        public MessageID MessageID { get; set; }

        public int RecordsPerPage { get; set; }

        public int Page { get; set; }
    }
}
