namespace dunno.Core;

public class RequestResponseConfiguration
{
    public RequestExpectation RequestExpectation { get; }
    public MockResponse MockResponse { get; }
    public IReadOnlyCollection<HttpRequestMessage> InvokedRequests { get; } = new List<HttpRequestMessage>();

    public RequestResponseConfiguration(RequestExpectation requestExpectation, MockResponse mockResponse)
    {
        RequestExpectation = requestExpectation ?? throw new ArgumentNullException(nameof(requestExpectation));
        MockResponse = mockResponse ?? throw new ArgumentNullException(nameof(mockResponse));
    }

    internal void RegisterInvocation(HttpRequestMessage request)
    {
        ((IList<HttpRequestMessage>)InvokedRequests).Add(request);
    }
}