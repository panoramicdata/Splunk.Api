using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// The optional settings of an LDAP strategy, shared by <see cref="LdapStrategyCreateRequest"/> and
/// <see cref="LdapStrategyUpdateRequest"/>. See the LDAP settings of <c>authentication.conf</c>; set others in
/// <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public abstract class LdapStrategySettings : SplunkFormRequest
{
	/// <summary>The LDAP server port. Splunk's default is 389, or 636 with SSL.</summary>
	[JsonPropertyName("port")]
	public int? Port { get; init; }

	/// <summary>Whether to connect with SSL.</summary>
	[JsonPropertyName("SSLEnabled")]
	public bool? SslEnabled { get; init; }

	/// <summary>The distinguished name to bind as; omit to bind anonymously.</summary>
	[JsonPropertyName("bindDN")]
	public string? BindDN { get; init; }

	/// <summary>The password of <see cref="BindDN"/>. A secret.</summary>
	[JsonPropertyName("bindDNpassword")]
	public string? BindDNPassword { get; init; }

	/// <summary>The filter applied to user entries.</summary>
	[JsonPropertyName("userBaseFilter")]
	public string? UserBaseFilter { get; init; }

	/// <summary>The user entry attribute holding the email address.</summary>
	[JsonPropertyName("emailAttribute")]
	public string? EmailAttribute { get; init; }

	/// <summary>The filter applied to group entries.</summary>
	[JsonPropertyName("groupBaseFilter")]
	public string? GroupBaseFilter { get; init; }

	/// <summary>The user entry attribute that group members' values refer to.</summary>
	[JsonPropertyName("groupMappingAttribute")]
	public string? GroupMappingAttribute { get; init; }

	/// <summary>The filter that identifies dynamic groups.</summary>
	[JsonPropertyName("dynamicGroupFilter")]
	public string? DynamicGroupFilter { get; init; }

	/// <summary>The dynamic group attribute holding the member URLs.</summary>
	[JsonPropertyName("dynamicMemberAttribute")]
	public string? DynamicMemberAttribute { get; init; }

	/// <summary>Whether to expand nested groups.</summary>
	[JsonPropertyName("nestedGroups")]
	public bool? NestedGroups { get; init; }

	/// <summary>Whether to follow referrals anonymously.</summary>
	[JsonPropertyName("anonymous_referrals")]
	public bool? AnonymousReferrals { get; init; }

	/// <summary>The maximum number of entries a search returns.</summary>
	[JsonPropertyName("sizelimit")]
	public int? SizeLimit { get; init; }

	/// <summary>The maximum time, in seconds, a search may take.</summary>
	[JsonPropertyName("timelimit")]
	public int? TimeLimit { get; init; }

	/// <summary>The network timeout, in seconds.</summary>
	[JsonPropertyName("network_timeout")]
	public int? NetworkTimeout { get; init; }
}
