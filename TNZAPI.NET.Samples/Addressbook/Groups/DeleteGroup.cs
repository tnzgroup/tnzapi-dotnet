﻿using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Groups
{
    public class DeleteGroup
    {
        private readonly ITNZAuth apiUser;

        public DeleteGroup(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        #region Run()
        public GroupApiResult Run(GroupModel group)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Delete(group);

            if (response.Result == Enums.ResultCode.Success)
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

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var response = client.Addressbook.Group.Delete(groupID);

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

        public GroupApiResult Simple() => Basic();

        public GroupApiResult Builder()
        {
            var client = new TNZApiClient(apiUser);

            var group = new GroupBuilder(new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD"))
                            .Build();

            var response = client.Addressbook.Group.Delete(group);

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

        public GroupApiResult Advanced(GroupModel? group = null)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Delete(
                new GroupModel()
                {
                    GroupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
                }
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
    }
}
