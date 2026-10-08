using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The status of an authentication token.</summary>
public enum TokenStatus
{
	/// <summary>A status this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>The token can be used.</summary>
	[JsonStringEnumMemberName("enabled")]
	Enabled = 1,

	/// <summary>The token is disabled and is rejected.</summary>
	[JsonStringEnumMemberName("disabled")]
	Disabled = 2
}
