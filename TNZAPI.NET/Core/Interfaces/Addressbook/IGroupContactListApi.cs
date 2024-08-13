using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupContactListApi : IApiClientBase
    {
        GroupContactListApiResult List(GroupModel entity, int? recordsPerPage = null, int? page = null);
        GroupContactListApiResult List(GroupModel entity, IListRequestOptions options);

        GroupContactListApiResult List(GroupID groupID, int? recordsPerPage = null, int? page = null);
        GroupContactListApiResult List(GroupID groupID, IListRequestOptions options);

        GroupContactListApiResult ListByGroupCode(string groupCode, int? recordsPerPage = null, int? page = null);
        GroupContactListApiResult ListByGroupCode(string groupCode, IListRequestOptions options);

        Task<GroupContactListApiResult> ListAsync(GroupModel entity, int? recordsPerPage = null, int? page = null);
        Task<GroupContactListApiResult> ListAsync(GroupModel entity, IListRequestOptions options);

        Task<GroupContactListApiResult> ListAsync(GroupID groupID, int? recordsPerPage = null, int? page = null);
        Task<GroupContactListApiResult> ListAsync(GroupID groupID, IListRequestOptions options);

        Task<GroupContactListApiResult> ListByGroupCodeAsync(string groupCode, int? recordsPerPage = null, int? page = null);
        Task<GroupContactListApiResult> ListByGroupCodeAsync(string groupCode, IListRequestOptions options);
    }
}