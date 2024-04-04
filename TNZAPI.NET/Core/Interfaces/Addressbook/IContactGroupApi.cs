using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactGroupApi : IApiClientBase
    {
        ContactGroupApiResult Add(ContactModel contact, GroupModel group);
        ContactGroupApiResult Add(ContactID contactID, GroupID groupID);
        ContactGroupApiResult Add(string contactID, string groupCode);
        ContactGroupApiResult Add(
            ContactModel contact = null,
            GroupModel group = null,
            object contactID = null,
            object groupID = null,
            string groupCode = null
        );

        Task<ContactGroupApiResult> AddAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> AddAsync(ContactID contactID, GroupID groupID);
        Task<ContactGroupApiResult> AddAsync(string contactID, string groupCode);
        Task<ContactGroupApiResult> AddAsync(
            ContactModel contact = null, 
            GroupModel group = null, 
            object contactID = null, 
            object groupID = null,
			string groupCode = null
        );

        ContactGroupApiResult Get(ContactModel contact, GroupModel group);
        ContactGroupApiResult Get(ContactID contactID, GroupID groupID);
        ContactGroupApiResult Get(string contactID, string groupCode);
        ContactGroupApiResult Get(
            ContactModel contact = null, 
            GroupModel group = null,
            object contactID = null, 
            object groupID = null,
			string groupCode = null
        );

        Task<ContactGroupApiResult> GetAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> GetAsync(ContactID contactID, GroupID groupID);
		Task<ContactGroupApiResult> GetAsync(string contactID, string groupCode);
        Task<ContactGroupApiResult> GetAsync(
            ContactModel contact = null,
            GroupModel group = null,
            object contactID = null,
            object groupID = null,
			string groupCode = null
        );

        ContactGroupApiResult Read(ContactModel contact, GroupModel group);
        ContactGroupApiResult Read(ContactID contactID, GroupID groupID);
		ContactGroupApiResult Read(string contactID, string groupCode);
        ContactGroupApiResult Read(
            ContactModel contact = null, 
            GroupModel group = null,
			object contactID = null,
			object groupID = null,
			string groupCode = null 
        );

        Task<ContactGroupApiResult> ReadAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> ReadAsync(ContactID contactID, GroupID groupID);
		Task<ContactGroupApiResult> ReadAsync(string contactID, string groupCode);
        Task<ContactGroupApiResult> ReadAsync(
            ContactModel contact = null, 
            GroupModel group = null,
			object contactID = null,
			object groupID = null,
			string groupCode = null 
        );

        ContactGroupApiResult Remove(ContactModel contact, GroupModel group);
        ContactGroupApiResult Remove(ContactID contactID, GroupID groupID);
		ContactGroupApiResult Remove(string contactID, string groupCode);
        ContactGroupApiResult Remove(
            ContactModel contact = null, 
            GroupModel group = null,
			object groupID = null,
			object contactID = null,
			string groupCode = null 
        );

        Task<ContactGroupApiResult> RemoveAsync(ContactModel contact, GroupModel group);
        Task<ContactGroupApiResult> RemoveAsync(ContactID contactID, GroupID groupID);
		Task<ContactGroupApiResult> RemoveAsync(string contactID, string groupCode);
        Task<ContactGroupApiResult> RemoveAsync(
            ContactModel contact = null, 
            GroupModel group = null,
			object contactID = null,
			object groupID = null,
			string groupCode = null 
        );
    }
}