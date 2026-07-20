using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Demo.Api.Helpers
{
    public static class ApiResultResponseExtensions
    {
        // TNZAPI.NET's own result types are always fully-shaped POCOs (see the SDK's "Result shape
        // convention") — a client-side validation failure like "Empty MessageID" would otherwise echo
        // back misleading noise on top of Result/ErrorMessage, like a non-null default JobStatus
        // ("Pending", C# enums default to their first member) or explicit nulls for fields that were
        // never populated (MessageID, JobNum, ActionResult, ...). This is purely a Demo-API
        // response-shaping convention for readability — the underlying SDK object itself is untouched,
        // and a successful result is still returned in full.
        public static IActionResult RespondWithResult(this ControllerBase controller, IApiResult result)
        {
            if (result.Result == Enums.ResultCode.Success)
            {
                return controller.StatusCode(200, result);
            }

            return controller.StatusCode(400, new { result.Result, result.ErrorMessage });
        }

        // The SDK does no bounds-checking of its own on page/recordsPerPage — it interpolates them
        // straight into the request URL (see e.g. ContactApi.BuildListURL) — so a negative page or an
        // excessively large recordsPerPage would otherwise reach TNZ's real API as-is and surface as
        // an opaque downstream error instead of a clear 400 from this Demo API.
        public static IActionResult? ValidatePagination(this ControllerBase controller, int page, int recordsPerPage, int maxRecordsPerPage = 1000)
        {
            if (page < 1)
            {
                return controller.BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Invalid page: '{page}' must be at least 1." } });
            }

            if (recordsPerPage < 1 || recordsPerPage > maxRecordsPerPage)
            {
                return controller.BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Invalid recordsPerPage: '{recordsPerPage}' must be between 1 and {maxRecordsPerPage}." } });
            }

            return null;
        }
    }
}