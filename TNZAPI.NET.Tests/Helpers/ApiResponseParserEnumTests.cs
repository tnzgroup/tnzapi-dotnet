using System.Net;
using System.Text;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Tests.Helpers;

public class ApiResponseParserEnumTests
{
    private class FakeApiResultWithEnum : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new();
        public Enums.JobStatus JobStatus { get; set; }
    }

    [Fact]
    public async Task ParseAsync_On200_DeserializesStringEnumValue()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"JobStatus\":\"Pending\"}", Encoding.UTF8, "application/json")
        };

        var result = await ApiResponseParser.ParseAsync<FakeApiResultWithEnum>(response);

        Assert.Equal(Enums.JobStatus.Pending, result.JobStatus);
    }
}