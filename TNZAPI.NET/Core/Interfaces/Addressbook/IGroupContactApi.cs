using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IGroupContactApi : IApiClientBase
    {
        GroupContactApiResult Add(GroupModel group, ContactModel contact);
        GroupContactApiResult AddById(string groupCode, string contactID);
        Task<GroupContactApiResult> AddAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> AddByIdAsync(string groupCode, string contactID);

        GroupContactApiResult Get(GroupModel group, ContactModel contact);
        GroupContactApiResult GetById(string groupCode, string contactID);
        Task<GroupContactApiResult> GetAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> GetByIdAsync(string groupCode, string contactID);

        GroupContactApiResult Read(GroupModel group, ContactModel contact);
        GroupContactApiResult ReadById(string groupCode, string contactID);
        Task<GroupContactApiResult> ReadAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> ReadByIdAsync(string groupCode, string contactID);

        GroupContactApiResult Remove(GroupModel group, ContactModel contact);
        GroupContactApiResult RemoveById(string groupCode, string contactID);
        Task<GroupContactApiResult> RemoveAsync(GroupModel group, ContactModel contact);
        Task<GroupContactApiResult> RemoveByIdAsync(string groupCode, string contactID);

    }
}