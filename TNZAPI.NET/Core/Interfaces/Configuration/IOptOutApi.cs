using TNZAPI.NET.Api.Configuration.OptOut.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces.Configuration
{
    public interface IOptOutApi
    {
        OptOutApiResult Create(OptOutModel entity);
        Task<OptOutApiResult> CreateAsync(OptOutModel entity);

        OptOutBatchApiResult CreateBatch(OptOutBatchModel entity);
        Task<OptOutBatchApiResult> CreateBatchAsync(OptOutBatchModel entity);

        OptOutListApiResult List(int timePeriod = 0, string? destType = null, string? contactID = null, int page = 1, int recordsPerPage = 100);
        Task<OptOutListApiResult> ListAsync(int timePeriod = 0, string? destType = null, string? contactID = null, int page = 1, int recordsPerPage = 100);

        OptOutListApiResult List(Enums.OptOutDestType destType, int timePeriod = 0, string? contactID = null, int page = 1, int recordsPerPage = 100);
        Task<OptOutListApiResult> ListAsync(Enums.OptOutDestType destType, int timePeriod = 0, string? contactID = null, int page = 1, int recordsPerPage = 100);

        OptOutApiResult Details(OptOutID optOutID);
        Task<OptOutApiResult> DetailsAsync(OptOutID optOutID);

        OptOutApiResult Update(OptOutID optOutID, OptOutModel entity);
        Task<OptOutApiResult> UpdateAsync(OptOutID optOutID, OptOutModel entity);

        OptOutApiResult Delete(OptOutID optOutID);
        Task<OptOutApiResult> DeleteAsync(OptOutID optOutID);
    }
}