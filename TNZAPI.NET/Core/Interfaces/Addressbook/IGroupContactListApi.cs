using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupContactListApi : IApiClientBase
    {
        GroupContactListApiResult List(GroupModel entity, GroupContactListRequestOptions options);
        GroupContactListApiResult List(GroupModel entity, int? recordsPerPage = null, int? page = null);

        GroupContactListApiResult ListByGroupCode(string groupCode, GroupContactListRequestOptions options);
        GroupContactListApiResult ListByGroupCode(string groupCode, int? recordsPerPage = null, int? page = null);

        Task<GroupContactListApiResult> ListAsync(GroupModel entity, GroupContactListRequestOptions options);
        Task<GroupContactListApiResult> ListAsync(GroupModel entity, int? recordsPerPage = null, int? page = null);

        Task<GroupContactListApiResult> ListByGroupCodeAsync(string groupCode, GroupContactListRequestOptions options);
        Task<GroupContactListApiResult> ListByGroupCodeAsync(string groupCode, int? recordsPerPage = null, int? page = null);
    }
}