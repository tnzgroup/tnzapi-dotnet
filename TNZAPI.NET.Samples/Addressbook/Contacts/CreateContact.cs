using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;
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

            var result = request.Addressbook.Contact.Create(
                attention: "Test Person",
                firstName: "Test",
                lastName: "Person",
                mobilePhone: "+6421000001"
                );

            if (result.Result == ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={result.Contact.ID}");
                Console.WriteLine(result.Contact.Dump());

                return result.Contact;
            }
            else
            {
                Console.WriteLine($"Could not create contact.");
            }

            return null;
        }

        public ContactModel? Simple()
        {
            var request = new TNZApiClient(apiUser);

            var result = request.Addressbook.Contact.Create(
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

            if (result.Result == ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={result.Contact.ID}");
                Console.WriteLine($"    -> Owner: '{result.Contact.Owner}'");
                Console.WriteLine($"    -> Created: '{result.Contact.Created}'");
                Console.WriteLine($"    -> Updated: '{result.Contact.Updated}'");
                Console.WriteLine($"    -> Attention: '{result.Contact.Attention}'");
                Console.WriteLine($"    -> Company: '{result.Contact.Company}'");
                Console.WriteLine($"    -> RecipDepartment: '{result.Contact.CompanyDepartment}'");
                Console.WriteLine($"    -> FirstName: '{result.Contact.FirstName}'");
                Console.WriteLine($"    -> LastName: '{result.Contact.LastName}'");
                Console.WriteLine($"    -> Position: '{result.Contact.Position}'");
                Console.WriteLine($"    -> StreetAddress: '{result.Contact.StreetAddress}'");
                Console.WriteLine($"    -> Suburb: '{result.Contact.Suburb}'");
                Console.WriteLine($"    -> City: '{result.Contact.City}'");
                Console.WriteLine($"    -> State: '{result.Contact.State}'");
                Console.WriteLine($"    -> Country: '{result.Contact.Country}'");
                Console.WriteLine($"    -> Postcode: '{result.Contact.Postcode}'");
                Console.WriteLine($"    -> MainPhone: '{result.Contact.MainPhone}'");
                Console.WriteLine($"    -> AltPhone1: '{result.Contact.AltPhone1}'");
                Console.WriteLine($"    -> AltPhone2: '{result.Contact.AltPhone2}'");
                Console.WriteLine($"    -> DirectPhone: '{result.Contact.DirectPhone}'");
                Console.WriteLine($"    -> MobilePhone: '{result.Contact.MobilePhone}'");
                Console.WriteLine($"    -> FaxNumber: '{result.Contact.FaxNumber}'");
                Console.WriteLine($"    -> EmailAddress: '{result.Contact.EmailAddress}'");
                Console.WriteLine($"    -> WebAddress: '{result.Contact.WebAddress}'");
                Console.WriteLine($"    -> Custom1: '{result.Contact.Custom1}'");
                Console.WriteLine($"    -> Custom2: '{result.Contact.Custom2}'");
                Console.WriteLine($"    -> Custom3: '{result.Contact.Custom3}'");
                Console.WriteLine($"    -> Custom4: '{result.Contact.Custom4}'");
                Console.WriteLine($"-------------------------");

                return result.Contact;
            }
            else
            {
                Console.WriteLine($"Could not create contact.");
            }

            return null;
        }

        public ContactModel? Builder()
        {
            var request = new TNZApiClient(apiUser);

            if (Contact is null)
            {
                Contact = new ContactBuilder()
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

        public ContactModel? Advanced()
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

                return response.Contact;
            }
            else
            {
                Console.WriteLine($"Could not find any contact with ID={Contact.ID}");
            }

            return null;
        }
    }
}
