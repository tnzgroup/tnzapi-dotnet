using System.Text.Json;

namespace TNZAPI.NET.Helpers
{
    public static class DebugUtil
    {
        public static string Dump<T>(this T obj)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(obj, jsonOptions);
        }
    }

}
