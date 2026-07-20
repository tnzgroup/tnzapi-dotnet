using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Contact
{
    /// <summary>
    /// Reference code demonstrating client.Addressbook.Contact CRUD operations.
    /// This class is not a runnable program — call these methods from your own application.
    /// See README.md#addressbook for the full reference.
    /// </summary>
    public class ContactSamples
    {
        private readonly ITNZAuth apiUser;

        public ContactSamples()
        {
            apiUser = new TNZApiUser();
        }

        public ContactSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public ContactApiResult Create()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new ContactBuilder();

            var contact = builder
                .SetAttention("API Test")
                .SetFirstName("API")
                .SetLastName("Test")
                .SetMobilePhone("+64211231234")
                .SetEmailAddress("test@example.com")
                .SetMainPhone("+6491112222")
                .Build();

            var response = client.Addressbook.Contact.Create(contact);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Created ContactID={response.ContactID}");
            }
            else
            {
                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }

        public ContactApiResult Details(ContactID contactID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Details(contactID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={response.ContactID}");
                Console.WriteLine($"    -> Owner: '{response.Owner}'");
                Console.WriteLine($"    -> Attention: '{response.Attention}'");
                Console.WriteLine($"    -> Company: '{response.Company}'");
                Console.WriteLine($"    -> FirstName: '{response.FirstName}'");
                Console.WriteLine($"    -> LastName: '{response.LastName}'");
                Console.WriteLine($"    -> MainPhone: '{response.MainPhone}'");
                Console.WriteLine($"    -> MobilePhone: '{response.MobilePhone}'");
                Console.WriteLine($"    -> EmailAddress: '{response.EmailAddress}'");
            }

            return response;
        }

        public ContactApiResult Update(ContactID contactID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Update(contactID, new ContactModel
            {
                Company = "Example Company"
            });

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Updated ContactID={response.ContactID}, Company='{response.Company}'");
            }

            return response;
        }

        public ContactApiResult Delete(ContactID contactID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Delete(contactID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Deleted ContactID={response.ContactID}");
            }

            return response;
        }

        public ContactListApiResult Search()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Search(emailAddress: "test@example.com");

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var contact in response.Contacts)
                {
                    Console.WriteLine($"{contact.ContactID}: {contact.FirstName} {contact.LastName}");
                }
            }

            return response;
        }

        public ContactListApiResult List()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.List(page: 1, recordsPerPage: 100);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"TotalRecords: {response.TotalRecords}, Page: {response.Page}/{response.PageCount}");

                foreach (var contact in response.Contacts)
                {
                    Console.WriteLine($"{contact.ContactID}: {contact.FirstName} {contact.LastName}");
                }
            }

            return response;
        }
    }
}