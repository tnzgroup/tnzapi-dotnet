using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Core.Common
{
    public class ListRequestOptions : IListRequestOptions
    {
        public int RecordsPerPage { get; set; } = 100;
        public int Page { get; set; } = 1;
    }
}
