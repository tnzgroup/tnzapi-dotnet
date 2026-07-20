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
        /// <exception cref="FileNotFoundException">The file at <paramref name="file_location"/> does not exist.</exception>
        /// <exception cref="DirectoryNotFoundException">A directory in <paramref name="file_location"/> does not exist.</exception>
        public static string GetFileContents(string file_location)
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

                return Convert.ToBase64String(buffer);
            }
        }

        /// <summary>
        /// Parses your recorded file get ready for transmission
        /// </summary>
        /// <param name="file_location">File location of your recorded message (wave format)</param>
        /// <returns>byte[]</returns>
        /// <exception cref="FileNotFoundException">The file at <paramref name="file_location"/> does not exist.</exception>
        /// <exception cref="DirectoryNotFoundException">A directory in <paramref name="file_location"/> does not exist.</exception>
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

                return System.Text.Encoding.ASCII.GetBytes(strContent);
            }
        }

        /// <summary>
        /// Parses your recorded file get ready for transmission
        /// </summary>
        /// <param name="file_location">File location of your recorded message (wave format)</param>
        /// <returns>string</returns>
        /// <exception cref="FileNotFoundException">The file at <paramref name="file_location"/> does not exist.</exception>
        /// <exception cref="DirectoryNotFoundException">A directory in <paramref name="file_location"/> does not exist.</exception>
        public static async Task<string> GetFileContentsAsync(string file_location)
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

                return Convert.ToBase64String(buffer);
            }
        }

        /// <summary>
        /// Parses your recorded file get ready for transmission
        /// </summary>
        /// <param name="file_location">File location of your recorded message (wave format)</param>
        /// <returns>byte[]</returns>
        /// <exception cref="FileNotFoundException">The file at <paramref name="file_location"/> does not exist.</exception>
        /// <exception cref="DirectoryNotFoundException">A directory in <paramref name="file_location"/> does not exist.</exception>
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

                return System.Text.Encoding.ASCII.GetBytes(strContent);
            }
        }
    }
}
