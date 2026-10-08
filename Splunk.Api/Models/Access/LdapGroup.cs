using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>An LDAP group and the Splunk roles mapped to it (<c>admin/LDAP-groups</c>).</summary>
public sealed class LdapGroup : SplunkContent
{
	/// <summary>The roles mapped to the group.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string> Roles { get; init; } = [];

	/// <summary>The LDAP strategy the group belongs to.</summary>
	[JsonPropertyName("strategy")]
	public string? Strategy { get; init; }

	/// <summary>The group type.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>The users in the group.</summary>
	[JsonPropertyName("users")]
	public IReadOnlyList<string> Users { get; init; } = [];
}
