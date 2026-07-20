using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Contact
{
    /// <summary>
    /// Reference code demonstrating client.Addressbook.Contact.Group — the Contact-to-Group
    /// many-to-many relation facade. This class is not a runnable program — call these methods
    /// from your own application. See README.md#addressbook for the full reference.
    /// </summary>
    public class ContactGroupSamples
    {
        private readonly ITNZAuth apiUser;

        public ContactGroupSamples()
        {
            apiUser = new TNZApiUser();
        }

        public ContactGroupSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        // Groups a contact belongs to
        public ContactGroupListApiResult List(ContactID contactID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Group.List(contactID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Groups for ContactID={contactID}:");

                foreach (var group in response.Groups)
                {
                    Console.WriteLine($" -> {group.GroupID}: {group.GroupName}");
                }
            }

            return response;
        }

        public ContactGroupRelationApiResult Add(ContactID contactID, GroupID groupID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Group.Add(contactID, groupID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Added {response.Contact?.FirstName} {response.Contact?.LastName} to group '{response.Group?.GroupName}'");
            }

            return response;
        }

        public ContactGroupRelationApiResult Remove(ContactID contactID, GroupID groupID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Group.Remove(contactID, groupID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Removed {response.Contact?.FirstName} {response.Contact?.LastName} from group '{response.Group?.GroupName}'");
            }

            return response;
        }

        public ContactGroupRelationApiResult Detail(ContactID contactID, GroupID groupID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.Contact.Group.Detail(contactID, groupID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"{response.Contact?.FirstName} {response.Contact?.LastName} is in group '{response.Group?.GroupName}'");
            }

            return response;
        }
    }
}