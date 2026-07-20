using System.Text.Json.Serialization;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Configuration.OptOut.Dto
{
    public class OptOutBatchApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();

        [JsonPropertyName("Data")]
        public List<OptOutDetail> OptOuts { get; set; } = new List<OptOutDetail>();
    }
}