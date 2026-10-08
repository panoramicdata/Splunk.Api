using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>An LDAP authentication strategy (<c>authentication/providers/LDAP</c>): an LDAP stanza of <c>authentication.conf</c>.</summary>
public sealed class LdapStrategy : SplunkContent
{
	/// <summary>Whether the connection uses SSL.</summary>
	[JsonPropertyName("SSLEnabled")]
	public bool? SslEnabled { get; init; }

	/// <summary>Whether referrals are followed anonymously.</summary>
	[JsonPropertyName("anonymous_referrals")]
	public bool? AnonymousReferrals { get; init; }

	/// <summary>The distinguished name Splunk binds as.</summary>
	[JsonPropertyName("bindDN")]
	public string? BindDN { get; init; }

	/// <summary>The filter that identifies dynamic groups.</summary>
	[JsonPropertyName("dynamicGroupFilter")]
	public string? DynamicGroupFilter { get; init; }

	/// <summary>The dynamic group attribute holding the member URLs.</summary>
	[JsonPropertyName("dynamicMemberAttribute")]
	public string? DynamicMemberAttribute { get; init; }

	/// <summary>The user entry attribute holding the email address.</summary>
	[JsonPropertyName("emailAttribute")]
	public string? EmailAttribute { get; init; }

	/// <summary>The base distinguished names of group entries, separated by semicolons.</summary>
	[JsonPropertyName("groupBaseDN")]
	public string? GroupBaseDN { get; init; }

	/// <summary>The filter applied to group entries.</summary>
	[JsonPropertyName("groupBaseFilter")]
	public string? GroupBaseFilter { get; init; }

	/// <summary>The user entry attribute that group members' values refer to.</summary>
	[JsonPropertyName("groupMappingAttribute")]
	public string? GroupMappingAttribute { get; init; }

	/// <summary>The group entry attribute holding the members.</summary>
	[JsonPropertyName("groupMemberAttribute")]
	public string? GroupMemberAttribute { get; init; }

	/// <summary>The group entry attribute holding the group name.</summary>
	[JsonPropertyName("groupNameAttribute")]
	public string? GroupNameAttribute { get; init; }

	/// <summary>The LDAP server host name.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>Whether nested groups are expanded.</summary>
	[JsonPropertyName("nestedGroups")]
	public bool? NestedGroups { get; init; }

	/// <summary>The network timeout, in seconds.</summary>
	[JsonPropertyName("network_timeout")]
	public int? NetworkTimeout { get; init; }

	/// <summary>The LDAP server port.</summary>
	[JsonPropertyName("port")]
	public int? Port { get; init; }

	/// <summary>The user entry attribute holding the user's real name.</summary>
	[JsonPropertyName("realNameAttribute")]
	public string? RealNameAttribute { get; init; }

	/// <summary>The maximum number of entries a search returns.</summary>
	[JsonPropertyName("sizelimit")]
	public int? SizeLimit { get; init; }

	/// <summary>The maximum time, in seconds, a search may take.</summary>
	[JsonPropertyName("timelimit")]
	public int? TimeLimit { get; init; }

	/// <summary>The base distinguished names of user entries, separated by semicolons.</summary>
	[JsonPropertyName("userBaseDN")]
	public string? UserBaseDN { get; init; }

	/// <summary>The filter applied to user entries.</summary>
	[JsonPropertyName("userBaseFilter")]
	public string? UserBaseFilter { get; init; }

	/// <summary>The user entry attribute holding the user name.</summary>
	[JsonPropertyName("userNameAttribute")]
	public string? UserNameAttribute { get; init; }
}
