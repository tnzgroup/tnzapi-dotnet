using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupContactApi : IApiClientBase
    {
        GroupContactApiResult Add(GroupModel group, ContactModel contact);
        GroupContactApiResult Add(string groupCode, string contactID);
        GroupContactApiResult Add(
            GroupModel group = null,
            ContactModel contact = null,
            string groupCode = null,
            string contactID = null
        );

        Task<GroupContactApiResult> AddAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> AddAsync(string groupCode,string contactID);
        Task<GroupContactApiResult> AddAsync(
            GroupModel group = null,
            ContactModel contact = null,
            string groupCode = null,
            string contactID = null
        );

        GroupContactApiResult Get(GroupModel group, ContactModel contact);
        GroupContactApiResult Get(string groupCode, string contactID);
        GroupContactApiResult Get(
            GroupModel group = null,
            ContactModel contact = null,
            string groupCode = null,
            string contactID = null
        );

        Task<GroupContactApiResult> GetAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> GetAsync(string groupCode, string contactID);
        Task<GroupContactApiResult> GetAsync(
            GroupModel group = null,
            ContactModel contact = null,
            string groupCode = null,
            string contactID = null
        );

        GroupContactApiResult Read(GroupModel group, ContactModel contact);
        GroupContactApiResult Read(string groupCode, string contactID);
        GroupContactApiResult Read(
            GroupModel group = null,
            ContactModel contact = null,
            string groupCode = null,
            string contactID = null
        );

        Task<GroupContactApiResult> ReadAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> ReadAsync(string groupCode, string contactID);
        Task<GroupContactApiResult> ReadAsync(
            GroupModel group = null,
            ContactModel contact = null,
            string groupCode = null,
            string contactID = null
        );

        GroupContactApiResult Remove(GroupModel group, ContactModel contact);
        GroupContactApiResult Remove(string groupCode, string contactID);
        GroupContactApiResult Remove(
            GroupModel group = null,
            ContactModel contact = null,
            string groupCode = null,
            string contactID = null
        );

        Task<GroupContactApiResult> RemoveAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> RemoveAsync(string groupCode, string contactID);
        Task<GroupContactApiResult> RemoveAsync(
            GroupModel group = null,
            ContactModel contact = null,
            string groupCode = null,
            string contactID = null
        );

    }
}