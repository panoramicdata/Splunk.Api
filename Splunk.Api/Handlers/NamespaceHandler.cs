namespace Splunk.Api.Handlers;

/// <summary>
/// Sends requests for the global <c>services/...</c> context to <c>servicesNS/{owner}/{app}/...</c> instead. Requests that
/// already name a namespace, or are not below the base address, pass through unchanged.
/// </summary>
internal sealed class NamespaceHandler(Uri baseUri, SplunkNamespace splunkNamespace) : DelegatingHandler
{
	private readonly string _prefix = splunkNamespace.PathPrefix;

	/// <inheritdoc />
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var requestUri = request.RequestUri!;
		if (ServicePath.Relative(baseUri, requestUri) is { } relative && ServicePath.IsGlobalServices(relative))
		{
			// Built as a string: UriBuilder's setters would escape the % of already-escaped segments a second time.
			var path = requestUri.AbsolutePath[..^relative.Length] + ServicePath.ToNamespace(relative, _prefix);
			request.RequestUri = new Uri(requestUri.GetLeftPart(UriPartial.Authority) + path + requestUri.Query);
		}

		return base.SendAsync(request, cancellationToken);
	}
}
