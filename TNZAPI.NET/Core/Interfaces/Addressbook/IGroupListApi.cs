using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupListApi : IApiClientBase
    {
        GroupListApiResult List(int? recordsPerPage = null, int? page = null);
        GroupListApiResult List(IListRequestOptions options);

        Task<GroupListApiResult> ListAsync(int? recordsPerPage = null, int? page = null);
        Task<GroupListApiResult> ListAsync(IListRequestOptions options);
    }
}