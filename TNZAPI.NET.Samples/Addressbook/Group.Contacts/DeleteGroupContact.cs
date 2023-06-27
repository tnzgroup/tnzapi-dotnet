using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Samples.Addressbook.Group.Contacts
{
    public class DeleteGroupContact
    {
        private readonly ITNZAuth apiUser;

        public DeleteGroupContact(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        #region Run()
        public GroupContactApiResult Run(GroupModel group, ContactModel contact)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.GroupContact.Remove(group, contact);

            if (response.Result == Enums.ResultCode.Success)
            {
                if (response.Group is not null)
                {
                    response.Group.Dump();
                    Console.WriteLine($"-------------------------");
                }
                if (response.Contact is not null)
                {
                    response.Contact.Dump();
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

            return response;
        }
        #endregion

        public GroupContactApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var groupCode = "Test-Group";

            var contactID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD";

            var response = client.Addressbook.GroupContact.Remove(groupCode, contactID);

            if (response.Result == Enums.ResultCode.Success)
            {
                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
                    Console.WriteLine($"-------------------------");
                }
                if (response.Contact is not null)
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

        public GroupContactApiResult Simple()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.GroupContact.Remove(
                groupCode: "Test-Group",                                // Group Code
                contactID: "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD"       // Contact ID
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
                    Console.WriteLine($"-------------------------");
                }
                if (response.Contact is not null)
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

        public GroupContactApiResult Builder()
        {
            var client = new TNZApiClient(apiUser);

            var group = new GroupBuilder("Test-Group").Build();

            var contact = new ContactBuilder("AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD").Build();

            var response = client.Addressbook.GroupContact.Remove(group, contact);

            if (response.Result == Enums.ResultCode.Success)
            {
                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
                    Console.WriteLine($"-------------------------");
                }
                if (response.Contact is not null)
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

        public GroupContactApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.GroupContact.Remove(
                new GroupModel()
                {
                    GroupCode = "Test-Group"                        // Group Code
                },
                new ContactModel()
                {
                    ID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD"     // Contact ID
                });

            if (response.Result == Enums.ResultCode.Success)
            {
                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
                    Console.WriteLine($"-------------------------");
                }
                if (response.Contact is not null)
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
