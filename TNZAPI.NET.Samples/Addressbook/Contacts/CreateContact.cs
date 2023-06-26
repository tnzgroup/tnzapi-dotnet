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

        public ContactApiResult? Basic()
        {
            var request = new TNZApiClient(apiUser);

            var response = request.Addressbook.Contact.Create(
                attention: "Test Person",
                firstName: "Test",
                lastName: "Person",
                mobilePhone: "+6421000001"
                );

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

            return response;
        }

        public ContactApiResult? Simple()
        {
            var request = new TNZApiClient(apiUser);

            var response = request.Addressbook.Contact.Create(
                attention: "Test Person",
                firstName: "Test",
                lastName: "Person",
                mainPhone: "+6495005001",
                mobilePhone: "+6421000001",
                emailAddress: "recipient.one@example.com",
                faxNumber: "+6495005002",
                viewBy: ViewEditByOptions.Account,
                editBy: ViewEditByOptions.Account
                );

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

            return response;
        }

        public ContactApiResult? Builder()
        {
            var request = new TNZApiClient(apiUser);

            var contact = new ContactBuilder()
                            .SetAttention("Test Person")
                            .SetFirstName("Test")
                            .SetLastName("Person")
                            .SetMainPhone("+6495005001")
                            .SetMobilePhone("+6421000001")
                            .SetEmailAddress("recipient.one@example.com")
                            .SetFaxNumber("+6495005002")
                            .SetViewBy(ViewEditByOptions.Account)
                            .SetEditBy(ViewEditByOptions.Account)
                            .Build();

            var response = request.Addressbook.Contact.Create(contact);

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

            return response;
        }

        public ContactApiResult? Advanced()
        {
            var request = new TNZApiClient(apiUser);

            var response = request.Addressbook.Contact.Create(
                new ContactModel() 
                {
                    Attention = "Person Attention",
                    Title = "Mr",
                    Company = "TNZ Group LTD.",
                    CompanyDepartment = "",
                    FirstName = "Person first name",
                    LastName = "Person last name",
                    Position = "",
                    StreetAddress = "123 ABC st.",
                    Suburb = "My Suburb",
                    City = "Auckland",
                    State = "",
                    Country = "New Zealand",
                    Postcode = "1234",
                    MainPhone = "092223333",
                    AltPhone1 = "",
                    AltPhone2 = "",
                    DirectPhone = "",
                    MobilePhone = "0211144489",
                    FaxNumber = "093334444",
                    EmailAddress = "person1@example.com",
                    WebAddress = "",
                    Custom1 = "",
                    Custom2 = "",
                    Custom3 = "",
                    Custom4 = "",
                    ViewBy = ViewEditByOptions.Account,
                    EditBy = ViewEditByOptions.No
                }
            );

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

            return response;
        }
    }
}
