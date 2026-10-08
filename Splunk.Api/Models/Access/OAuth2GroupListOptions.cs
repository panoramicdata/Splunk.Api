using Refit;

namespace Splunk.Api.Models.Access;

/// <summary>Filters for listing OAuth 2.0 group mappings (<c>GET admin/oauth2-groups</c>).</summary>
public sealed class OAuth2GroupListOptions : ListOptions
{
	/// <summary>Only mappings of this OAuth configuration (<c>config</c>).</summary>
	[AliasAs("config")]
	public string? Config { get; init; }
}
