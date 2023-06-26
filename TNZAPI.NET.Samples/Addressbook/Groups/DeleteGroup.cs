using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
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

        public GroupApiResult Basic(string? groupCode = null)
        {
            var client = new TNZApiClient(apiUser);

            if (groupCode is null)
            {
                groupCode = "Test-Group";
            }

            var response = client.Addressbook.Group.DeleteByGroupCode(groupCode);

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

        public GroupApiResult Simple(string? groupCode = null)
        {
            var client = new TNZApiClient(apiUser);

            if (groupCode is null)
            {
                groupCode = "Test-Group";
            }

            var response = client.Addressbook.Group.DeleteByGroupCode(groupCode);

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

        public GroupApiResult Builder(GroupModel? group = null)
        {
            var client = new TNZApiClient(apiUser);

            if (group is null)
            {
                group = new GroupBuilder("Test-Group").Build();
            }

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

        public GroupApiResult Advanced(GroupModel? group = null)
        {
            var client = new TNZApiClient(apiUser);

            if (group is null)
            {
                group = new GroupModel()
                {
                    GroupCode = "Test-Group"
                };
            }

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
    }
}
