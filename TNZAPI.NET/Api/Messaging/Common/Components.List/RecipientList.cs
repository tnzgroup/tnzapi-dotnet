using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
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

        #region Add() Functions
        public RecipientList Add(string recipient)
        {
            if (recipient is null || recipient == "")
            {
                return this;
            }

            Recipients.Add(new Recipient(recipient));

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

        public RecipientList Add(GroupModel group)
        {
            if (group is null)
            {
                return this;
            }

            Recipients.Add(Mapper.Map<Recipient>(group));

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

        public RecipientList Add(ContactModel contact)
        {
            if (contact is null)
            {
                return this;
            }

            Recipients.Add(Mapper.Map<Recipient>(contact));

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

        public RecipientList Add<T>(ICollection<T> recipients)
        {
            if (recipients is null || recipients.Count() == 0)
            {
                return this;
            }

            foreach (var recipient in recipients)
            {
                switch (recipient)
                {
                    case Recipient _:
                        Add(Mapper.Map<Recipient>(recipient));
                        break;
                    case GroupModel _:
                        Add(Mapper.Map<GroupModel>(recipient));
                        break;
                    case GroupID _:
                        Add(Mapper.Map<GroupID>(recipient));
                        break;
                    case ContactModel _:
                        Add(Mapper.Map<ContactModel>(recipient));
                        break;
                    case ContactID _:
                        Add(Mapper.Map<ContactID>(recipient));
                        break;
                    case string _:
                        Add(recipient.ToString());
                        break;
                    default:
                        throw new Exception($"Unsupported Recipient Type '{typeof(T).Name}'.");
                }
            }

            return this;
        }
        #endregion

        public ICollection<Recipient> ToList()
        {
            if (Recipients is null)
            {
                return null;
            }

            return Recipients.ToList();
        }

        public void Dispose()
        {
            Recipients.Clear();
            Recipients = null;
        }
    }
}
