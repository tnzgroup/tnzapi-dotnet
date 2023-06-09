using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactGroupApi : IApiClientBase
    {
        ContactGroupApiResult Add(ContactModel contact, GroupModel group);
        ContactGroupApiResult AddById(string contactID, string groupCode);
        Task<ContactGroupApiResult> AddAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> AddByIdAsync(string contactID, string groupCode);
        ContactGroupApiResult Get(ContactModel contact, GroupModel group);
        ContactGroupApiResult GetById(string contactID, string groupCode);
        Task<ContactGroupApiResult> GetAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> GetByIdAsync(string contactID, string groupCode);
        ContactGroupApiResult Read(ContactModel contact, GroupModel group);
        ContactGroupApiResult ReadById(string contactID, string groupCode);
        Task<ContactGroupApiResult> ReadAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> ReadByIdAsync(string contactID, string groupCode);
        ContactGroupApiResult Remove(ContactModel contact, GroupModel group);
        ContactGroupApiResult RemoveById(string contactID, string groupCode);
        Task<ContactGroupApiResult> RemoveAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> RemoveByIdAsync(string contactID, string groupCode);
    }
}