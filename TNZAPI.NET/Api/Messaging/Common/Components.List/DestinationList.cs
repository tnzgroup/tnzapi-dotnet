using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.Common.Components.List
{
    public class DestinationList : IDisposable
    {
        private ICollection<Destination> Destinations { get; set; }

        public DestinationList()
        {
            Destinations = new List<Destination>();
        }

        #region Add() Functions
        public DestinationList Add(string? recipient)
        {
            foreach (var segment in RecipientList.SplitAddresses(recipient))
            {
                Destinations.Add(new Destination(segment));
            }

            return this;
        }

        public DestinationList Add(Destination destination)
        {
            if (destination is null)
            {
                return this;
            }

            Destinations.Add(destination);

            return this;
        }

        // Used by the named-parameter SendMessage(...)/SendMessageAsync(...) overloads' singular
        // `destination` parameter, which is typed object? to accept either a string or a Destination
        // on one parameter (two overloads differing only in that parameter's type are ambiguous for
        // any call that omits it — see SMSApi.SendMessage's comment for the full reasoning).
        // Centralizes the type-dispatch and error message so all 8 modules share one implementation
        // instead of each duplicating this switch.
        public DestinationList Add(object? destination)
        {
            switch (destination)
            {
                case null:
                    break;
                case string destinationString:
                    Add(destinationString);
                    break;
                case Destination destinationObject:
                    Add(destinationObject);
                    break;
                default:
                    throw new ArgumentException($"destination must be a string or Destination, got '{destination.GetType().Name}'.", nameof(destination));
            }

            return this;
        }

        public DestinationList Add(GroupID groupID)
        {
            if (groupID is null || groupID == "")
            {
                return this;
            }

            Destinations.Add(new Destination()
            {
                GroupID = groupID
            });

            return this;
        }

        public DestinationList Add(ContactID contactID)
        {
            if (contactID is null || contactID == "")
            {
                return this;
            }

            Destinations.Add(new Destination()
            {
                ContactID = contactID
            });

            return this;
        }

        // Recipient carries module-specific channel fields (MobileNumber/PhoneNumber/EmailAddress/
        // etc.), so unlike the other Add() overloads this can't infer the wire mapping on its own —
        // callers pass their module's ToDestination(Recipient) helper as map.
        public DestinationList Add(ICollection<Recipient>? recipients, Func<Recipient, Destination> map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (recipients is null || recipients.Count == 0)
            {
                return this;
            }

            foreach (var recipient in recipients)
            {
                if (recipient is null)
                {
                    continue;
                }

                Add(map(recipient));
            }

            return this;
        }

        public DestinationList Add<T>(ICollection<T>? destinations)
        {
            if (destinations is null || destinations.Count == 0)
            {
                return this;
            }

            foreach (var destination in destinations)
            {
                if (destination is null)
                {
                    continue;
                }

                switch (destination)
                {
                    case Destination _:
                        Add(Mapper.Map<Destination>(destination));
                        break;
                    case GroupID _:
                        Add(Mapper.Map<GroupID>(destination));
                        break;
                    case ContactID _:
                        Add(Mapper.Map<ContactID>(destination));
                        break;
                    case string _:
                        Add(destination.ToString());
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported Destination Type '{typeof(T).Name}'.");
                }
            }

            return this;
        }
        #endregion

        public ICollection<Destination> ToList()
        {
            return Destinations.ToList();
        }

        public void Dispose()
        {
            Destinations.Clear();
        }
    }
}