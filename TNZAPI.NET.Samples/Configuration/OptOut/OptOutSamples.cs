using TNZAPI.NET.Api.Configuration.OptOut;
using TNZAPI.NET.Api.Configuration.OptOut.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Configuration.OptOut
{
    /// <summary>
    /// Reference code demonstrating client.Configuration.OptOut.
    /// This class is not a runnable program — call these methods from your own application.
    /// See README.md#configuration--optout for the full reference.
    /// </summary>
    public class OptOutSamples
    {
        private readonly ITNZAuth apiUser;

        public OptOutSamples()
        {
            apiUser = new TNZApiUser();
        }

        public OptOutSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public OptOutApiResult Create()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new OptOutBuilder();

            var entry = builder
                .SetDestType("SMS")
                .SetDestination("+6421003004")
                .SetNotes("Requested via support call")
                .Build();

            var response = client.Configuration.OptOut.Create(entry);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Created OptOut ID={response.ID}");
            }
            else
            {
                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }

        public OptOutBatchApiResult CreateBatch()
        {
            // Response is {"Data": [OptOutDetail, ...]} on the wire - the SDK maps it to .OptOuts.
            var client = new TNZApiClient(apiUser);

            var response = client.Configuration.OptOut.CreateBatch(new OptOutBatchModel
            {
                DestType = "SMS",
                Destinations = new List<string> { "+6421003004", "+6421003005" }
            });

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var entry in response.OptOuts)
                {
                    Console.WriteLine($"Opted out: {entry.Destination} ({entry.ID})");
                }
            }

            return response;
        }

        public OptOutApiResult Details(OptOutID optOutID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Configuration.OptOut.Details(optOutID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"OptOut details for ID={response.ID}");
                Console.WriteLine($"    -> DestType: '{response.DestType}'");
                Console.WriteLine($"    -> Destination: '{response.Destination}'");
                Console.WriteLine($"    -> Notes: '{response.Notes}'");
            }

            return response;
        }

        public OptOutApiResult Update(OptOutID optOutID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Configuration.OptOut.Update(optOutID, new OptOutModel
            {
                DestType = "SMS",
                Destination = "+6421003004",
                Notes = "Updated via API"
            });

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Updated OptOut ID={response.ID}, Notes='{response.Notes}'");
            }

            return response;
        }

        public OptOutApiResult Delete(OptOutID optOutID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Configuration.OptOut.Delete(optOutID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Deleted OptOut ID={response.ID}");
            }

            return response;
        }

        public OptOutListApiResult List()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Configuration.OptOut.List(timePeriod: 30, destType: "SMS");

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"TotalRecords: {response.TotalRecords}, Page: {response.Page}/{response.PageCount}");

                foreach (var entry in response.OptOuts)
                {
                    Console.WriteLine($" -> {entry.Destination} — {entry.DestType}");
                }
            }

            return response;
        }
    }
}