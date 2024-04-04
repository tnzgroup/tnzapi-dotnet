using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupContactApi : IApiClientBase
    {
        GroupContactApiResult Add(GroupModel group, ContactModel contact);
        GroupContactApiResult Add(GroupID groupID, ContactID contactID);
        GroupContactApiResult Add(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        );

        Task<GroupContactApiResult> AddAsync(GroupModel group, ContactModel contact);
		Task<GroupContactApiResult> AddAsync(GroupID groupID, ContactID contactID);
		Task<GroupContactApiResult> AddAsync(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        );

        GroupContactApiResult Get(GroupModel group, ContactModel contact);
		GroupContactApiResult Get(GroupID groupID, ContactID contactID);
        GroupContactApiResult Get(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        );

        Task<GroupContactApiResult> GetAsync(GroupModel group, ContactModel contact);
		Task<GroupContactApiResult> GetAsync(GroupID groupID, ContactID contact);
        Task<GroupContactApiResult> GetAsync(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        );

        GroupContactApiResult Read(GroupModel group, ContactModel contact);
        GroupContactApiResult Read(GroupID groupID, ContactID contactID);
        GroupContactApiResult Read(
            GroupModel group = null,
            ContactModel contact = null,
            GroupID groupID = null,
            ContactID contactID = null
        );

        Task<GroupContactApiResult> ReadAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> ReadAsync(GroupID groupID, ContactID contactID);
        Task<GroupContactApiResult> ReadAsync(
            GroupModel group = null,
            ContactModel contact = null,
			GroupID groupID = null,
			ContactID contactID = null
		);

        GroupContactApiResult Remove(GroupModel group, ContactModel contact);
        GroupContactApiResult Remove(GroupID groupID, ContactID contactID);
        GroupContactApiResult Remove(
            GroupModel group = null,
            ContactModel contact = null,
			GroupID groupID = null,
			ContactID contactID = null
		);

        Task<GroupContactApiResult> RemoveAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> RemoveAsync(GroupID groupID, ContactID contactID);
        Task<GroupContactApiResult> RemoveAsync(
            GroupModel group = null,
            ContactModel contact = null,
			GroupID groupID = null,
			ContactID contactID = null
		);

    }
}