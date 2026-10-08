using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Changes an LDAP strategy (<c>POST authentication/providers/LDAP/{LDAP_strategy_name}</c>).</summary>
public sealed class LdapStrategyUpdateRequest : LdapStrategySettings
{
	/// <summary>The LDAP server host name.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The base distinguished names of user entries, separated by semicolons.</summary>
	[JsonPropertyName("userBaseDN")]
	public string? UserBaseDN { get; init; }

	/// <summary>The user entry attribute holding the user name.</summary>
	[JsonPropertyName("userNameAttribute")]
	public string? UserNameAttribute { get; init; }

	/// <summary>The user entry attribute holding the user's real name.</summary>
	[JsonPropertyName("realNameAttribute")]
	public string? RealNameAttribute { get; init; }

	/// <summary>The base distinguished names of group entries, separated by semicolons.</summary>
	[JsonPropertyName("groupBaseDN")]
	public string? GroupBaseDN { get; init; }

	/// <summary>The group entry attribute holding the group name.</summary>
	[JsonPropertyName("groupNameAttribute")]
	public string? GroupNameAttribute { get; init; }

	/// <summary>The group entry attribute holding the members.</summary>
	[JsonPropertyName("groupMemberAttribute")]
	public string? GroupMemberAttribute { get; init; }
}
