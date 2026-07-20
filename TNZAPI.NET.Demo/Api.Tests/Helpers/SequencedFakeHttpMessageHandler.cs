using System.Net;

namespace TNZAPI.NET.Demo.Api.Tests.Helpers;

// Like FakeHttpMessageHandler, but returns a different canned response for each successive outbound
// call — needed for tests where a single Demo API request triggers more than one SDK call (e.g.
// AddressbookController.Update fetching Details before it PATCHes, so the caller can preserve fields
// omitted from the request). Responses beyond the last one provided are repeated.
public class SequencedFakeHttpMessageHandler : HttpMessageHandler
{
    private readonly (HttpStatusCode StatusCode, string ResponseBody)[] _responses;
    private int _callCount;

    public List<HttpRequestMessage> Requests { get; } = new();
    public List<string?> RequestBodies { get; } = new();

    public SequencedFakeHttpMessageHandler(params (HttpStatusCode StatusCode, string ResponseBody)[] responses)
    {
        _responses = responses;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);
        RequestBodies.Add(request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken));

        var (statusCode, responseBody) = _responses[Math.Min(_callCount, _responses.Length - 1)];
        _callCount++;

        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseBody)
        };
    }
}