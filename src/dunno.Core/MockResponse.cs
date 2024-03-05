namespace dunno.Core;

public delegate Task MockResponse(HttpResponseMessage mockResponse, CancellationToken cancellationToken = default);