using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.Common.Components.List
{
    public class RecipientList : IDisposable
    {
        private ICollection<Recipient> Recipients { get; set; }

        public RecipientList()
        {
            Recipients = new List<Recipient>();
        }

        // Shared by every messaging module's SendMessage(...)/SendMessageAsync(...) named-parameter
        // overloads for toNumber/contactID/groupID/mainPhone/emailAddress — keep this the single
        // implementation of comma-separated recipient splitting rather than re-inlining it per module.
        internal static IEnumerable<string> SplitAddresses(string? value)
        {
            if (value is null)
            {
                return Enumerable.Empty<string>();
            }

            return value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(segment => segment.Trim())
                .Where(segment => segment.Length > 0);
        }

        #region Add() Functions
        public RecipientList Add(string? recipient)
        {
            foreach (var segment in SplitAddresses(recipient))
            {
                Recipients.Add(new Recipient(segment));
            }

            return this;
        }

        public RecipientList Add(Recipient recipient)
        {
            if (recipient is null)
            {
                return this;
            }

            Recipients.Add(recipient);

            return this;
        }

        public RecipientList Add(GroupID groupID)
        {
            if (groupID is null || groupID == "")
            {
                return this;
            }

            Recipients.Add(new Recipient()
            {
                GroupID = groupID
            });

            return this;
        }

        public RecipientList Add(ContactID contactID)
        {
            if (contactID is null || contactID == "")
            {
                return this;
            }

            Recipients.Add(new Recipient()
            {
                ContactID = contactID
            });

            return this;
        }

        public RecipientList Add<T>(ICollection<T>? recipients)
        {
            if (recipients is null || recipients.Count() == 0)
            {
                return this;
            }

            foreach (var recipient in recipients)
            {
                if (recipient is null)
                {
                    continue;
                }

                switch (recipient)
                {
                    case Recipient _:
                        Add(Mapper.Map<Recipient>(recipient));
                        break;
                    case GroupID _:
                        Add(Mapper.Map<GroupID>(recipient));
                        break;
                    case ContactID _:
                        Add(Mapper.Map<ContactID>(recipient));
                        break;
                    case string _:
                        Add(recipient.ToString());
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported Recipient Type '{typeof(T).Name}'.");
                }
            }

            return this;
        }
        #endregion

        public ICollection<Recipient> ToList()
        {
            return Recipients.ToList();
        }

        public void Dispose()
        {
            Recipients.Clear();
        }
    }
}