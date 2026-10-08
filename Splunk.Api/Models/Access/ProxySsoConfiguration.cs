using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>A ProxySSO authentication configuration (<c>admin/ProxySSO-auth</c>).</summary>
public sealed class ProxySsoConfiguration : SplunkContent
{
	/// <summary>The older name of <see cref="ExcludedAutoMappedRoles"/> (<c>blacklistedAutoMappedRoles</c>).</summary>
	[JsonPropertyName("blacklistedAutoMappedRoles")]
	public string? BlacklistedAutoMappedRoles { get; init; }

	/// <summary>The older name of <see cref="ExcludedUsers"/> (<c>blacklistedUsers</c>).</summary>
	[JsonPropertyName("blacklistedUsers")]
	public string? BlacklistedUsers { get; init; }

	/// <summary>The role to use when no mapping is found.</summary>
	[JsonPropertyName("defaultRoleIfMissing")]
	public string? DefaultRoleIfMissing { get; init; }

	/// <summary>The roles excluded from automatic mapping, separated by commas (<c>excludedAutoMappedRoles</c>).</summary>
	[JsonPropertyName("excludedAutoMappedRoles")]
	public string? ExcludedAutoMappedRoles { get; init; }

	/// <summary>The excluded users, separated by commas (<c>excludedUsers</c>).</summary>
	[JsonPropertyName("excludedUsers")]
	public string? ExcludedUsers { get; init; }

	/// <summary>The configuration name.</summary>
	[JsonPropertyName("title")]
	public string? Title { get; init; }
}
