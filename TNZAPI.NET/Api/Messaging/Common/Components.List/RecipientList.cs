using TNZAPI.NET.Api.Messaging.Common.Components;

namespace TNZAPI.NET.Api.Messaging.Common.Components.List
{
    public class RecipientList : IDisposable
    {
        private ICollection<Recipient> Recipients { get; set; }

        public RecipientList()
        {
            Recipients = new List<Recipient>();
        }

        public RecipientList Add(string recipient)
        {
            if (recipient is null || recipient == "")
            {
                return this;
            }

            Recipients.Add(new Recipient(recipient));

            return this;
        }

        public RecipientList Add(ICollection<string> recipients)
        {
            if (recipients is null || recipients.Count() == 0)
            {
                return this;
            }

            foreach (var recipient in recipients)
            {
                Recipients.Add(new Recipient(recipient));
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

        public RecipientList Add(ICollection<Recipient> recipients)
        {
            if (recipients is null || recipients.Count() == 0)
            {
                return this;
            }

            foreach (var recipient in recipients)
            {
                Recipients.Add(recipient);
            }

            return this;
        }

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
