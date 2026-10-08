using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The order in which a server class applies its filters.</summary>
public enum DeploymentFilterType
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>Whitelist filters first: clients do not match unless a whitelist entry matches and no blacklist entry does.</summary>
	[JsonStringEnumMemberName("whitelist")]
	Whitelist,

	/// <summary>Blacklist filters first: clients match unless a blacklist entry matches and no whitelist entry does.</summary>
	[JsonStringEnumMemberName("blacklist")]
	Blacklist
}
