using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupApi
    {
        GroupApiResult Create(GroupModel entity);
        Task<GroupApiResult> CreateAsync(GroupModel entity);

        GroupListApiResult List(int page = 1, int recordsPerPage = 100);
        Task<GroupListApiResult> ListAsync(int page = 1, int recordsPerPage = 100);

        GroupApiResult Details(GroupID groupID);
        Task<GroupApiResult> DetailsAsync(GroupID groupID);

        GroupApiResult Update(GroupID groupID, GroupModel entity);
        Task<GroupApiResult> UpdateAsync(GroupID groupID, GroupModel entity);

        GroupApiResult Delete(GroupID groupID);
        Task<GroupApiResult> DeleteAsync(GroupID groupID);

        IGroupContactApi Contact { get; set; }
    }
}