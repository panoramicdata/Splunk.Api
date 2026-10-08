using Refit;

namespace Splunk.Api.Models.Applications;

/// <summary>Options for getting an app (<c>GET apps/local/{name}</c>).</summary>
public sealed class AppGetOptions
{
	/// <summary>Whether to reload the app's objects first (<c>refresh</c>).</summary>
	[AliasAs("refresh")]
	public bool? Refresh { get; init; }
}
