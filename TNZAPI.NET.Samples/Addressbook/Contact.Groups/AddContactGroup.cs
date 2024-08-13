using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Samples.Addressbook.Contact.Groups
{
    public class AddContactGroup
    {
        private readonly ITNZAuth apiUser;

        public AddContactGroup(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        #region Run()
        public ContactGroupApiResult Run(ContactModel contact, GroupModel group)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.ContactGroup.Add(
                contact: contact,
                group: group
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                response.Contact.Dump();
                Console.WriteLine($"-------------------------");

                if (response.Group is not null)
                {
                    response.Group.Dump();
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

        public ContactGroupApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");      // ContactID
            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");          // GroupID

            var response = client.Addressbook.ContactGroup.Add(contactID, groupID);

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

                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupID}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
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

        public ContactGroupApiResult Simple()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.ContactGroup.Add(
                contactID: new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD"),       // ContactID
                groupID: new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD")            // GroupID
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

                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupID}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
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

        public ContactGroupApiResult Builder()
        {
            var client = new TNZApiClient(apiUser);

            var contact = new ContactBuilder(new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD"))
                            .Build();

            var group = new GroupBuilder(new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD"))
                            .Build();

            var response = client.Addressbook.ContactGroup.Add(
                contact: contact,
                group: group
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

                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupID}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
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

        public ContactGroupApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.ContactGroup.Add(
                contact: new ContactModel()                                                 // ContactModel
                {
                    ContactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD")       // ContactID
                },
                group: new GroupModel()                                                     // GroupModel
                {
                    GroupCode = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD")         // GroupID
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

                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupID}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
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
