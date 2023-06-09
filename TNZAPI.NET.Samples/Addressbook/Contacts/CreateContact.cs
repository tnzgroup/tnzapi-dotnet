using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Api.Messaging.Common.Enums;

namespace TNZAPI.NET.Samples.Addressbook.Contacts
{
    public class CreateContact
    {
        private readonly ITNZAuth apiUser;

        public ContactModel Contact;

        public CreateContact(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            Contact = new ContactModel();
        }

        public CreateContact(ITNZAuth apiUser, ContactModel contact)
        {
            this.apiUser = apiUser;

            Contact = contact;
        }

        public ContactModel? Basic()
        {
            var request = new TNZApiClient(apiUser);

            if (Contact is null)
            {
                Contact = new ContactBuilder()
                                .SetAttention("API Test")
                                .SetFirstName("API")
                                .SetLastName("Test")
                                .Build();
            }

            var requestResult = request.Addressbook.Contact.Create(Contact);

            if (requestResult.Result == ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={requestResult.Contact.ID}");
                Console.WriteLine($"    -> Owner: '{requestResult.Contact.Owner}'");
                Console.WriteLine($"    -> Created: '{requestResult.Contact.Created}'");
                Console.WriteLine($"    -> Updated: '{requestResult.Contact.Updated}'");
                Console.WriteLine($"    -> Attention: '{requestResult.Contact.Attention}'");
                Console.WriteLine($"    -> Company: '{requestResult.Contact.Company}'");
                Console.WriteLine($"    -> RecipDepartment: '{requestResult.Contact.CompanyDepartment}'");
                Console.WriteLine($"    -> FirstName: '{requestResult.Contact.FirstName}'");
                Console.WriteLine($"    -> LastName: '{requestResult.Contact.LastName}'");
                Console.WriteLine($"    -> Position: '{requestResult.Contact.Position}'");
                Console.WriteLine($"    -> StreetAddress: '{requestResult.Contact.StreetAddress}'");
                Console.WriteLine($"    -> Suburb: '{requestResult.Contact.Suburb}'");
                Console.WriteLine($"    -> City: '{requestResult.Contact.City}'");
                Console.WriteLine($"    -> State: '{requestResult.Contact.State}'");
                Console.WriteLine($"    -> Country: '{requestResult.Contact.Country}'");
                Console.WriteLine($"    -> Postcode: '{requestResult.Contact.Postcode}'");
                Console.WriteLine($"    -> MainPhone: '{requestResult.Contact.MainPhone}'");
                Console.WriteLine($"    -> AltPhone1: '{requestResult.Contact.AltPhone1}'");
                Console.WriteLine($"    -> AltPhone2: '{requestResult.Contact.AltPhone2}'");
                Console.WriteLine($"    -> DirectPhone: '{requestResult.Contact.DirectPhone}'");
                Console.WriteLine($"    -> MobilePhone: '{requestResult.Contact.MobilePhone}'");
                Console.WriteLine($"    -> FaxNumber: '{requestResult.Contact.FaxNumber}'");
                Console.WriteLine($"    -> EmailAddress: '{requestResult.Contact.EmailAddress}'");
                Console.WriteLine($"    -> WebAddress: '{requestResult.Contact.WebAddress}'");
                Console.WriteLine($"    -> Custom1: '{requestResult.Contact.Custom1}'");
                Console.WriteLine($"    -> Custom2: '{requestResult.Contact.Custom2}'");
                Console.WriteLine($"    -> Custom3: '{requestResult.Contact.Custom3}'");
                Console.WriteLine($"    -> Custom4: '{requestResult.Contact.Custom4}'");
                Console.WriteLine($"-------------------------");

                return requestResult.Contact;
            }
            else
            {
                Console.WriteLine($"Could not find any contact with ID={Contact.ID}");
            }

            return null;
        }
    }
}
