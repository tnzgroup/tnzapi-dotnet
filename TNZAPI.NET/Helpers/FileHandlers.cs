namespace TNZAPI.NET.Helpers
{
    public class FileHandlers
    {
        /// <summary>
        /// Gets file name from full path
        /// </summary>
        /// <param name="file_location">File location of your recorded message (wave format)</param>
        /// <returns>string</returns>
        public static string GetFileName(string file_location)
        {
            return Path.GetFileName(file_location);
        }

        /// <summary>
        /// Parses your recorded file get ready for transmission
        /// </summary>
        /// <param name="file_location">File location of your recorded message (wave format)</param>
        /// <returns>string</returns>
        public static string GetFileContents(string file_location)
        {
            //
            // Read files in bytes, convert to Base64String and return in byte[]
            //
            if (File.Exists(file_location))
            {
                using (FileStream reader = new FileStream(file_location, FileMode.Open))
                {
                    byte[] buffer = new byte[reader.Length];

#if NET7_0_OR_GREATER
										reader.ReadExactly(buffer);
#else
                    reader.Read(buffer, 0, (int)reader.Length);
#endif



										string strContent = Convert.ToBase64String(buffer);

                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    return System.Text.Encoding.ASCII.GetString(encoding.GetBytes(strContent));
                }
            }

            return "";
        }

        /// <summary>
        /// Parses your recorded file get ready for transmission
        /// </summary>
        /// <param name="file_location">File location of your recorded message (wave format)</param>
        /// <returns>byte[]</returns>
        public static byte[] GetFileContentsGetBytes(string file_location)
        {
            //
            // Read files in bytes, convert to Base64String and return in byte[]
            //
            using (FileStream reader = new FileStream(file_location, FileMode.Open))
            {
                byte[] buffer = new byte[reader.Length];
#if NET7_0_OR_GREATER
										reader.ReadExactly(buffer);
#else
								reader.Read(buffer, 0, (int)reader.Length);
#endif
								string strContent = Convert.ToBase64String(buffer);

                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                return encoding.GetBytes(strContent);
            }
        }

        /// <summary>
        /// Parses your recorded file get ready for transmission
        /// </summary>
        /// <param name="file_location">File location of your recorded message (wave format)</param>
        /// <returns>string</returns>
        public static async Task<string> GetFileContentsAsync(string file_location)
        {
            //
            // Read files in bytes, convert to Base64String and return in byte[]
            //
            if (File.Exists(file_location))
            {
                using (FileStream reader = new FileStream(file_location, FileMode.Open))
                {
                    byte[] buffer = new byte[reader.Length];

#if NET7_0_OR_GREATER
										await reader.ReadExactlyAsync(buffer);
#else
										await reader.ReadAsync(buffer, 0, (int)reader.Length);
#endif

										string strContent = Convert.ToBase64String(buffer);

                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    return System.Text.Encoding.ASCII.GetString(encoding.GetBytes(strContent));
                }
            }

            return "";
        }

        /// <summary>
        /// Parses your recorded file get ready for transmission
        /// </summary>
        /// <param name="file_location">File location of your recorded message (wave format)</param>
        /// <returns>byte[]</returns>
        public static async Task<byte[]> GetFileContentsGetBytesAsync(string file_location)
        {
            //
            // Read files in bytes, convert to Base64String and return in byte[]
            //
            using (FileStream reader = new FileStream(file_location, FileMode.Open))
            {
                byte[] buffer = new byte[reader.Length];

#if NET7_0_OR_GREATER
								await reader.ReadExactlyAsync(buffer);
#else
								await reader.ReadAsync(buffer, 0, (int)reader.Length);
#endif
								string strContent = Convert.ToBase64String(buffer);

                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                return encoding.GetBytes(strContent);
            }
        }
    }
}
