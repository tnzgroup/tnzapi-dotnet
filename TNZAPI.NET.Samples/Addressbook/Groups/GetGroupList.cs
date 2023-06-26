﻿using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Builders;

namespace TNZAPI.NET.Samples.Addressbook.Groups
{
    public class GetGroupList
    {
        private readonly ITNZAuth apiUser;

        public GetGroupList(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public GroupListApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.GroupList.List(
                recordsPerPage: 10,     // Record per page
                page: 1                 // Page number
                );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

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

        public GroupListApiResult Simple() => Basic();    // Same as Basic

        public GroupListApiResult Builder(GroupListRequestOptions? listOptions = null)
        {
            var client = new TNZApiClient(apiUser);

            if (listOptions is null)
            {
                listOptions = new ListRequestOptionBuilder<GroupListRequestOptions>()
                    .SetRecordsPerPage(100)         // Number of records for this request
                    .SetPage(1)                     // Current location
                    .Build();
            }

            var response = client.Addressbook.GroupList.List(listOptions);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

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

        public GroupListApiResult Advanced(GroupListRequestOptions? listOptions = null)
        {
            var client = new TNZApiClient(apiUser);

            if (listOptions is null)
            {
                listOptions = new GroupListRequestOptions()
                {
                    RecordsPerPage = 10,    // Record per page
                    Page = 1                // Page number
                };
            }

            var response = client.Addressbook.GroupList.List(listOptions);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact list details");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

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
