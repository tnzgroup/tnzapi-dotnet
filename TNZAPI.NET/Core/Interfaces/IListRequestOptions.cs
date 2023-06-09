namespace TNZAPI.NET.Core.Interfaces
{
    public interface IListRequestOptions
    {
        public int RecordsPerPage { get; set; }

        public int Page { get; set; }
    }
}
