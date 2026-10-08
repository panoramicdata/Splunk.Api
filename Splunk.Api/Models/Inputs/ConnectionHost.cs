using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>How a network input sets the host field of events from a remote sender (<c>connection_host</c>).</summary>
public enum ConnectionHost
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>The sender's IP address.</summary>
	[JsonStringEnumMemberName("ip")]
	Ip,

	/// <summary>The reverse DNS name of the sender's IP address.</summary>
	[JsonStringEnumMemberName("dns")]
	Dns,

	/// <summary>The input's own <c>host</c> setting.</summary>
	[JsonStringEnumMemberName("none")]
	None
}
