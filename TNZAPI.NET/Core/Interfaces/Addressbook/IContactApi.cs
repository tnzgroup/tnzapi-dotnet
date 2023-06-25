using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common;

namespace TNZAPI.NET.Core.Interfaces.Addressbook
{
    public interface IContactApi : IApiClientBase
    {
        ContactApiResult Create(ContactModel entity);

        ContactApiResult Create(
            string attention = null,
            string Title = null,
            string company = null,
            string companyDepartment = null,
            string firstName = null,
            string lastName = null,
            string position = null,
            string streetAddress = null,
            string suburb = null,
            string city = null,
            string state = null,
            string country = null,
            string postcode = null,
            string mainPhone = null,
            string altPhone1 = null,
            string altPhone2 = null,
            string directPhone = null,
            string mobilePhone = null,
            string faxNumber = null,
            string emailAddress = null,
            string webAddress = null,
            string custom1 = null,
            string custom2 = null,
            string custom3 = null,
            string custom4 = null,
            Enums.ViewEditByOptions? viewBy = null,
            Enums.ViewEditByOptions? editBy = null
        );

        Task<ContactApiResult> CreateAsync(ContactModel entity);

        Task<ContactApiResult> CreateAsync(
            string attention = null,
            string Title = null,
            string company = null,
            string companyDepartment = null,
            string firstName = null,
            string lastName = null,
            string position = null,
            string streetAddress = null,
            string suburb = null,
            string city = null,
            string state = null,
            string country = null,
            string postcode = null,
            string mainPhone = null,
            string altPhone1 = null,
            string altPhone2 = null,
            string directPhone = null,
            string mobilePhone = null,
            string faxNumber = null,
            string emailAddress = null,
            string webAddress = null,
            string custom1 = null,
            string custom2 = null,
            string custom3 = null,
            string custom4 = null,
            Enums.ViewEditByOptions? viewBy = null,
            Enums.ViewEditByOptions? editBy = null
        );

        ContactApiResult Update(ContactModel entity);

        ContactApiResult Update(
            string contactId,
            string attention = null,
            string title = null,
            string company = null,
            string companyDepartment = null,
            string firstName = null,
            string lastName = null,
            string position = null,
            string streetAddress = null,
            string suburb = null,
            string city = null,
            string state = null,
            string country = null,
            string postcode = null,
            string mainPhone = null,
            string altPhone1 = null,
            string altPhone2 = null,
            string directPhone = null,
            string mobilePhone = null,
            string faxNumber = null,
            string emailAddress = null,
            string webAddress = null,
            string custom1 = null,
            string custom2 = null,
            string custom3 = null,
            string custom4 = null,
            Enums.ViewEditByOptions? viewBy = null,
            Enums.ViewEditByOptions? editBy = null
        );

        Task<ContactApiResult> UpdateAsync(ContactModel entity);

        Task<ContactApiResult> UpdateAsync(
            string contactId,
            string attention = null,
            string title = null,
            string company = null,
            string companyDepartment = null,
            string firstName = null,
            string lastName = null,
            string position = null,
            string streetAddress = null,
            string suburb = null,
            string city = null,
            string state = null,
            string country = null,
            string postcode = null,
            string mainPhone = null,
            string altPhone1 = null,
            string altPhone2 = null,
            string directPhone = null,
            string mobilePhone = null,
            string faxNumber = null,
            string emailAddress = null,
            string webAddress = null,
            string custom1 = null,
            string custom2 = null,
            string custom3 = null,
            string custom4 = null,
            Enums.ViewEditByOptions? viewBy = null,
            Enums.ViewEditByOptions? editBy = null
        );

        ContactApiResult Delete(ContactModel entity);
        ContactApiResult DeleteById(string contactID);
        Task<ContactApiResult> DeleteAsync(ContactModel entity);
        Task<ContactApiResult> DeleteByIdAsync(string contactID);
        ContactApiResult Get(ContactModel entity);
        ContactApiResult GetById(string contactID);
        Task<ContactApiResult> GetAsync(ContactModel entity);
        Task<ContactApiResult> GetByIdAsync(string contactID);
        ContactApiResult Read(ContactModel entity);
        ContactApiResult ReadById(string contactId);
        Task<ContactApiResult> ReadAsync(ContactModel entity);
        Task<ContactApiResult> ReadByIdAsync(string contactID);
        

        ContactListApiResult List();
        ContactListApiResult List(IListRequestOptions options);
        Task<ContactListApiResult> ListAsync();
        Task<ContactListApiResult> ListAsync(IListRequestOptions options);

    }
}