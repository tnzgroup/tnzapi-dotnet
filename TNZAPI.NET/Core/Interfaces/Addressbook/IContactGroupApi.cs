using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactGroupApi : IApiClientBase
    {
        ContactGroupApiResult Add(ContactModel contact, GroupModel group);
        ContactGroupApiResult Add(ContactID contactID, GroupID groupID);
        ContactGroupApiResult Add(
            ContactModel contact = null,
            GroupModel group = null,
            ContactID contactID = null,
            GroupID groupID = null
        );

        Task<ContactGroupApiResult> AddAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> AddAsync(ContactID contactID, GroupID groupID);
        Task<ContactGroupApiResult> AddAsync(
            ContactModel contact = null, 
            GroupModel group = null, 
            ContactID contactID = null, 
			GroupID groupID = null
        );

        ContactGroupApiResult Get(ContactModel contact, GroupModel group);
        ContactGroupApiResult Get(ContactID contactID, GroupID groupID);
        ContactGroupApiResult Get(
            ContactModel contact = null, 
            GroupModel group = null,
            ContactID contactID = null, 
            GroupID groupID = null
        );

        Task<ContactGroupApiResult> GetAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> GetAsync(ContactID contactID, GroupID groupID);
        Task<ContactGroupApiResult> GetAsync(
            ContactModel contact = null,
            GroupModel group = null,
            ContactID contactID = null,
            GroupID groupID = null
        );

        ContactGroupApiResult Read(ContactModel contact, GroupModel group);
        ContactGroupApiResult Read(ContactID contactID, GroupID groupID);
        ContactGroupApiResult Read(
            ContactModel contact = null, 
            GroupModel group = null,
			ContactID contactID = null,
			GroupID groupID = null
        );

        Task<ContactGroupApiResult> ReadAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> ReadAsync(ContactID contactID, GroupID groupID);
        Task<ContactGroupApiResult> ReadAsync(
            ContactModel contact = null, 
            GroupModel group = null,
			ContactID contactID = null,
			GroupID groupID = null
        );

        ContactGroupApiResult Remove(ContactModel contact, GroupModel group);
        ContactGroupApiResult Remove(ContactID contactID, GroupID groupID);
        ContactGroupApiResult Remove(
            ContactModel contact = null, 
            GroupModel group = null,
			ContactID contactID = null,
			GroupID groupID = null
        );

        Task<ContactGroupApiResult> RemoveAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> RemoveAsync(ContactID contactID, GroupID groupID);
        Task<ContactGroupApiResult> RemoveAsync(
            ContactModel contact = null, 
            GroupModel group = null,
			ContactID contactID = null,
			GroupID groupID = null
        );
    }
}