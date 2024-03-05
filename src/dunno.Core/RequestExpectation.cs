namespace dunno.Core;

public delegate Task<bool> RequestExpectation(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken = default);