using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupApi : IApiClientBase
    {
        GroupModel Entity { get; set; }

        GroupApiResult Create(GroupModel entity);
        GroupApiResult Create(
            string groupCode = null, 
            string groupName = null, 
            string subAccount = null, 
            string department = null,
            Enums.ViewEditByOptions? viewEditBy = null
        );

        Task<GroupApiResult> CreateAsync(GroupModel entity);
        Task<GroupApiResult> CreateAsync(
            string groupCode = null, 
            string groupName = null, 
            string subAccount = null, 
            string department = null,
            Enums.ViewEditByOptions? viewEditBy = null
        );

        GroupApiResult Delete(GroupModel entity);
        GroupApiResult DeleteByGroupCode(string groupCode);

        Task<GroupApiResult> DeleteAsync(GroupModel entity);
        Task<GroupApiResult> DeleteByGroupCodeAsync(string groupCode);

        GroupApiResult Get(GroupModel entity);
        GroupApiResult GetByGroupCode(string groupCode);

        Task<GroupApiResult> GetAsync(GroupModel entity);
        Task<GroupApiResult> GetByGroupCodeAsync(string groupCode);

        GroupApiResult Read(GroupModel entity);
        Task<GroupApiResult> ReadAsync(GroupModel entity);

        GroupApiResult ReadByGroupCode(string groupCode);
        Task<GroupApiResult> ReadByGroupCodeAsync(string groupCode);

        GroupApiResult Update(GroupModel entity);
        GroupApiResult Update(
            string groupCode = null, 
            string groupName = null,
            string subAccount = null, 
            string department = null,
            Enums.ViewEditByOptions? viewEditBy = null
        );

        Task<GroupApiResult> UpdateAsync(GroupModel entity);
        Task<GroupApiResult> UpdateAsync(
            string groupCode = null, 
            string groupName = null, 
            string subAccount = null, 
            string department = null,
            Enums.ViewEditByOptions? viewEditBy = null
        );

        GroupListApiResult List();
        GroupListApiResult List(IListRequestOptions options);
        Task<GroupListApiResult> ListAsync();
        Task<GroupListApiResult> ListAsync(IListRequestOptions options);
    }
}