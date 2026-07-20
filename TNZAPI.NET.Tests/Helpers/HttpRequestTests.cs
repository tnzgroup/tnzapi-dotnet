using System.Net;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Helpers;
using TNZAPI.NET.Tests.Helpers;

namespace TNZAPI.NET.Tests.Helpers;

public class HttpRequestTests : IDisposable
{
    private class FakeApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new();
        public string? MessageID { get; set; }
    }

    public void Dispose()
    {
        HttpRequest.MessageHandler = new HttpClientHandler();
        Environment.SetEnvironmentVariable("TNZ_ALLOW_INSECURE_HTTP", null);
    }

    [Fact]
    public async Task PostAsync_SendsBearerAuthorizationHeader()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        await HttpRequest.PostAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/sms", user, new { Message = "hi" });

        Assert.NotNull(fakeHandler.LastRequest);
        Assert.Equal("Bearer", fakeHandler.LastRequest!.Headers.Authorization!.Scheme);
        Assert.Equal("test-token-123", fakeHandler.LastRequest.Headers.Authorization!.Parameter);
    }

    [Fact]
    public async Task PostAsync_SerializesBodyAsJson()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        await HttpRequest.PostAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/sms", user, new { Message = "hi" });

        Assert.Equal("{\"Message\":\"hi\"}", fakeHandler.LastRequestBody);
    }

    [Fact]
    public async Task PostAsync_UsesPostVerb()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        await HttpRequest.PostAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/sms", user, new { Message = "hi" });

        Assert.Equal(HttpMethod.Post, fakeHandler.LastRequest!.Method);
    }

    [Fact]
    public async Task GetAsync_UsesGetVerbAndNoBody()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        await HttpRequest.GetAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/sms/abc-123", user);

        Assert.Equal(HttpMethod.Get, fakeHandler.LastRequest!.Method);
        Assert.Null(fakeHandler.LastRequestBody);
    }

    [Fact]
    public async Task PatchAsync_UsesPatchVerb()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        await HttpRequest.PatchAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/sms/abc-123/reschedule", user, new { SendTime = "2026-08-01T10:00:00" });

        Assert.Equal(HttpMethod.Patch, fakeHandler.LastRequest!.Method);
    }

    [Fact]
    public async Task DeleteAsync_UsesDeleteVerb()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        await HttpRequest.DeleteAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/addressbook/contact/abc-123", user);

        Assert.Equal(HttpMethod.Delete, fakeHandler.LastRequest!.Method);
    }

    [Fact]
    public async Task PostAsync_On400Response_ReturnsParsedFailedResult()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.BadRequest, "{\"Result\":\"Failed\",\"ErrorMessage\":[\"Empty message\"]}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        var result = await HttpRequest.PostAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/sms", user, new { });

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Equal("Empty message", result.ErrorMessage[0]);
    }

    [Fact]
    public async Task PostAsync_SetsUserAgentAndAcceptHeaders()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        await HttpRequest.PostAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/sms", user, new { });

        Assert.Equal("TNZAPI.NET-3.00", fakeHandler.LastRequest!.Headers.UserAgent.ToString());
        Assert.Contains(fakeHandler.LastRequest.Headers.Accept, h => h.MediaType == "application/json");
    }

    [Fact]
    public async Task GetAsync_WithNonHttpsUrl_FailsClientSideWithoutNetworkCall()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        var result = await HttpRequest.GetAsync<FakeApiResult>("http://api.tnz.co.nz/api/v3.00/sms/abc-123", user);

        Assert.Equal(Enums.ResultCode.Failed, result.Result);
        Assert.Null(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task GetAsync_WithNonHttpsUrlAndAllowInsecureHttpSet_SendsTheRequest()
    {
        Environment.SetEnvironmentVariable("TNZ_ALLOW_INSECURE_HTTP", "true");
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        var result = await HttpRequest.GetAsync<FakeApiResult>("http://api.tnz.co.nz/api/v3.00/sms/abc-123", user);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task GetAsync_WithNonHttpsUrlAndAllowInsecureHttpSetWithDifferentCasing_SendsTheRequest()
    {
        Environment.SetEnvironmentVariable("TNZ_ALLOW_INSECURE_HTTP", "True");
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        var result = await HttpRequest.GetAsync<FakeApiResult>("http://api.tnz.co.nz/api/v3.00/sms/abc-123", user);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(fakeHandler.LastRequest);
    }

    [Fact]
    public async Task GetAsync_WithHttpsUrl_SendsTheRequest()
    {
        var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, "{\"MessageID\":\"abc-123\"}");
        HttpRequest.MessageHandler = fakeHandler;
        var user = new TNZApiUser { AuthToken = "test-token-123" };

        var result = await HttpRequest.GetAsync<FakeApiResult>("https://api.tnz.co.nz/api/v3.00/sms/abc-123", user);

        Assert.Equal(Enums.ResultCode.Success, result.Result);
        Assert.NotNull(fakeHandler.LastRequest);
    }
}