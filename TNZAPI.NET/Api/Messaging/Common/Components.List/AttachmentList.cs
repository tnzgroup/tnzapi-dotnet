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
        public AttachmentList Add(string? file)
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

        public AttachmentList Add(ICollection<Attachment>? attachments)
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
            var result = new List<Attachment>(Attachments);

            foreach (var file in Files)
            {
                using (var attachment = Attachment.GetAttachment(file))
                {
                    if (attachment is null)
                    {
                        throw new FileNotFoundException($"Attachment file not found: {file}", file);
                    }

                    // Copy object as properties disposed after using() statement
                    result.Add(Mapper.Map<Attachment>(attachment));
                }
            }

            return result;
        }
        #endregion

        #region ToListAsync
        public async Task<ICollection<Attachment>> ToListAsync()
        {
            var result = new List<Attachment>(Attachments);

            foreach (var file in Files)
            {
                using (var attachment = await Attachment.GetAttachmentAsync(file))
                {
                    if (attachment is null)
                    {
                        throw new FileNotFoundException($"Attachment file not found: {file}", file);
                    }

                    // Copy object as properties disposed after using() statement
                    result.Add(Mapper.Map<Attachment>(attachment));
                }
            }

            return result;
        }
        #endregion

        public void Dispose()
        {
            Files.Clear();
            Attachments.Clear();
        }
    }
}
