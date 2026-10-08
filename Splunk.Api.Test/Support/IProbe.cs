using Refit;
using Splunk.Api.Models;

namespace Splunk.Api.Test.Support;

/// <summary>Sends any verb to any path through a client's full pipeline (via <c>SplunkClient.For&lt;IProbe&gt;()</c>).</summary>
public interface IProbe
{
	[Get("{**path}")]
	Task<SplunkFeed<SplunkDynamicContent>> GetAsync(string path, CancellationToken cancellationToken);

	[Post("{**path}")]
	Task PostAsync(string path, [Body] IDictionary<string, string?> fields, CancellationToken cancellationToken);

	[Delete("{**path}")]
	Task DeleteAsync(string path, CancellationToken cancellationToken);
}
