namespace Splunk.Api.Handlers;

/// <summary>
/// Adds <c>output_mode=json</c> to every request that does not already choose an output mode, so Splunk answers in JSON
/// rather than its default Atom XML. An endpoint that needs another format (CSV or raw results) sets it explicitly.
/// </summary>
internal sealed class OutputModeHandler : DelegatingHandler
{
	internal const string Parameter = "output_mode";

	/// <inheritdoc />
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		request.RequestUri = WithJsonOutputMode(request.RequestUri!);
		return base.SendAsync(request, cancellationToken);
	}

	internal static Uri WithJsonOutputMode(Uri uri)
	{
		var query = uri.Query.TrimStart('?');
		if (query.Split('&').Any(p => p.StartsWith(Parameter + "=", StringComparison.Ordinal)))
		{
			return uri;
		}

		// Built as a string: UriBuilder's setters would escape the % of already-escaped values a second time.
		var separator = query.Length == 0 ? "?" : $"?{query}&";
		return new Uri(uri.GetLeftPart(UriPartial.Path) + separator + Parameter + "=json");
	}
}
