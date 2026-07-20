using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Configuration.OptOut.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Configuration;
using TNZAPI.NET.Extensions;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Configuration.OptOut
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class OptOutApi : IOptOutApi
    {
        private ITNZAuth User = new TNZApiUser();

        public OptOutApi()
        {
        }

        public OptOutApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        public OptOutApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        public OptOutApi(ITNZAuth auth)
        {
            User = auth;
        }

        #region Set API User
        public void SetAPIUser(ITNZAuth apiUser)
        {
            User = apiUser;
        }

        public void SetAuthToken(string authToken)
        {
            User.AuthToken = authToken;
        }
        #endregion

        private static string BuildAPIURL()
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/optout";
        }

        private static string BuildBatchURL()
        {
            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/optout/batch";
        }

        #region Create
        public OptOutApiResult Create(OptOutModel entity)
        {
            return Task.Run(() => CreateAsync(entity)).Result;
        }

        [ComVisible(false)]
        public async Task<OptOutApiResult> CreateAsync(OptOutModel entity)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (entity is null || string.IsNullOrEmpty(entity.DestType))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty DestType: Please specify DestType");
            }
            if (string.IsNullOrEmpty(entity.Destination) && (entity.ContactID is null || string.IsNullOrEmpty(entity.ContactID.Value)))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty Destination: Please specify Destination or ContactID");
            }

            return await HttpRequest.PostAsync<OptOutApiResult>(BuildAPIURL(), User, entity);
        }
        #endregion

        #region CreateBatch
        public OptOutBatchApiResult CreateBatch(OptOutBatchModel entity)
        {
            return Task.Run(() => CreateBatchAsync(entity)).Result;
        }

        [ComVisible(false)]
        public async Task<OptOutBatchApiResult> CreateBatchAsync(OptOutBatchModel entity)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<OptOutBatchApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (entity is null || string.IsNullOrEmpty(entity.DestType))
            {
                return ResultHelper.RespondError<OptOutBatchApiResult>("Empty DestType: Please specify DestType");
            }

            var hasDestination = !string.IsNullOrEmpty(entity.Destination)
                || (entity.Destinations is not null && entity.Destinations.Count > 0)
                || (entity.ContactID is not null && !string.IsNullOrEmpty(entity.ContactID.Value))
                || (entity.ContactIDs is not null && entity.ContactIDs.Count > 0);

            if (!hasDestination)
            {
                return ResultHelper.RespondError<OptOutBatchApiResult>("Empty Destination: Please specify Destination, Destinations, ContactID or ContactIDs");
            }

            return await HttpRequest.PostAsync<OptOutBatchApiResult>(BuildBatchURL(), User, entity);
        }
        #endregion

        private static string BuildListURL(int timePeriod, string? destType, string? contactID, int page, int recordsPerPage)
        {
            var query = new List<string> { $"page={page}", $"recordsPerPage={recordsPerPage}" };

            if (timePeriod > 0) query.Add($"timePeriod={timePeriod}");
            if (!string.IsNullOrEmpty(destType)) query.Add($"destType={Uri.EscapeDataString(destType)}");
            if (!string.IsNullOrEmpty(contactID)) query.Add($"contactID={Uri.EscapeDataString(contactID)}");

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/optout/list?{string.Join("&", query)}";
        }

        #region List
        public OptOutListApiResult List(int timePeriod = 0, string? destType = null, string? contactID = null, int page = 1, int recordsPerPage = 100)
        {
            return Task.Run(() => ListAsync(timePeriod, destType, contactID, page, recordsPerPage)).Result;
        }

        // Convenience overload for the common single-channel filter case — see Enums.OptOutDestType's
        // doc comment for why this is a narrower, dedicated enum rather than RecipientChannelType.
        // destType is required here (not optional/defaulted like the string overload above)
        // specifically so a zero-argument List() call stays unambiguous — two overloads that differ
        // only in one all-default parameter's type would otherwise be an ambiguous call the compiler
        // can't resolve.
        public OptOutListApiResult List(Enums.OptOutDestType destType, int timePeriod = 0, string? contactID = null, int page = 1, int recordsPerPage = 100)
        {
            return Task.Run(() => ListAsync(destType, timePeriod, contactID, page, recordsPerPage)).Result;
        }

        [ComVisible(false)]
        public async Task<OptOutListApiResult> ListAsync(int timePeriod = 0, string? destType = null, string? contactID = null, int page = 1, int recordsPerPage = 100)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<OptOutListApiResult>("Empty AuthToken: Please specify AuthToken");
            }

            return await HttpRequest.GetAsync<OptOutListApiResult>(BuildListURL(timePeriod, destType, contactID, page, recordsPerPage), User);
        }

        // GetDescription() (not ToString()) reads the enum's [Description] attribute, pinning the
        // wire value explicitly so a future rename of a member can't silently change what's sent.
        [ComVisible(false)]
        public Task<OptOutListApiResult> ListAsync(Enums.OptOutDestType destType, int timePeriod = 0, string? contactID = null, int page = 1, int recordsPerPage = 100)
        {
            return ListAsync(timePeriod, destType.GetDescription(), contactID, page, recordsPerPage);
        }
        #endregion

        private static string BuildOptOutURL(OptOutID optOutID)
        {
            IDGuard.EnsureProvided(optOutID);

            return $"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/optout/{Uri.EscapeDataString(optOutID.Value!)}";
        }

        #region Details
        public OptOutApiResult Details(OptOutID optOutID)
        {
            return Task.Run(() => DetailsAsync(optOutID)).Result;
        }

        [ComVisible(false)]
        public async Task<OptOutApiResult> DetailsAsync(OptOutID optOutID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (optOutID is null || string.IsNullOrEmpty(optOutID.Value))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty OptOutID: Please specify OptOutID");
            }

            return await HttpRequest.GetAsync<OptOutApiResult>(BuildOptOutURL(optOutID), User);
        }
        #endregion

        #region Update
        public OptOutApiResult Update(OptOutID optOutID, OptOutModel entity)
        {
            return Task.Run(() => UpdateAsync(optOutID, entity)).Result;
        }

        [ComVisible(false)]
        public async Task<OptOutApiResult> UpdateAsync(OptOutID optOutID, OptOutModel entity)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (optOutID is null || string.IsNullOrEmpty(optOutID.Value))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty OptOutID: Please specify OptOutID");
            }

            return await HttpRequest.PatchAsync<OptOutApiResult>(BuildOptOutURL(optOutID), User, entity);
        }
        #endregion

        #region Delete
        public OptOutApiResult Delete(OptOutID optOutID)
        {
            return Task.Run(() => DeleteAsync(optOutID)).Result;
        }

        [ComVisible(false)]
        public async Task<OptOutApiResult> DeleteAsync(OptOutID optOutID)
        {
            if (string.IsNullOrEmpty(User.AuthToken))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty AuthToken: Please specify AuthToken");
            }
            if (optOutID is null || string.IsNullOrEmpty(optOutID.Value))
            {
                return ResultHelper.RespondError<OptOutApiResult>("Empty OptOutID: Please specify OptOutID");
            }

            return await HttpRequest.DeleteAsync<OptOutApiResult>(BuildOptOutURL(optOutID), User);
        }
        #endregion
    }
}