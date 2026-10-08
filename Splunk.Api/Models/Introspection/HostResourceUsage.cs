using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>Host-wide CPU, memory and paging use (<c>server/status/resource-usage/hostwide</c>).</summary>
public sealed class HostResourceUsage : SplunkContent
{
	/// <summary>The CPU architecture (<c>cpu_arch</c>).</summary>
	[JsonPropertyName("cpu_arch")]
	public string? CpuArchitecture { get; init; }

	/// <summary>Physical cores (<c>cpu_count</c>).</summary>
	[JsonPropertyName("cpu_count")]
	public int CpuCount { get; init; }

	/// <summary>Virtual cores (<c>virtual_cpu_count</c>).</summary>
	[JsonPropertyName("virtual_cpu_count")]
	public int VirtualCpuCount { get; init; }

	/// <summary>Idle CPU percentage (<c>cpu_idle_pct</c>).</summary>
	[JsonPropertyName("cpu_idle_pct")]
	public double CpuIdlePercent { get; init; }

	/// <summary>System CPU percentage (<c>cpu_system_pct</c>).</summary>
	[JsonPropertyName("cpu_system_pct")]
	public double CpuSystemPercent { get; init; }

	/// <summary>User CPU percentage (<c>cpu_user_pct</c>).</summary>
	[JsonPropertyName("cpu_user_pct")]
	public double CpuUserPercent { get; init; }

	/// <summary>The one-minute load average divided by cores (<c>normalized_load_avg_1min</c>).</summary>
	[JsonPropertyName("normalized_load_avg_1min")]
	public double NormalizedLoadAverage1Min { get; init; }

	/// <summary>Physical memory in megabytes (<c>mem</c>).</summary>
	[JsonPropertyName("mem")]
	public double MemoryMB { get; init; }

	/// <summary>Memory in use in megabytes (<c>mem_used</c>).</summary>
	[JsonPropertyName("mem_used")]
	public double MemoryUsedMB { get; init; }

	/// <summary>Swap in megabytes (<c>swap</c>).</summary>
	[JsonPropertyName("swap")]
	public double SwapMB { get; init; }

	/// <summary>Swap in use in megabytes (<c>swap_used</c>).</summary>
	[JsonPropertyName("swap_used")]
	public double SwapUsedMB { get; init; }

	/// <summary>Processes forked since boot (<c>forks</c>).</summary>
	[JsonPropertyName("forks")]
	public long Forks { get; init; }

	/// <summary>Runnable processes (<c>runnable_process_count</c>).</summary>
	[JsonPropertyName("runnable_process_count")]
	public int RunnableProcessCount { get; init; }

	/// <summary>Pages paged out since boot (<c>pg_paged_out</c>).</summary>
	[JsonPropertyName("pg_paged_out")]
	public long PagesPagedOut { get; init; }

	/// <summary>Pages swapped out since boot (<c>pg_swapped_out</c>).</summary>
	[JsonPropertyName("pg_swapped_out")]
	public long PagesSwappedOut { get; init; }

	/// <summary>The operating system name (<c>os_name</c>).</summary>
	[JsonPropertyName("os_name")]
	public string? OsName { get; init; }

	/// <summary>The extended operating system name (<c>os_name_ext</c>).</summary>
	[JsonPropertyName("os_name_ext")]
	public string? OsNameExtended { get; init; }

	/// <summary>The operating system version (<c>os_version</c>).</summary>
	[JsonPropertyName("os_version")]
	public string? OsVersion { get; init; }

	/// <summary>The operating system build (<c>os_build</c>).</summary>
	[JsonPropertyName("os_build")]
	public string? OsBuild { get; init; }

	/// <summary>The Splunk version (<c>splunk_version</c>).</summary>
	[JsonPropertyName("splunk_version")]
	public string? SplunkVersion { get; init; }

	/// <summary>The instance's GUID (<c>instance_guid</c>).</summary>
	[JsonPropertyName("instance_guid")]
	public string? InstanceGuid { get; init; }
}
