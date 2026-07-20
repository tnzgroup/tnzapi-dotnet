using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Group
{
    /// <summary>
    /// Reference code demonstrating client.Addressbook.Group CRUD operations.
    /// This class is not a runnable program — call these methods from your own application.
    /// See README.md#addressbook for the full reference.
    /// </summary>
    public class GroupSamples
    {
        private readonly ITNZAuth apiUser;

        public GroupSamples()
        {
            apiUser = new TNZApiUser();
        }

        public GroupSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public GroupApiResult Create()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new GroupBuilder();

            var group = builder
                .SetGroupName("API Test Group")
                .Build();

            var response = client.Addressbook.Group.Create(group);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Created GroupID={response.GroupID}, GroupCode={response.GroupCode}");
            }
            else
            {
                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }

        public GroupApiResult Details(GroupID groupID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Details(groupID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Group details for GroupID={response.GroupID}");
                Console.WriteLine($"    -> GroupCode: '{response.GroupCode}'");
                Console.WriteLine($"    -> GroupName: '{response.GroupName}'");
                Console.WriteLine($"    -> SubAccount: '{response.SubAccount}'");
                Console.WriteLine($"    -> Department: '{response.Department}'");
                Console.WriteLine($"    -> ViewEditBy: '{response.ViewEditBy}'");
                Console.WriteLine($"    -> Owner: '{response.Owner}'");
            }

            return response;
        }

        public GroupApiResult Update(GroupID groupID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Update(groupID, new GroupModel
            {
                GroupName = "Renamed Group"
            });

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Updated GroupID={response.GroupID}, GroupName='{response.GroupName}'");
            }

            return response;
        }

        public GroupApiResult Delete(GroupID groupID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Delete(groupID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Deleted GroupID={response.GroupID}");
            }

            return response;
        }

        public GroupListApiResult List()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.List(page: 1, recordsPerPage: 100);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"TotalRecords: {response.TotalRecords}, Page: {response.Page}/{response.PageCount}");

                foreach (var group in response.Groups)
                {
                    Console.WriteLine($" -> {group.GroupID}: {group.GroupName}");
                }
            }

            return response;
        }
    }
}