﻿using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Samples.Addressbook.Groups
{
    public class CreateGroup
    {
        private readonly ITNZAuth apiUser;

        public CreateGroup(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        #region Run(GroupModel group)
        public GroupApiResult Run(GroupModel group)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Create(group);

            if (response.Result == Enums.ResultCode.Success)
            {
                response.Group.Dump();
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

        public GroupApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Create(
                groupName: "Test Group"
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Group details for GroupID={response.Group.GroupID}");
                Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
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

        public GroupApiResult Simple()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Create(
                groupCode: "Test-Group",
                groupName: "Test Group",
                subAccount: "Test SubAccount",
                department: "Test Department",
                viewEditBy: Enums.ViewEditByOptions.Account
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Group details for GroupID={response.Group.GroupID}");
                Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
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

        public GroupApiResult Builder()
        {
            var client = new TNZApiClient(apiUser);

            var group = new GroupBuilder("Test-Group")
                            .SetGroupName("Test Group")
                            .SetSubAccount("Test SubAccount")
                            .SetDepartment("Test Department")
                            .SetViewEditBy(Enums.ViewEditByOptions.Account)
                            .Build();
                            

            var response = client.Addressbook.Group.Create(group);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Group details for GroupID={response.Group.GroupID}");
                Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
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

        public GroupApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Create(new GroupModel()
            {
                GroupCode = "Test-Group",
                GroupName = "Test Group",
                SubAccount = "Test SubAccount",
                Department = "Test Department",
                ViewEditBy = Enums.ViewEditByOptions.Account
            });

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Group details for GroupID={response.Group.GroupID}");
                Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
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
