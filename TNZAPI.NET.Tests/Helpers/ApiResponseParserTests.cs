using System.Net;
using System.Text;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Tests.Helpers;

public class ApiResponseParserTests
{
    private class FakeApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new();
        public string? MessageID { get; set; }
        public DateTime? CreatedTimeLocal { get; set; }
    }

    private static HttpResponseMessage BuildResponse(HttpStatusCode statusCode, string? body)
    {
        var response = new HttpResponseMessage(statusCode);

        if (body is not null)
        {
            response.Content = new StringContent(body, Encoding.UTF8, "application/json");
        }

        return response;
    }

    [Fact]
    public async Task ParseAsync_On200_DeserializesBodyAndSetsSuccess()
    {
        var response = BuildResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal("abc-123", result.MessageID);
        Assert.Empty(result.ErrorMessage);
    }

    [Fact]
    public async Task ParseAsync_On400_WithArrayErrorMessage_ParsesFailed()
    {
        var response = BuildResponse(HttpStatusCode.BadRequest, "{\"Result\":\"Failed\",\"ErrorMessage\":[\"Empty sender\"]}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Single(result.ErrorMessage);
        Assert.Equal("Empty sender", result.ErrorMessage[0]);
    }

    [Fact]
    public async Task ParseAsync_On400_WithStringErrorMessage_ParsesFailed()
    {
        var response = BuildResponse(HttpStatusCode.BadRequest, "{\"Result\":\"Failed\",\"ErrorMessage\":\"Empty sender\"}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Single(result.ErrorMessage);
        Assert.Equal("Empty sender", result.ErrorMessage[0]);
    }

    [Fact]
    public async Task ParseAsync_On401_ParsesUnauthorized()
    {
        var response = BuildResponse(HttpStatusCode.Unauthorized, "{\"Result\":\"Unauthorized\",\"ErrorMessage\":[\"Invalid token\"]}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Unauthorized, result.Result);
        Assert.Equal("Invalid token", result.ErrorMessage[0]);
    }

    [Fact]
    public async Task ParseAsync_On404_WithMessageIDVariant_ParsesRecordNotFound()
    {
        var response = BuildResponse(HttpStatusCode.NotFound, "{\"Result\":\"RecordNotFound\",\"MessageID\":\"abc-123\",\"ErrorMessage\":[\"Not found\"]}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.RecordNotFound, result.Result);
        Assert.Equal("Not found", result.ErrorMessage[0]);
    }

    [Fact]
    public async Task ParseAsync_On404_WithNoResultField_StillParsesRecordNotFoundFromStatusCode()
    {
        var response = BuildResponse(HttpStatusCode.NotFound, "{\"ErrorMessage\":[\"Not found\"]}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.RecordNotFound, result.Result);
        Assert.Equal("Not found", result.ErrorMessage[0]);
    }

    [Fact]
    public async Task ParseAsync_On415_WithNoResultOrErrorMessageFields_StillReturnsFailedWithEmptyMessages()
    {
        var response = BuildResponse(HttpStatusCode.UnsupportedMediaType, "{\"status\":415,\"extend1\":\"\",\"extend2\":\"\"}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Empty(result.ErrorMessage);
    }

    [Fact]
    public async Task ParseAsync_On500_WithEmptyBody_ReturnsFailedWithEmptyMessages()
    {
        var response = BuildResponse(HttpStatusCode.InternalServerError, null);

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Empty(result.ErrorMessage);
    }

    [Fact]
    public async Task ParseAsync_On200_WithTNZNonISOTimestamp_ParsesFieldInsteadOfSilentlyBlankingWholeObject()
    {
        var response = BuildResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"CreatedTimeLocal\":\"2026-07-09 10:34:30\"}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal("abc-123", result.MessageID);
        Assert.Equal(new DateTime(2026, 7, 9, 10, 34, 30), result.CreatedTimeLocal);
    }

    [Fact]
    public async Task ParseAsync_On200_WithRFC3339Timestamp_StillParsesCorrectly()
    {
        var response = BuildResponse(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\",\"CreatedTimeLocal\":\"2026-07-08T22:34:30.472Z\"}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.Equal("abc-123", result.MessageID);
        Assert.NotNull(result.CreatedTimeLocal);
    }

    [Fact]
    public async Task ParseAsync_On200_WithMalformedJson_ReturnsFailedInsteadOfBlankSuccess()
    {
        var response = BuildResponse(HttpStatusCode.OK, "{\"MessageID\": this is not valid json");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.NotEmpty(result.ErrorMessage);
        Assert.Null(result.MessageID);
    }

    [Fact]
    public async Task ParseAsync_On200_WithEmptyBody_ReturnsFailedInsteadOfBlankSuccess()
    {
        var response = BuildResponse(HttpStatusCode.OK, null);

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.NotEmpty(result.ErrorMessage);
    }

    [Fact]
    public async Task ParseAsync_On403_ParsesFailed()
    {
        var response = BuildResponse(HttpStatusCode.Forbidden, "{\"Result\":\"Failed\",\"ErrorMessage\":[\"Address Book API access not enabled\"]}");

        var result = await ApiResponseParser.ParseAsync<FakeApiResult>(response);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Equal("Address Book API access not enabled", result.ErrorMessage[0]);
    }
}