using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Builders;
using TNZAPI.NET.Core.Common;
using TNZAPI.NET.Helpers;

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

        #region Run()
        public ContactGroupListApiResult Run(ContactModel contact, int recordsPerPage = 50, int page = 1)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.ContactGroupList.List(
                contact,                                    // Contact
                new ListRequestOptions()
                {
                    RecordsPerPage = recordsPerPage,        // Record per page
                    Page = page                             // Page number
                }
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                response.Dump();
                Console.WriteLine($"-------------------------");

                response.Contact.Dump();
                Console.WriteLine($"-------------------------");

                if (response.Groups is not null)
                {
                    foreach (var group in response.Groups)
                    {
                        group.Dump();
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
        #endregion

        public ContactGroupListApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var contactID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD";

            var response = client.Addressbook.ContactGroupList.ListById(
                contactID,              // ContactID
                page: 1                 // Page number
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Contact ID: {response.Contact.ContactID}");
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

        public ContactGroupListApiResult Simple()
        {
            var client = new TNZApiClient(apiUser);

            var contactID = "AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD";

            var response = client.Addressbook.ContactGroupList.ListById(
                contactID,              // ContactID
                recordsPerPage: 10,     // Record per page
                page: 1                 // Page number
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Contact ID: {response.Contact.ContactID}");
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

        public ContactGroupListApiResult Builder()
        {
            var client = new TNZApiClient(apiUser);

            var contact = new ContactBuilder("AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
                            .Build();

            var listOptions = new ListRequestOptionBuilder()
                                .SetRecordsPerPage(100)
                                .SetPage(1)
                                .Build();

            var response = client.Addressbook.ContactGroupList.List(
                contact,            // Contact
                listOptions         // ListRequestOptions
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Contact ID: {response.Contact.ContactID}");
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

        public ContactGroupListApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.ContactGroupList.List(
                new ContactModel()              // ContactModel
                {
                    ContactID = new("AAAAAAAA-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
                },
                new ListRequestOptions()
                {
                    RecordsPerPage = 10,        // Record per page
                    Page = 1                    // Page number
                }
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Contact ID: {response.Contact.ContactID}");
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
