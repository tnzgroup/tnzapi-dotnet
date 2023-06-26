using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Groups
{
    public class DeleteGroup
    {
        private readonly ITNZAuth apiUser;

        public GroupModel GroupModel { get; set; }

        public DeleteGroup(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            GroupModel = new GroupModel();
        }

        public DeleteGroup(ITNZAuth apiUser, GroupModel group)
        {
            this.apiUser = apiUser;

            GroupModel = group;
        }

        public void Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Delete(GroupModel);

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
