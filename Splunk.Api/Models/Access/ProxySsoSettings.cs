using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// The settings of a ProxySSO configuration, shared by <see cref="ProxySsoConfigurationCreateRequest"/> and
/// <see cref="ProxySsoConfigurationUpdateRequest"/>.
/// </summary>
public abstract class ProxySsoSettings : SplunkFormRequest
{
	/// <summary>The role to use when no mapping is found.</summary>
	[JsonPropertyName("defaultRoleIfMissing")]
	public string? DefaultRoleIfMissing { get; init; }

	/// <summary>Users to exclude, separated by commas (<c>excludedUsers</c>).</summary>
	[JsonPropertyName("excludedUsers")]
	public string? ExcludedUsers { get; init; }

	/// <summary>Roles to exclude from automatic mapping, separated by commas (<c>excludedAutoMappedRoles</c>).</summary>
	[JsonPropertyName("excludedAutoMappedRoles")]
	public string? ExcludedAutoMappedRoles { get; init; }

	/// <summary>The older name of <see cref="ExcludedUsers"/>, as the reference documents it (<c>blacklistedUsers</c>).</summary>
	[JsonPropertyName("blacklistedUsers")]
	public string? BlacklistedUsers { get; init; }

	/// <summary>
	/// The older name of <see cref="ExcludedAutoMappedRoles"/>, as the reference documents it (<c>blacklistedAutoMappedRoles</c>).
	/// </summary>
	[JsonPropertyName("blacklistedAutoMappedRoles")]
	public string? BlacklistedAutoMappedRoles { get; init; }
}
