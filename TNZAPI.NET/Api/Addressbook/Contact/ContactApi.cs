using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Addressbook;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Contact
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ContactApi : IContactApi
    {
        private ITNZAuth User = new TNZApiUser();

        public IContactGroupApi Group { get; set; }

        public ContactApi()
        {
            Group = new ContactGroupApi(User);
        }

        public ContactApi(string authToken)
        {
            User.AuthToken = authToken;
            Group = new ContactGroupApi(User);
        }

        public ContactApi(TNZApiUser apiUser)
        {
            User = apiUser;
            Group = new ContactGroupApi(User);
        }

        public ContactApi(ITNZAuth auth)
        {
            User = auth;
            Group = new ContactGroupApi(User);
        }

        #region Set API User
        public void SetAPIUser(ITNZAuth apiUser)
        {
            User = apiUser;
            Group.SetAPIUser(apiUser);
        }

        public void SetAuthToken(string authToken)
        {
            User.AuthToken = authToken;
        }
        #endregion

        private static string BuildAPIURL()
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact";
        }

        private static string BuildContactURL(ContactID contactID)
        {
            IDGuard.EnsureProvided(contactID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/{Uri.EscapeDataString(contactID.Value!)}";
        }

        #region Create
        public ContactApiResult Create(ContactModel entity)
        {
            return Task.Run(() => CreateAsync(entity)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactApiResult> CreateAsync(ContactModel entity)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty AuthToken: Please specify AuthToken");
            }

            return await HttpRequest.PostAsync<ContactApiResult>(BuildAPIURL(), User, entity);
        }
        #endregion

        #region Details
        public ContactApiResult Details(ContactID contactID)
        {
            return Task.Run(() => DetailsAsync(contactID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactApiResult> DetailsAsync(ContactID contactID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty ContactID: Please specify ContactID");
            }

            return await HttpRequest.GetAsync<ContactApiResult>(BuildContactURL(contactID), User);
        }
        #endregion

        #region Update
        public ContactApiResult Update(ContactID contactID, ContactModel entity)
        {
            return Task.Run(() => UpdateAsync(contactID, entity)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactApiResult> UpdateAsync(ContactID contactID, ContactModel entity)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty ContactID: Please specify ContactID");
            }

            return await HttpRequest.PatchAsync<ContactApiResult>(BuildContactURL(contactID), User, entity);
        }
        #endregion

        #region Delete
        public ContactApiResult Delete(ContactID contactID)
        {
            return Task.Run(() => DeleteAsync(contactID)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactApiResult> DeleteAsync(ContactID contactID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (contactID is null || string.IsNullOrEmpty(contactID.Value))
            {
                return ResultHelper.RespondError<ContactApiResult>("Empty ContactID: Please specify ContactID");
            }

            return await HttpRequest.DeleteAsync<ContactApiResult>(BuildContactURL(contactID), User);
        }
        #endregion

        private static string BuildListURL(int page, int recordsPerPage)
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/list?page={page}&recordsPerPage={recordsPerPage}";
        }

        #region List
        public ContactListApiResult List(int page = 1, int recordsPerPage = 100)
        {
            return Task.Run(() => ListAsync(page, recordsPerPage)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactListApiResult> ListAsync(int page = 1, int recordsPerPage = 100)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactListApiResult>("Empty AuthToken: Please specify AuthToken");
            }

            return await HttpRequest.GetAsync<ContactListApiResult>(BuildListURL(page, recordsPerPage), User);
        }
        #endregion

        private static string BuildSearchURL(string? emailAddress, string? mobilePhone, string? mainPhone, string? attention, string? firstName, string? lastName, string? company, int page, int recordsPerPage)
        {
            var query = new List<string> { $"page={page}", $"recordsPerPage={recordsPerPage}" };

            if (!string.IsNullOrEmpty(emailAddress)) query.Add($"EmailAddress={Uri.EscapeDataString(emailAddress)}");
            if (!string.IsNullOrEmpty(mobilePhone)) query.Add($"MobilePhone={Uri.EscapeDataString(mobilePhone)}");
            if (!string.IsNullOrEmpty(mainPhone)) query.Add($"MainPhone={Uri.EscapeDataString(mainPhone)}");
            if (!string.IsNullOrEmpty(attention)) query.Add($"Attention={Uri.EscapeDataString(attention)}");
            if (!string.IsNullOrEmpty(firstName)) query.Add($"FirstName={Uri.EscapeDataString(firstName)}");
            if (!string.IsNullOrEmpty(lastName)) query.Add($"LastName={Uri.EscapeDataString(lastName)}");
            if (!string.IsNullOrEmpty(company)) query.Add($"Company={Uri.EscapeDataString(company)}");

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/addressbook/contact/search?{string.Join("&", query)}";
        }

        #region Search
        public ContactListApiResult Search(string? emailAddress = null, string? mobilePhone = null, string? mainPhone = null, string? attention = null, string? firstName = null, string? lastName = null, string? company = null, int page = 1, int recordsPerPage = 100)
        {
            return Task.Run(() => SearchAsync(emailAddress, mobilePhone, mainPhone, attention, firstName, lastName, company, page, recordsPerPage)).Result;
        }

        [ComVisible(false)]
        public async Task<ContactListApiResult> SearchAsync(string? emailAddress = null, string? mobilePhone = null, string? mainPhone = null, string? attention = null, string? firstName = null, string? lastName = null, string? company = null, int page = 1, int recordsPerPage = 100)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<ContactListApiResult>("Empty AuthToken: Please specify AuthToken");
            }

            return await HttpRequest.GetAsync<ContactListApiResult>(
                BuildSearchURL(emailAddress, mobilePhone, mainPhone, attention, firstName, lastName, company, page, recordsPerPage), User);
        }
        #endregion
    }
}