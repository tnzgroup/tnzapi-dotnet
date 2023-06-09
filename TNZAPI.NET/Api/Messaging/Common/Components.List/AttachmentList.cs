using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.Common.Components.List
{
    public class AttachmentList : IDisposable
    {
        private ICollection<string> Files { get; set; }
        private ICollection<Attachment> Attachments { get; set; }

        public AttachmentList()
        {
            Files = new List<string>();
            Attachments = new List<Attachment>();
        }

        #region Add
        public AttachmentList Add(string file)
        {
            if (file is null || file == "")
            {
                return this;
            }

            Files.Add(file);

            return this;
        }

        public AttachmentList Add(ICollection<string> files)
        {
            if (files is null || files.Count() == 0)
            {
                return this;
            }

            foreach (var file in files)
            {
                Add(file);
            }

            return this;
        }

        public AttachmentList Add(Attachment attachment)
        {
            if (attachment is null)
            {
                return this;
            }

            Attachments.Add(attachment);

            return this;
        }

        public AttachmentList Add(ICollection<Attachment> attachments)
        {
            if (attachments is null || attachments.Count() == 0)
            {
                return this;
            }

            foreach (var attachment in attachments)
            {
                Add(attachment);
            }

            return this;
        }

        #endregion

        #region ToList
        public ICollection<Attachment> ToList()
        {
            if (Files is null)
            {
                return null;
            }

            foreach (var file in Files)
            {
                using (var attachment = Attachment.GetAttachment(file))
                {
                    // Copy object as properties disposed after using() statement
                    Attachments.Add(Mapper.Map<Attachment>(attachment));
                }
            }
            return Attachments.ToList();
        }
        #endregion

        #region ToListAsync
        public async Task<ICollection<Attachment>> ToListAsync()
        {
            if (Files is null)
            {
                return null;
            }

            foreach (var file in Files)
            {
                using (var attachment = await Attachment.GetAttachmentAsync(file))
                {
                    // Copy object as properties disposed after using() statement
                    Attachments.Add(Mapper.Map<Attachment>(attachment));
                }
            }

            return Attachments.ToList();
        }
        #endregion

        public void Dispose()
        {
            Attachments.Clear();
            Attachments = null;
        }
    }
}
