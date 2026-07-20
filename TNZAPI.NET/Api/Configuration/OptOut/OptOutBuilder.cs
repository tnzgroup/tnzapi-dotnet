using TNZAPI.NET.Api.Configuration.OptOut.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Extensions;

namespace TNZAPI.NET.Api.Configuration.OptOut
{
    public sealed class OptOutBuilder : IDisposable
    {
        private OptOutModel Entity { get; set; }

        public OptOutBuilder()
        {
            Entity = new OptOutModel();
        }

        public void Dispose()
        {
            Entity = new OptOutModel();
        }

        public OptOutBuilder SetDestType(string destType)
        {
            Entity.DestType = destType;

            return this;
        }

        // DestType stays a plain string on the wire, but Enums.OptOutDestType covers the confirmed
        // subset of channels OptOut actually validates (SMS/Email/Voice/Fax — see its doc comment).
        // This overload is offered for convenience/discoverability without giving up the string
        // overload above as an escape hatch for anything the enum doesn't cover. GetDescription()
        // (not ToString()) reads the enum's [Description] attribute, pinning the wire value
        // explicitly so a future rename of a member can't silently change what's sent.
        public OptOutBuilder SetDestType(Enums.OptOutDestType destType)
        {
            Entity.DestType = destType.GetDescription();

            return this;
        }

        public OptOutBuilder SetDestination(string destination)
        {
            Entity.Destination = destination;

            return this;
        }

        public OptOutBuilder SetContactID(ContactID contactID)
        {
            Entity.ContactID = contactID;

            return this;
        }

        public OptOutBuilder SetSubAccount(string subAccount)
        {
            Entity.SubAccount = subAccount;

            return this;
        }

        public OptOutBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        public OptOutBuilder SetStopMessage(string stopMessage)
        {
            Entity.StopMessage = stopMessage;

            return this;
        }

        public OptOutBuilder SetNotes(string notes)
        {
            Entity.Notes = notes;

            return this;
        }

        public OptOutModel Build()
        {
            return Entity;
        }
    }
}