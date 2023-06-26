using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Contact.Groups
{
    public class GetContactGroupList
    {
        private readonly ITNZAuth apiUser;

        public ContactModel Contact { get; set; }

        public GetContactGroupList(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            Contact = new ContactModel();
        }

        public GetContactGroupList(ITNZAuth apiUser, ContactModel contact)
        {
            this.apiUser = apiUser;

            Contact = contact;
        }

        public ContactGroupListApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.ContactGroupList.List(
                Contact,                // ContactModel
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

        public ContactGroupListApiResult Simple() => Basic();    // Same as Basic

        public ContactGroupListApiResult Builder() => Basic();   // Same as Basic

        public ContactGroupListApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.ContactGroupList.List(
                Contact,                        // Contact
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
