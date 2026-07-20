using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Group
{
    /// <summary>
    /// Reference code demonstrating client.Addressbook.Group.Contact — the Group-to-Contact
    /// many-to-many relation facade. This class is not a runnable program — call these methods
    /// from your own application. See README.md#addressbook for the full reference.
    /// </summary>
    public class GroupContactSamples
    {
        private readonly ITNZAuth apiUser;

        public GroupContactSamples()
        {
            apiUser = new TNZApiUser();
        }

        public GroupContactSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        // Contacts belonging to a group
        public GroupContactListApiResult List(GroupID groupID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Contact.List(groupID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contacts in group '{response.Group?.GroupName}':");

                foreach (var contact in response.Contacts)
                {
                    Console.WriteLine($" -> {contact.ContactID}: {contact.FirstName} {contact.LastName}");
                }
            }

            return response;
        }

        public ContactGroupRelationApiResult Add(GroupID groupID, ContactID contactID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Contact.Add(groupID, contactID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Added {response.Contact?.FirstName} {response.Contact?.LastName} to group '{response.Group?.GroupName}'");
            }

            return response;
        }

        public ContactGroupRelationApiResult Remove(GroupID groupID, ContactID contactID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Contact.Remove(groupID, contactID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Removed {response.Contact?.FirstName} {response.Contact?.LastName} from group '{response.Group?.GroupName}'");
            }

            return response;
        }

        public ContactGroupRelationApiResult Detail(GroupID groupID, ContactID contactID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Group.Contact.Detail(groupID, contactID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"{response.Contact?.FirstName} {response.Contact?.LastName} is in group '{response.Group?.GroupName}'");
            }

            return response;
        }
    }
}
