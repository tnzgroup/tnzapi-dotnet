using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Api.Messaging.Common.Enums;

namespace TNZAPI.NET.Samples.Addressbook.Contacts
{
    public class UpdateContact
    {
        private readonly ITNZAuth apiUser;

        public ContactModel Contact { get; set; }

        public UpdateContact(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            Contact = new ContactModel();
        }

        public UpdateContact(ITNZAuth apiUser, ContactModel contact)
        {
            this.apiUser = apiUser;

            Contact = contact;
        }

        public void Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Update(Contact);

            if (response.Result == ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
                Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
                Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
                Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
                Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
                Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
                Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
                Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
                Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
                Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
                Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
                Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
                Console.WriteLine($"    -> City: '{response.Contact.City}'");
                Console.WriteLine($"    -> State: '{response.Contact.State}'");
                Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
                Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
                Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
                Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
                Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
                Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
                Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
                Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
                Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
                Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
                Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
                Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
                Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
                Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
                Console.WriteLine($"-------------------------");
            }
            else
            {
                Console.WriteLine($"Could not find any contact with ID={Contact.ID}");
            }
        }
    }
}
