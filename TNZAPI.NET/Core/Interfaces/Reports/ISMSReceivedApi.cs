using TNZAPI.NET.Api.Reports.SMSReceived.Dto;

namespace TNZAPI.NET.Core.Interfaces.Reports
{
    public interface ISMSReceivedApi : IApiClientBase
    {
        SMSReceivedApiResult List(SMSReceivedRequestOptions options);
        SMSReceivedApiResult List(
            int? timePeriod = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            int? recordsPerPage = null,
            int? page = null
        );

        Task<SMSReceivedApiResult> ListAsync(SMSReceivedRequestOptions options);
        Task<SMSReceivedApiResult> ListAsync(
            int? timePeriod = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            int? recordsPerPage = null,
            int? page = null
        );
    }
}