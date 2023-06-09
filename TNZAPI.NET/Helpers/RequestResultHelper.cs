using TNZAPI.NET.Core;

namespace TNZAPI.NET.Helpers
{
    internal class ResultHelper
    {
        internal static T RespondError<T>(string error) where T : new()
        {
            var response = new T();
            typeof(T).GetProperty("Result").SetValue(response, (int)_Result.ResultCode.Failed);
            typeof(T).GetProperty("ErrorMessage").SetValue(response, new List<string> { error });

            return response;
        }
    }
}
