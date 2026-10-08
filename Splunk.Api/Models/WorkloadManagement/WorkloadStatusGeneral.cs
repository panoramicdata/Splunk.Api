using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>General workload management status (<c>workload-management-status</c>, <c>general</c>).</summary>
public sealed class WorkloadStatusGeneral
{
	/// <summary>Whether workload management is enabled.</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>Whether this operating system supports workload management (<c>isSupported</c>).</summary>
	[JsonPropertyName("isSupported")]
	public bool? IsSupported { get; init; }

	/// <summary>Whether basic mode is allowed (<c>allow_basic</c>).</summary>
	[JsonPropertyName("allow_basic")]
	public bool? AllowBasic { get; init; }

	/// <summary>The mode, for example <c>basic</c> (<c>mode</c>).</summary>
	[JsonPropertyName("mode")]
	public string? Mode { get; init; }

	/// <summary>Why workload management cannot run, if it cannot (<c>error_message</c>).</summary>
	[JsonPropertyName("error_message")]
	public string? ErrorMessage { get; init; }

	/// <summary>The operating system name (<c>os_name</c>).</summary>
	[JsonPropertyName("os_name")]
	public string? OsName { get; init; }

	/// <summary>The extended operating system name (<c>os_extended_name</c>).</summary>
	[JsonPropertyName("os_extended_name")]
	public string? OsExtendedName { get; init; }

	/// <summary>The operating system version (<c>os_version</c>).</summary>
	[JsonPropertyName("os_version")]
	public string? OsVersion { get; init; }

	/// <summary>The operating system build (<c>os_build</c>).</summary>
	[JsonPropertyName("os_build")]
	public string? OsBuild { get; init; }
}
