using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>Query parameters for <c>GET saved/searches/{name}/suppress</c>.</summary>
public sealed class SavedSearchSuppressionOptions
{
	/// <summary>The suppression key to check (<c>key</c>).</summary>
	[AliasAs("key")]
	public string? Key { get; init; }

	/// <summary>The suppression expiry to report against (<c>expiration</c>).</summary>
	[AliasAs("expiration")]
	public string? Expiration { get; init; }
}
