using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>Information about the Splunk server (<c>server/info</c>).</summary>
public sealed class ServerInfo : SplunkContent
{
	/// <summary>The Splunk version, for example <c>10.6.0.5</c>.</summary>
	[JsonPropertyName("version")]
	public string? Version { get; init; }

	/// <summary>The build identifier.</summary>
	[JsonPropertyName("build")]
	public string? Build { get; init; }

	/// <summary>The server's name (<c>serverName</c>).</summary>
	[JsonPropertyName("serverName")]
	public string? ServerName { get; init; }

	/// <summary>The host name.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The fully qualified host name.</summary>
	[JsonPropertyName("host_fqdn")]
	public string? HostFqdn { get; init; }

	/// <summary>The server's unique identifier.</summary>
	[JsonPropertyName("guid")]
	public string? ServerGuid { get; init; }

	/// <summary>The product type, for example <c>enterprise</c> or <c>cloud</c>.</summary>
	[JsonPropertyName("product_type")]
	public string? ProductType { get; init; }

	/// <summary>The server mode, for example <c>normal</c>.</summary>
	[JsonPropertyName("mode")]
	public string? Mode { get; init; }

	/// <summary>The roles this server plays, for example <c>indexer</c>, <c>search_head</c>, <c>kv_store</c>.</summary>
	[JsonPropertyName("server_roles")]
	public IReadOnlyList<string> ServerRoles { get; init; } = [];

	/// <summary>The overall health, for example <c>green</c>, <c>yellow</c> or <c>red</c>.</summary>
	[JsonPropertyName("health_info")]
	public string? HealthInfo { get; init; }

	/// <summary>The KV store status, for example <c>ready</c>.</summary>
	[JsonPropertyName("kvStoreStatus")]
	public string? KvStoreStatus { get; init; }

	/// <summary>The active license group, for example <c>Enterprise</c>, <c>Trial</c> or <c>Free</c>.</summary>
	[JsonPropertyName("activeLicenseGroup")]
	public string? ActiveLicenseGroup { get; init; }

	/// <summary>The license state, for example <c>OK</c>.</summary>
	[JsonPropertyName("licenseState")]
	public string? LicenseState { get; init; }

	/// <summary>Whether the server runs a trial license.</summary>
	[JsonPropertyName("isTrial")]
	public bool IsTrial { get; init; }

	/// <summary>Whether the server runs the free license.</summary>
	[JsonPropertyName("isFree")]
	public bool IsFree { get; init; }

	/// <summary>Whether the server forwards data.</summary>
	[JsonPropertyName("isForwarding")]
	public bool IsForwarding { get; init; }

	/// <summary>Whether FIPS mode is on.</summary>
	[JsonPropertyName("fips_mode")]
	public bool FipsMode { get; init; }

	/// <summary>Whether the server is shutting down.</summary>
	[JsonPropertyName("shutting_down")]
	public bool ShuttingDown { get; init; }

	/// <summary>The operating system name. Deprecated by Splunk here: read it from <c>server/sysinfo</c> instead.</summary>
	[JsonPropertyName("os_name")]
	public string? OsName { get; init; }

	/// <summary>The operating system version. Deprecated by Splunk here: read it from <c>server/sysinfo</c> instead.</summary>
	[JsonPropertyName("os_version")]
	public string? OsVersion { get; init; }

	/// <summary>The CPU architecture, for example <c>x86_64</c>. Deprecated by Splunk here: read it from <c>server/sysinfo</c> instead.</summary>
	[JsonPropertyName("cpu_arch")]
	public string? CpuArchitecture { get; init; }

	/// <summary>The number of physical cores. Deprecated by Splunk here: read it from <c>server/sysinfo</c> instead.</summary>
	[JsonPropertyName("numberOfCores")]
	public int NumberOfCores { get; init; }

	/// <summary>The number of virtual cores. Deprecated by Splunk here: read it from <c>server/sysinfo</c> instead.</summary>
	[JsonPropertyName("numberOfVirtualCores")]
	public int NumberOfVirtualCores { get; init; }

	/// <summary>Physical memory in megabytes. Deprecated by Splunk here: read it from <c>server/sysinfo</c> instead.</summary>
	[JsonPropertyName("physicalMemoryMB")]
	public long PhysicalMemoryMB { get; init; }

	/// <summary>When the server started.</summary>
	[JsonPropertyName("startup_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? StartupTime { get; init; }
}
