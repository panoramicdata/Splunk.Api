using System.Text.RegularExpressions;

namespace Splunk.Api.Handlers;

/// <summary>
/// Refuses, before anything is sent, every request that could change Splunk: any method other than GET or HEAD, except
/// POSTs to the read-only operations in <see cref="ReadOnlyPosts"/>.
/// </summary>
internal sealed partial class ReadOnlyHandler(Uri baseUri) : DelegatingHandler
{
	/// <summary>
	/// POST endpoints that read but do not change Splunk configuration or data: logging in; creating, exporting and
	/// controlling search jobs (a job belongs to the caller and expires); reading a job's events or results with a
	/// post-process search; and parsing searches.
	/// </summary>
	[GeneratedRegex(@"^(auth/login|search/(v2/)?jobs(/export)?|search/(v2/)?jobs/[^/]+/(control|events|results|results_preview)|search/(v2/)?parser)/?$", RegexOptions.CultureInvariant)]
	private static partial Regex ReadOnlyPosts();

	/// <inheritdoc />
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (!IsAllowed(request))
		{
			throw new SplunkReadOnlyException(request.Method.Method, request.RequestUri!.GetLeftPart(UriPartial.Path));
		}

		return base.SendAsync(request, cancellationToken);
	}

	private bool IsAllowed(HttpRequestMessage request)
	{
		if (request.Method == HttpMethod.Get || request.Method == HttpMethod.Head)
		{
			return true;
		}

		if (request.Method != HttpMethod.Post)
		{
			return false;
		}

		var relative = ServicePath.Relative(baseUri, request.RequestUri!);
		var endpoint = relative is null ? null : ServicePath.Endpoint(relative);
		return endpoint is not null && ReadOnlyPosts().IsMatch(endpoint);
	}
}
