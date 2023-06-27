using System.Diagnostics;
using System.Text.Json;

namespace TNZAPI.NET.Helpers
{
    public static class DebugUtil
    {
        public static string Dump<T>(this T obj, bool display=true)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var jsonString = JsonSerializer.Serialize(obj, jsonOptions);

            if (display)
            {
                Debug.WriteLine(jsonString);
            }

            return jsonString;
        }
    }

}
