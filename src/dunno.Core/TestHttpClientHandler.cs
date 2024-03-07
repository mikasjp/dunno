using dunno.Core.Exceptions;

namespace dunno.Core;

public sealed class TestHttpClientHandler : HttpClientHandler
{
    public TestHttpClientBehavior TestHttpClientBehavior { get; }
    private IEnumerable<RequestResponseConfiguration> Configurations { get; }

    public TestHttpClientHandler(
        TestHttpClientBehavior testHttpClientBehavior,
        IEnumerable<RequestResponseConfiguration> configurations)
    {
        TestHttpClientBehavior = testHttpClientBehavior;
        Configurations = configurations;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var candidate = await Configurations
            .ToAsyncEnumerable()
            .FirstOrDefaultAwaitAsync(async x => await x.RequestExpectation(request, cancellationToken),
                cancellationToken: cancellationToken);
        
        if (candidate is null)
        {
            return TestHttpClientBehavior switch
            {
                TestHttpClientBehavior.Loose => await base.SendAsync(request, cancellationToken),
                TestHttpClientBehavior.Strict => throw new CandidateNotFoundException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        candidate.RegisterInvocation(request);

        var mockResponse = new HttpResponseMessage();
        await candidate.MockResponse(mockResponse, cancellationToken);

        return mockResponse;
    }
}