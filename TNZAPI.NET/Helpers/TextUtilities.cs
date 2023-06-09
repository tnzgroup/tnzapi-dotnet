namespace TNZAPI.NET.Helpers
{
    //
    // http://www.digitalcoding.com/Code-Snippets/C-Sharp/C-Code-Snippet-AddSlashes-StripSlashes-Escape-String.html
    //
    internal class TextUtilities
    {
        /// <summary>
        /// Returns a string with backslashes before characters that need to be quoted
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static string AddSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }

        /// <summary>
        /// Un-quotes a quoted string
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static string StripSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }

        /// <summary>
        /// Returns a string with backslashes before characters that need to be quoted
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static async Task<string> AddSlashesAsync(string InputTxt)
        {
            return await Task.Run(() =>
            {

                return AddSlashes(InputTxt);

                /*
                // List of characters handled:
                // \000 null
                // \010 backspace
                // \011 horizontal tab
                // \012 new line
                // \015 carriage return
                // \032 substitute
                // \042 double quote
                // \047 single quote
                // \134 backslash
                // \140 grave accent

                string Result = InputTxt;

                try
                {
                    Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
                }
                catch (Exception Ex)
                {
                    // handle any exception here
                    Console.WriteLine(Ex.Message);
                }

                return Result;
                */
            });
        }
    }
}
