using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The machine's resources and operating system settings (<c>server/sysinfo</c>).</summary>
public sealed class SystemInfo : SplunkContent
{
	/// <summary>The CPU architecture, for example <c>x86_64</c> (<c>cpu_arch</c>).</summary>
	[JsonPropertyName("cpu_arch")]
	public string? CpuArchitecture { get; init; }

	/// <summary>Physical cores (<c>numberOfCores</c>).</summary>
	[JsonPropertyName("numberOfCores")]
	public int NumberOfCores { get; init; }

	/// <summary>Virtual cores (<c>numberOfVirtualCores</c>).</summary>
	[JsonPropertyName("numberOfVirtualCores")]
	public int NumberOfVirtualCores { get; init; }

	/// <summary>Physical memory in megabytes (<c>physicalMemoryMB</c>).</summary>
	[JsonPropertyName("physicalMemoryMB")]
	public long PhysicalMemoryMB { get; init; }

	/// <summary>The operating system name (<c>os_name</c>).</summary>
	[JsonPropertyName("os_name")]
	public string? OsName { get; init; }

	/// <summary>The extended operating system name (<c>os_name_extended</c>).</summary>
	[JsonPropertyName("os_name_extended")]
	public string? OsNameExtended { get; init; }

	/// <summary>The operating system version (<c>os_version</c>).</summary>
	[JsonPropertyName("os_version")]
	public string? OsVersion { get; init; }

	/// <summary>The operating system build (<c>os_build</c>).</summary>
	[JsonPropertyName("os_build")]
	public string? OsBuild { get; init; }

	/// <summary>The transparent huge pages settings; Linux only (<c>transparent_hugepages</c>).</summary>
	[JsonPropertyName("transparent_hugepages")]
	public TransparentHugePages? TransparentHugePages { get; init; }

	/// <summary>splunkd's resource limits; Unix only (<c>ulimits</c>).</summary>
	[JsonPropertyName("ulimits")]
	public ProcessLimits? Limits { get; init; }
}
