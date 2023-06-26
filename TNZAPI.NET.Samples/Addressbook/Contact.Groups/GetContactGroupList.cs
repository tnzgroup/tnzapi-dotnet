using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Builders;

namespace TNZAPI.NET.Samples.Addressbook.Contact.Groups
{
    public class GetContactGroupList
    {
        private readonly ITNZAuth apiUser;

        public GetContactGroupList(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public GetContactGroupList(ITNZAuth apiUser, ContactModel contact)
        {
            this.apiUser = apiUser;
        }

        public ContactGroupListApiResult Basic(ContactModel? contact = null)
        {
            var client = new TNZApiClient(apiUser);

            if (contact is null)
            {
                contact = new ContactModel()
                {
                    ID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD"
                };
            }

            var response = client.Addressbook.ContactGroupList.List(
                contact,                // ContactModel
                recordsPerPage: 10,     // Record per page
                page: 1                 // Page number
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Contact ID: {response.Contact.ID}");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                if (response.Groups is not null)
                {
                    foreach (var group in response.Groups)
                    {
                        Console.WriteLine($"Group details for GroupCode={group.GroupCode}");
                        Console.WriteLine($"    -> GroupCode: '{group.GroupCode}'");
                        Console.WriteLine($"    -> GroupName: '{group.GroupName}'");
                        Console.WriteLine($"    -> SubAccount: '{group.SubAccount}'");
                        Console.WriteLine($"    -> Department: '{group.Department}'");
                        Console.WriteLine($"    -> ViewEditBy: '{group.ViewEditBy}'");
                        Console.WriteLine($"    -> Owner: '{group.Owner}'");
                        Console.WriteLine($"-------------------------");
                    }
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

        public ContactGroupListApiResult Simple(ContactModel? contact = null)
        {
            var client = new TNZApiClient(apiUser);

            if (contact is null)
            {
                contact = new ContactModel()
                {
                    ID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD"
                };
            }

            var response = client.Addressbook.ContactGroupList.List(
                contact,                // ContactModel
                recordsPerPage: 10,     // Record per page
                page: 1                 // Page number
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Contact ID: {response.Contact.ID}");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                if (response.Groups is not null)
                {
                    foreach (var group in response.Groups)
                    {
                        Console.WriteLine($"Group details for GroupCode={group.GroupCode}");
                        Console.WriteLine($"    -> GroupCode: '{group.GroupCode}'");
                        Console.WriteLine($"    -> GroupName: '{group.GroupName}'");
                        Console.WriteLine($"    -> SubAccount: '{group.SubAccount}'");
                        Console.WriteLine($"    -> Department: '{group.Department}'");
                        Console.WriteLine($"    -> ViewEditBy: '{group.ViewEditBy}'");
                        Console.WriteLine($"    -> Owner: '{group.Owner}'");
                        Console.WriteLine($"-------------------------");
                    }
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

        public ContactGroupListApiResult Builder(ContactModel? contact = null, ContactGroupListRequestOptions? listOptions = null)
        {
            var client = new TNZApiClient(apiUser);

            if (contact is null)
            {
                contact = new ContactBuilder("AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
                                .Build();
            }
            if (listOptions is null)
            {
                listOptions = new ListRequestOptionBuilder<ContactGroupListRequestOptions>()
                                    .SetRecordsPerPage(100)
                                    .SetPage(1)
                                    .Build();
            }

            var response = client.Addressbook.ContactGroupList.List(
                contact,            // Contact
                listOptions         // ListRequestOptions
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Contact ID: {response.Contact.ID}");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                if (response.Groups is not null)
                {
                    foreach (var group in response.Groups)
                    {
                        Console.WriteLine($"Group details for GroupCode={group.GroupCode}");
                        Console.WriteLine($"    -> GroupCode: '{group.GroupCode}'");
                        Console.WriteLine($"    -> GroupName: '{group.GroupName}'");
                        Console.WriteLine($"    -> SubAccount: '{group.SubAccount}'");
                        Console.WriteLine($"    -> Department: '{group.Department}'");
                        Console.WriteLine($"    -> ViewEditBy: '{group.ViewEditBy}'");
                        Console.WriteLine($"    -> Owner: '{group.Owner}'");
                        Console.WriteLine($"-------------------------");
                    }
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

        public ContactGroupListApiResult Advanced(ContactModel contact)
        {
            var client = new TNZApiClient(apiUser);

            if (contact is null)
            {
                contact = new ContactModel()
                {
                    ID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD"
                };
            }

            var response = client.Addressbook.ContactGroupList.List(
                contact,                        // Contact
                new ContactGroupListRequestOptions()
                {
                    RecordsPerPage = 10,        // Record per page
                    Page = 1                    // Page number
                }
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Contact ID: {response.Contact.ID}");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                if (response.Groups is not null)
                {
                    foreach (var group in response.Groups)
                    {
                        Console.WriteLine($"Group details for GroupCode={group.GroupCode}");
                        Console.WriteLine($"    -> GroupCode: '{group.GroupCode}'");
                        Console.WriteLine($"    -> GroupName: '{group.GroupName}'");
                        Console.WriteLine($"    -> SubAccount: '{group.SubAccount}'");
                        Console.WriteLine($"    -> Department: '{group.Department}'");
                        Console.WriteLine($"    -> ViewEditBy: '{group.ViewEditBy}'");
                        Console.WriteLine($"    -> Owner: '{group.Owner}'");
                        Console.WriteLine($"-------------------------");
                    }
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
