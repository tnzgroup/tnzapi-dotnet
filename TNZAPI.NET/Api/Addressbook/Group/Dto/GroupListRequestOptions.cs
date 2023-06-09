using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Addressbook.Group.Dto
{
    public class GroupListRequestOptions : IListRequestOptions
    {
        public int RecordsPerPage { get; set; } = 100;

        public int Page { get; set; } = 1;
    }
}
