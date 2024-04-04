using TNZAPI.NET.Api.Addressbook.Contact.Dto;
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

		public RecipientList Add<T>(T recipient)
		{
			if (recipient is null || recipient.ToString() == "")
			{
				return this;
			}

			switch (recipient)
			{
				case Recipient _:
					return Add(Mapper.Map<Recipient>(recipient));
				case GroupID _:
					return Add(Mapper.Map<GroupID>(recipient));
				case ContactID _:
					return Add(Mapper.Map<ContactID>(recipient));
				case string _:
					return Add(recipient.ToString());
				default:
					throw new Exception($"Unsupported Recipient Type '{typeof(T).Name}'.");
			}

			//if (typeof(T) == typeof(Recipient))
			//{
			//	return Add(Mapper.Map<Recipient>(recipient));
			//}
			//if (typeof(T) == typeof(GroupID))
			//{
			//	return Add(Mapper.Map<GroupID>(recipient));
			//}
			//if (typeof(T) == typeof(GroupCode))
			//{
			//	return Add(Mapper.Map<GroupCode>(recipient));
			//}
			//if (typeof(T) == typeof(ContactID))
			//{
			//	return Add(Mapper.Map<ContactID>(recipient));
			//}

			//return Add(recipient.ToString());
		}

		public RecipientList Add<T>(ICollection<T> recipients)
		{
			if (recipients is null || recipients.Count() == 0)
			{
				return this;
			}

			foreach (var recipient in recipients)
			{
				Add(recipient);
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
