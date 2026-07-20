using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Configuration.OptOut.Dto
{
    public class OptOutListApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();

        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }

        public List<OptOutDetail> OptOuts { get; set; } = new List<OptOutDetail>();
    }
}