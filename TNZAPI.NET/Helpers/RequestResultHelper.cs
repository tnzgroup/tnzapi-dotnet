using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Helpers
{
    internal class ResultHelper
    {
        internal static T RespondError<T>(string error) where T : IApiResult, new()
        {
            var response = new T
            {
                Result = Enums.ResultCode.Failed,
                ErrorMessage = new List<string> { error }
            };

            return response;
        }
    }
}
