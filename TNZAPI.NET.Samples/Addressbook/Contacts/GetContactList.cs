using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Builders;

namespace TNZAPI.NET.Samples.Addressbook.Contacts
{
    public class GetContactList
    {
        private readonly ITNZAuth apiUser;

        public GetContactList(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public void Basic()
        {
            var request = new TNZApiClient(apiUser);

            var response = request.Addressbook.ContactList.List(
                recordsPerPage: 10,
                page: 1
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                foreach (var contact in response.Contacts)
                {
                    Console.WriteLine($"Contact details for ContactID={contact.ID}");
                    Console.WriteLine($"    -> Owner: '{contact.Owner}'");
                    Console.WriteLine($"    -> Created: '{contact.Created}'");
                    Console.WriteLine($"    -> Updated: '{contact.Updated}'");
                    Console.WriteLine($"    -> Attention: '{contact.Attention}'");
                    Console.WriteLine($"    -> Company: '{contact.Company}'");
                    Console.WriteLine($"    -> RecipDepartment: '{contact.CompanyDepartment}'");
                    Console.WriteLine($"    -> FirstName: '{contact.FirstName}'");
                    Console.WriteLine($"    -> LastName: '{contact.LastName}'");
                    Console.WriteLine($"    -> Position: '{contact.Position}'");
                    Console.WriteLine($"    -> StreetAddress: '{contact.StreetAddress}'");
                    Console.WriteLine($"    -> Suburb: '{contact.Suburb}'");
                    Console.WriteLine($"    -> City: '{contact.City}'");
                    Console.WriteLine($"    -> State: '{contact.State}'");
                    Console.WriteLine($"    -> Country: '{contact.Country}'");
                    Console.WriteLine($"    -> Postcode: '{contact.Postcode}'");
                    Console.WriteLine($"    -> MainPhone: '{contact.MainPhone}'");
                    Console.WriteLine($"    -> AltPhone1: '{contact.AltPhone1}'");
                    Console.WriteLine($"    -> AltPhone2: '{contact.AltPhone2}'");
                    Console.WriteLine($"    -> DirectPhone: '{contact.DirectPhone}'");
                    Console.WriteLine($"    -> MobilePhone: '{contact.MobilePhone}'");
                    Console.WriteLine($"    -> FaxNumber: '{contact.FaxNumber}'");
                    Console.WriteLine($"    -> EmailAddress: '{contact.EmailAddress}'");
                    Console.WriteLine($"    -> WebAddress: '{contact.WebAddress}'");
                    Console.WriteLine($"    -> Custom1: '{contact.Custom1}'");
                    Console.WriteLine($"    -> Custom2: '{contact.Custom2}'");
                    Console.WriteLine($"    -> Custom3: '{contact.Custom3}'");
                    Console.WriteLine($"    -> Custom4: '{contact.Custom4}'");
                    Console.WriteLine($"-------------------------");
                }


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

        public void Simple() => Basic();    // Same as Basic

        public void Builder()
        {
            var request = new TNZApiClient(apiUser);

            var options = new ListRequestOptionBuilder<ContactListRequestOptions>()
                                .SetRecordsPerPage(10)
                                .SetPage(1)
                                .Build();

            var response = request.Addressbook.ContactList.List(options);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                foreach (var contact in response.Contacts)
                {
                    Console.WriteLine($"Contact details for ContactID={contact.ID}");
                    Console.WriteLine($"    -> Owner: '{contact.Owner}'");
                    Console.WriteLine($"    -> Created: '{contact.Created}'");
                    Console.WriteLine($"    -> Updated: '{contact.Updated}'");
                    Console.WriteLine($"    -> Attention: '{contact.Attention}'");
                    Console.WriteLine($"    -> Company: '{contact.Company}'");
                    Console.WriteLine($"    -> RecipDepartment: '{contact.CompanyDepartment}'");
                    Console.WriteLine($"    -> FirstName: '{contact.FirstName}'");
                    Console.WriteLine($"    -> LastName: '{contact.LastName}'");
                    Console.WriteLine($"    -> Position: '{contact.Position}'");
                    Console.WriteLine($"    -> StreetAddress: '{contact.StreetAddress}'");
                    Console.WriteLine($"    -> Suburb: '{contact.Suburb}'");
                    Console.WriteLine($"    -> City: '{contact.City}'");
                    Console.WriteLine($"    -> State: '{contact.State}'");
                    Console.WriteLine($"    -> Country: '{contact.Country}'");
                    Console.WriteLine($"    -> Postcode: '{contact.Postcode}'");
                    Console.WriteLine($"    -> MainPhone: '{contact.MainPhone}'");
                    Console.WriteLine($"    -> AltPhone1: '{contact.AltPhone1}'");
                    Console.WriteLine($"    -> AltPhone2: '{contact.AltPhone2}'");
                    Console.WriteLine($"    -> DirectPhone: '{contact.DirectPhone}'");
                    Console.WriteLine($"    -> MobilePhone: '{contact.MobilePhone}'");
                    Console.WriteLine($"    -> FaxNumber: '{contact.FaxNumber}'");
                    Console.WriteLine($"    -> EmailAddress: '{contact.EmailAddress}'");
                    Console.WriteLine($"    -> WebAddress: '{contact.WebAddress}'");
                    Console.WriteLine($"    -> Custom1: '{contact.Custom1}'");
                    Console.WriteLine($"    -> Custom2: '{contact.Custom2}'");
                    Console.WriteLine($"    -> Custom3: '{contact.Custom3}'");
                    Console.WriteLine($"    -> Custom4: '{contact.Custom4}'");
                    Console.WriteLine($"-------------------------");
                }


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

            var response = request.Addressbook.ContactList.List(
                new ContactListRequestOptions()
                {
                    RecordsPerPage = 10,
                    Page = 1
                }
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                foreach (var contact in response.Contacts)
                {
                    Console.WriteLine($"Contact details for ContactID={contact.ID}");
                    Console.WriteLine($"    -> Owner: '{contact.Owner}'");
                    Console.WriteLine($"    -> Created: '{contact.Created}'");
                    Console.WriteLine($"    -> Updated: '{contact.Updated}'");
                    Console.WriteLine($"    -> Attention: '{contact.Attention}'");
                    Console.WriteLine($"    -> Company: '{contact.Company}'");
                    Console.WriteLine($"    -> RecipDepartment: '{contact.CompanyDepartment}'");
                    Console.WriteLine($"    -> FirstName: '{contact.FirstName}'");
                    Console.WriteLine($"    -> LastName: '{contact.LastName}'");
                    Console.WriteLine($"    -> Position: '{contact.Position}'");
                    Console.WriteLine($"    -> StreetAddress: '{contact.StreetAddress}'");
                    Console.WriteLine($"    -> Suburb: '{contact.Suburb}'");
                    Console.WriteLine($"    -> City: '{contact.City}'");
                    Console.WriteLine($"    -> State: '{contact.State}'");
                    Console.WriteLine($"    -> Country: '{contact.Country}'");
                    Console.WriteLine($"    -> Postcode: '{contact.Postcode}'");
                    Console.WriteLine($"    -> MainPhone: '{contact.MainPhone}'");
                    Console.WriteLine($"    -> AltPhone1: '{contact.AltPhone1}'");
                    Console.WriteLine($"    -> AltPhone2: '{contact.AltPhone2}'");
                    Console.WriteLine($"    -> DirectPhone: '{contact.DirectPhone}'");
                    Console.WriteLine($"    -> MobilePhone: '{contact.MobilePhone}'");
                    Console.WriteLine($"    -> FaxNumber: '{contact.FaxNumber}'");
                    Console.WriteLine($"    -> EmailAddress: '{contact.EmailAddress}'");
                    Console.WriteLine($"    -> WebAddress: '{contact.WebAddress}'");
                    Console.WriteLine($"    -> Custom1: '{contact.Custom1}'");
                    Console.WriteLine($"    -> Custom2: '{contact.Custom2}'");
                    Console.WriteLine($"    -> Custom3: '{contact.Custom3}'");
                    Console.WriteLine($"    -> Custom4: '{contact.Custom4}'");
                    Console.WriteLine($"-------------------------");
                }
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
