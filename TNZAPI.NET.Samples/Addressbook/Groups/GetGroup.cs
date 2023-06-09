using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Groups
{
    public class GetGroup
    {
        private readonly ITNZAuth apiUser;

        public string GroupCode { get; set; }

        public GetGroup(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            GroupCode = "";
        }

        public GetGroup(ITNZAuth apiUser, string groupCode)
        {
            this.apiUser = apiUser;

            GroupCode = groupCode;
        }

        public GroupModel? Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.GetByGroupCode(GroupCode);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
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
                Console.WriteLine($"Error={response.ErrorMessage}");
            }

            return null;
        }

        public GroupModel? Simple() => Basic();    // Same as Basic

        public GroupModel? Builder() => Basic();   // Same as Basic

        public GroupModel? Advanced() => Basic();  // Same as Basic
    }
}
