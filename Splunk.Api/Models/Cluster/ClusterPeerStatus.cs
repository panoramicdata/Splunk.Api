using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The status of an indexer cluster peer or search head cluster member, as the manager or captain sees it.</summary>
public enum ClusterPeerStatus
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>Up.</summary>
	[JsonStringEnumMemberName("Up")]
	Up,

	/// <summary>Joining.</summary>
	[JsonStringEnumMemberName("Pending")]
	Pending,

	/// <summary>In automatic detention (for example, low disk).</summary>
	[JsonStringEnumMemberName("AutomaticDetention")]
	AutomaticDetention,

	/// <summary>In manual detention with its data ports open.</summary>
	[JsonStringEnumMemberName("ManualDetention-PortsEnabled")]
	ManualDetentionPortsEnabled,

	/// <summary>In manual detention.</summary>
	[JsonStringEnumMemberName("ManualDetention")]
	ManualDetention,

	/// <summary>Restarting.</summary>
	[JsonStringEnumMemberName("Restarting")]
	Restarting,

	/// <summary>Shutting down.</summary>
	[JsonStringEnumMemberName("ShuttingDown")]
	ShuttingDown,

	/// <summary>Handing its primary copies to other peers.</summary>
	[JsonStringEnumMemberName("ReassigningPrimaries")]
	ReassigningPrimaries,

	/// <summary>Being decommissioned.</summary>
	[JsonStringEnumMemberName("Decommissioning")]
	Decommissioning,

	/// <summary>Shut down gracefully.</summary>
	[JsonStringEnumMemberName("GracefulShutdown")]
	GracefulShutdown,

	/// <summary>Stopped.</summary>
	[JsonStringEnumMemberName("Stopped")]
	Stopped,

	/// <summary>Down.</summary>
	[JsonStringEnumMemberName("Down")]
	Down,

	/// <summary>Being added in a batch.</summary>
	[JsonStringEnumMemberName("BatchAdding")]
	BatchAdding
}
