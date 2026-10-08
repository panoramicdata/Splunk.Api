using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>Disk space of a file system holding Splunk data (<c>server/status/partitions-space</c>).</summary>
public sealed class PartitionSpace : SplunkContent
{
	/// <summary>The mount point (<c>mount_point</c>).</summary>
	[JsonPropertyName("mount_point")]
	public string? MountPoint { get; init; }

	/// <summary>The file system type, for example <c>ext4</c>; not always reported (<c>fs_type</c>).</summary>
	[JsonPropertyName("fs_type")]
	public string? FileSystemType { get; init; }

	/// <summary>The capacity in megabytes (<c>capacity</c>).</summary>
	[JsonPropertyName("capacity")]
	public double CapacityMB { get; init; }

	/// <summary>The free space in megabytes (<c>free</c>).</summary>
	[JsonPropertyName("free")]
	public double FreeMB { get; init; }

	/// <summary>The space available to Splunk in megabytes (<c>available</c>).</summary>
	[JsonPropertyName("available")]
	public double AvailableMB { get; init; }
}
