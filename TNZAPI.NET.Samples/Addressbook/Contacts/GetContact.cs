using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Samples.Addressbook.Contacts
{
    public class GetContact
    {
        private readonly ITNZAuth apiUser;

        public GetContact(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        #region Run()
        public ContactApiResult Run(ContactModel contact)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Read(contact);

            if (response.Result == Enums.ResultCode.Success)
            {
                response.Contact.Dump();
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
        #endregion

        public ContactApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Read(new ContactID("AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD"));

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={response.Contact.ContactID}");
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

        public ContactApiResult Simple() => Basic();    // Same as Basic

        public ContactApiResult Builder()
        {
            var client = new TNZApiClient(apiUser);

            var contact = new ContactBuilder(new ContactID("AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD"))
                            .Build();

            var response = client.Addressbook.Contact.Read(contact);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={response.Contact.ContactID}");
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

        public ContactApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Read(
                new ContactModel()
                {
                    ContactID = new ContactID("AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
                }
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={response.Contact.ContactID}");
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
