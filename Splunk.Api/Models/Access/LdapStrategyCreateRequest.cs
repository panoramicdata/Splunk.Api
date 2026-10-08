using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates an LDAP strategy (<c>POST authentication/providers/LDAP</c>).</summary>
public sealed class LdapStrategyCreateRequest : LdapStrategySettings
{
	/// <summary>The strategy name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The LDAP server host name.</summary>
	[JsonPropertyName("host")]
	public required string Host { get; init; }

	/// <summary>The base distinguished names of user entries, separated by semicolons.</summary>
	[JsonPropertyName("userBaseDN")]
	public required string UserBaseDN { get; init; }

	/// <summary>The user entry attribute holding the user name, for example <c>uid</c> or <c>sAMAccountName</c>.</summary>
	[JsonPropertyName("userNameAttribute")]
	public required string UserNameAttribute { get; init; }

	/// <summary>The user entry attribute holding the user's real name, for example <c>cn</c>.</summary>
	[JsonPropertyName("realNameAttribute")]
	public required string RealNameAttribute { get; init; }

	/// <summary>The base distinguished names of group entries, separated by semicolons.</summary>
	[JsonPropertyName("groupBaseDN")]
	public required string GroupBaseDN { get; init; }

	/// <summary>The group entry attribute holding the group name, for example <c>cn</c>.</summary>
	[JsonPropertyName("groupNameAttribute")]
	public required string GroupNameAttribute { get; init; }

	/// <summary>The group entry attribute holding the members, for example <c>member</c>.</summary>
	[JsonPropertyName("groupMemberAttribute")]
	public required string GroupMemberAttribute { get; init; }
}
