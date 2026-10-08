using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The manual detention state of an indexer cluster peer or search head cluster member.</summary>
public enum ManualDetentionMode
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>Not in detention (the default).</summary>
	[JsonStringEnumMemberName("off")]
	Off,

	/// <summary>In manual detention. A peer closes its TCP, UDP and HEC data ports; a search head accepts no new searches.</summary>
	[JsonStringEnumMemberName("on")]
	On,

	/// <summary>In manual detention with the data ports left open, so the peer keeps indexing (indexer cluster peers only).</summary>
	[JsonStringEnumMemberName("on_ports_enabled")]
	OnPortsEnabled
}
