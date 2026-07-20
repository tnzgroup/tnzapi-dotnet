using System.Net;

namespace TNZAPI.NET.Demo.Api.Tests.Helpers;

// Mirrors TNZAPI.NET.Tests/Helpers/FakeHttpMessageHandler.cs — intercepts the SDK's outbound call to
// TNZ's real API (swapped in via TNZAPI.NET.Helpers.HttpRequest.MessageHandler, an internal static
// field the SDK exposes as a test seam) so these integration tests exercise the full Demo API
// request path (HTTP request -> controller -> TNZApiClient -> SDK -> this fake transport -> back)
// without ever making a real network call.
public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly string _responseBody;

    public HttpRequestMessage? LastRequest { get; private set; }
    public string? LastRequestBody { get; private set; }

    public FakeHttpMessageHandler(HttpStatusCode statusCode, string responseBody)
    {
        _statusCode = statusCode;
        _responseBody = responseBody;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        LastRequestBody = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);

        return new HttpResponseMessage(_statusCode)
        {
            Content = new StringContent(_responseBody)
        };
    }
}