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
            var result = TryReadFile(file_location);
            if (result is null)
            {
                return;
            }

            FileLocation = file_location;
            FileName = result.Value.FileName;
            FileContent = result.Value.FileContent;
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
        public static Attachment? GetAttachment(string? file_location)
        {
            if (string.IsNullOrEmpty(file_location))
            {
                return null;
            }

            var result = TryReadFile(file_location!);
            if (result is null)
            {
                return null;
            }

            return new Attachment
            {
                FileLocation = file_location!,
                FileName = result.Value.FileName,
                FileContent = result.Value.FileContent
            };
        }

        /// <summary>
        /// Read attachment information from the file location async
        /// </summary>
        /// <param name="file_location">File location - Full path</param>
        /// <returns></returns>
        public static async Task<Attachment?> GetAttachmentAsync(string? file_location)
        {
            if (string.IsNullOrEmpty(file_location))
            {
                return null;
            }

            var result = await TryReadFileAsync(file_location!);
            if (result is null)
            {
                return null;
            }

            return new Attachment
            {
                FileLocation = file_location!,
                FileName = result.Value.FileName,
                FileContent = result.Value.FileContent
            };
        }

        // Shared by the constructor, GetAttachment, and GetAttachmentAsync — File.Exists is checked
        // once here, then the actual read is wrapped in the same TOCTOU-safe catch, so a file that
        // vanishes (or whose parent directory vanishes) between the check and the read is treated
        // identically to "file doesn't exist" everywhere, rather than three separately-maintained copies.
        private static (string FileName, string FileContent)? TryReadFile(string file_location)
        {
            if (!File.Exists(file_location))
            {
                return null;
            }

            try
            {
                return (FileHandlers.GetFileName(file_location), FileHandlers.GetFileContents(file_location));
            }
            catch (Exception ex) when (IsFileMissingException(ex))
            {
                return null;
            }
        }

        private static async Task<(string FileName, string FileContent)?> TryReadFileAsync(string file_location)
        {
            if (!File.Exists(file_location))
            {
                return null;
            }

            try
            {
                return (FileHandlers.GetFileName(file_location), await FileHandlers.GetFileContentsAsync(file_location));
            }
            catch (Exception ex) when (IsFileMissingException(ex))
            {
                return null;
            }
        }

        private static bool IsFileMissingException(Exception ex)
        {
            return ex is FileNotFoundException or DirectoryNotFoundException;
        }

        public void Dispose()
        {
            FileName = "";
            FileContent = "";
            FileLocation = "";
        }
    }
}