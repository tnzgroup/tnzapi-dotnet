using TNZAPI.NET.Api.Configuration.OptOut.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Extensions;

namespace TNZAPI.NET.Api.Configuration.OptOut
{
    public sealed class OptOutBatchBuilder : IDisposable
    {
        private OptOutBatchModel Entity { get; set; }

        public OptOutBatchBuilder()
        {
            Entity = new OptOutBatchModel();
        }

        public void Dispose()
        {
            Entity = new OptOutBatchModel();
        }

        public OptOutBatchBuilder SetDestType(string destType)
        {
            Entity.DestType = destType;

            return this;
        }

        // See OptOutBuilder.SetDestType(Enums.OptOutDestType) for why GetDescription() is used here
        // instead of ToString().
        public OptOutBatchBuilder SetDestType(Enums.OptOutDestType destType)
        {
            Entity.DestType = destType.GetDescription();

            return this;
        }

        public OptOutBatchBuilder SetDestination(string destination)
        {
            Entity.Destination = destination;

            return this;
        }

        public OptOutBatchBuilder AddDestination(string destination)
        {
            Entity.Destinations ??= new List<string>();
            Entity.Destinations.Add(destination);

            return this;
        }

        public OptOutBatchBuilder AddDestinations(ICollection<string> destinations)
        {
            Entity.Destinations ??= new List<string>();
            foreach (var destination in destinations)
            {
                Entity.Destinations.Add(destination);
            }

            return this;
        }

        public OptOutBatchBuilder SetContactID(ContactID contactID)
        {
            Entity.ContactID = contactID;

            return this;
        }

        public OptOutBatchBuilder AddContactID(ContactID contactID)
        {
            Entity.ContactIDs ??= new List<ContactID>();
            Entity.ContactIDs.Add(contactID);

            return this;
        }

        public OptOutBatchBuilder AddContactIDs(ICollection<ContactID> contactIDs)
        {
            Entity.ContactIDs ??= new List<ContactID>();
            foreach (var contactID in contactIDs)
            {
                Entity.ContactIDs.Add(contactID);
            }

            return this;
        }

        public OptOutBatchBuilder SetSubAccount(string subAccount)
        {
            Entity.SubAccount = subAccount;

            return this;
        }

        public OptOutBatchBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public OptOutBatchModel Build()
        {
            return Entity;
        }
    }
}
