using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Groups
{
    public class CreateGroup
    {
        private readonly ITNZAuth apiUser;

        public GroupModel Group;

        public CreateGroup(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            Group = new GroupModel();
        }

        public CreateGroup(ITNZAuth apiUser, GroupModel group)
        {
            this.apiUser = apiUser;

            Group = group;
        }

        public GroupModel? Basic()
        {
            var client = new TNZApiClient(apiUser);

            if (Group is null)
            {
                Group = new GroupModel()
                {
                    GroupName = "API Test Group"
                };
            }

            var response = client.Addressbook.Group.Create(Group);

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

                return response.Group;
            }
            else
            {
                Console.WriteLine("Error occurred while processing...");
                Console.WriteLine($"Error: {response.ErrorMessage}");
            }

            return null;
        }

        public GroupModel? Simple() => Basic();    // Same as Basic

        public GroupModel? Builder() => Basic();   // Same as Basic

        public GroupModel? Advanced() => Basic();  // Same as Basic
    }
}
