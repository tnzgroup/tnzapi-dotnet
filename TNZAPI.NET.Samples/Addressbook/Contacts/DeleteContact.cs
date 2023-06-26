using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Api.Messaging.Common.Enums;

namespace TNZAPI.NET.Samples.Addressbook.Contacts
{
    public class DeleteContact
    {
        private readonly ITNZAuth apiUser;

        public ContactModel Contact { get; set; }

        public DeleteContact(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            Contact = new ContactModel();
        }

        public DeleteContact(ITNZAuth apiUser, ContactModel contact)
        {
            this.apiUser = apiUser;

            Contact = contact;
        }

        public ContactApiResult? Basic()
        {
            var request = new TNZApiClient(apiUser);

            var contactID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD";

            var response = request.Addressbook.Contact.DeleteById(contactID);

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

                return response;
            }
            else
            {
                Console.WriteLine("Error occurred while processing.");

                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return null;
        }

        public ContactApiResult? Simple()
        {
            var request = new TNZApiClient(apiUser);

            var contactID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD";

            var response = request.Addressbook.Contact.DeleteById(contactID);

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

                return response;
            }
            else
            {
                Console.WriteLine("Error occurred while processing.");

                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return null;
        }

        public void Builder()
        {
            var request = new TNZApiClient(apiUser);

            var contact = new ContactBuilder("AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
                                .Build(); 

            var response = request.Addressbook.Contact.Delete(contact);

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
                Console.WriteLine("Error occurred while processing.");

                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }
        }

        public void Advanced()
        {
            var request = new TNZApiClient(apiUser);

            var response = request.Addressbook.Contact.Delete(new ContactModel()
            {
                ID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD"
            });

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
                Console.WriteLine("Error occurred while processing.");

                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }
        }
    }
}
