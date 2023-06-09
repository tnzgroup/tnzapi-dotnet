using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Messaging.Common.Components
{
    public class Attachment : IDisposable
    {
        public string FileName { get; set; } = "";
        public string FileLocation { get; set; } = "";
        public string FileContent { get; set; } = "";


        /// <summary>
        /// Initiates Attachment
        /// </summary>
        /// <returns></returns>
        public Attachment()
        {
        }

        /// <summary>
        /// Initiates Recipient
        /// </summary>
        /// <param name="file_location">Location of attachment</param>
        /// <returns></returns>
        public Attachment(string file_location)
        {
            if (File.Exists(file_location))
            {
                FileLocation = file_location;

                FileName = FileHandlers.GetFileName(file_location);
                FileContent = FileHandlers.GetFileContents(file_location);
            }
        }

        /// <summary>
        /// Initiates Recipient
        /// </summary>
        /// <param name="file_name">Name of attachment</param>
        /// <param name="file_content">Content of attachment</param>
        /// <returns></returns>
        public Attachment(string file_name, string file_content)
        {
            FileName = file_name;
            FileContent = file_content;
        }

        /// <summary>
        /// Read attachment information from the file location
        /// </summary>
        /// <param name="file_location">File location - Full path</param>
        /// <returns></returns>
        public static Attachment GetAttachment(string file_location)
        {
            if (string.IsNullOrEmpty(file_location))
            {
                return null;
            }

            if (File.Exists(file_location))
            {
                return new Attachment()
                {
                    FileLocation = file_location,
                    FileName = FileHandlers.GetFileName(file_location),
                    FileContent = FileHandlers.GetFileContents(file_location)
                };
            }

            return null;
        }

        /// <summary>
        /// Read attachment information from the file location async
        /// </summary>
        /// <param name="file_location">File location - Full path</param>
        /// <returns></returns>
        public static async Task<Attachment> GetAttachmentAsync(string file_location)
        {
            if (string.IsNullOrEmpty(file_location))
            {
                return null;
            }

            if (File.Exists(file_location))
            {
                return new Attachment()
                {
                    FileLocation = file_location,
                    FileName = FileHandlers.GetFileName(file_location),
                    FileContent = await FileHandlers.GetFileContentsAsync(file_location)
                };
            }

            return null;
        }

        public void Dispose()
        {
            FileName = null;
            FileContent = null;
            FileLocation = null;
        }
    }
}
