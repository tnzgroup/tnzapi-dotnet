using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Groups
{
    public class UpdateGroup
    {
        private readonly ITNZAuth apiUser;

        public GroupModel GroupModel { get; set; }

        public UpdateGroup(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            GroupModel = new GroupModel();
        }

        public UpdateGroup(ITNZAuth apiUser, GroupModel group)
        {
            this.apiUser = apiUser;

            GroupModel = group;
        }

        public void Basic(string? groupCode = null)
        {
            var client = new TNZApiClient(apiUser);

            if (groupCode is null)
            {
                groupCode = "Test-Group";
            }

            var response = client.Addressbook.Group.Update(
                groupCode: groupCode,
                groupName: "Test Group"
            );

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
        }

        public void Simple(string? groupCode = null)
        {
            var client = new TNZApiClient(apiUser);

            if (groupCode is null)
            {
                groupCode = "Test-Group";
            }

            var response = client.Addressbook.Group.Update(
                groupCode: groupCode,
                groupName: "Test Group",
                subAccount: "Test SubAccount",
                department: "Test Department",
                viewEditBy: Enums.ViewEditByOptions.Account
            );

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
        }

        public void Builder(GroupModel? group = null)
        {
            var client = new TNZApiClient(apiUser);

            if (group is null)
            {
                group = new GroupBuilder("Test-Group")
                            .SetGroupName("Test Group")
                            .SetSubAccount("Test SubAccount")
                            .SetDepartment("Test Department")
                            .SetViewEditBy(Enums.ViewEditByOptions.Account)
                            .Build();
            }

            var response = client.Addressbook.Group.Update(group);

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
        }

        public void Advanded(GroupModel? group = null)
        {
            var client = new TNZApiClient(apiUser);

            if (group is null)
            {
                group = new GroupModel()
                {
                    GroupCode = "Test-Group",
                    GroupName = "Test Group",
                    SubAccount = "Test SubAccount",
                    Department = "Test Department",
                    ViewEditBy = Enums.ViewEditByOptions.Account
                };
            }

            var response = client.Addressbook.Group.Update(group);

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
        }
    }
}
