using TNZAPI.NET.Api.Addressbook.Contact.Dto;
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
        GroupApiResult Delete(GroupID groupID);

        Task<GroupApiResult> DeleteAsync(GroupModel entity);
        Task<GroupApiResult> DeleteAsync(GroupID groupID);

        GroupApiResult Get(GroupModel entity);
        GroupApiResult Get(GroupID groupID);

        Task<GroupApiResult> GetAsync(GroupModel entity);
        Task<GroupApiResult> GetAsync(GroupID groupID);

        GroupApiResult Read(GroupModel entity);
        GroupApiResult Read(GroupID groupID);

        Task<GroupApiResult> ReadAsync(GroupModel entity);
        Task<GroupApiResult> ReadAsync(GroupID groupID);

        GroupApiResult Update(GroupModel entity);
        GroupApiResult Update(
            GroupID groupID = null,
            string groupCode = null, 
            string groupName = null,
            string subAccount = null, 
            string department = null,
            Enums.ViewEditByOptions? viewEditBy = null
        );

		Task<GroupApiResult> UpdateAsync(GroupModel entity);
        Task<GroupApiResult> UpdateAsync(
			GroupID groupID = null,
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