using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

public sealed partial class License
{
	/// <summary>The number of violations allowed within <see cref="WindowPeriod"/> before searching is disabled.</summary>
	[JsonPropertyName("max_violations")]
	public int MaxViolations { get; init; }

	/// <summary>The rolling period, in days, over which violations are counted.</summary>
	[JsonPropertyName("window_period")]
	public int WindowPeriod { get; init; }

	/// <summary>The maximum number of users; 0 or 4294967295 for no limit.</summary>
	[JsonPropertyName("max_users")]
	public long MaxUsers { get; init; }

	/// <summary>The maximum retention size; 0 for no limit.</summary>
	[JsonPropertyName("max_retention_size")]
	public long MaxRetentionSize { get; init; }

	/// <summary>
	/// The maximum quota of the license's stack. Splunk reports 2^64 (as <c>18446744073709552000</c>) for no limit, so this
	/// is a <see cref="double"/>.
	/// </summary>
	[JsonPropertyName("max_stack_quota")]
	public double MaxStackQuota { get; init; }

	/// <summary>For a relative (trial) license, how long it lasts, in seconds.</summary>
	[JsonPropertyName("relative_expiration_interval")]
	public long RelativeExpirationInterval { get; init; }

	/// <summary>For a relative (trial) license, when its period started.</summary>
	[JsonPropertyName("relative_expiration_start")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? RelativeExpirationStart { get; init; }

	/// <summary>The roles the license allows.</summary>
	[JsonPropertyName("allowedRoles")]
	public IReadOnlyList<string> AllowedRoles { get; init; } = [];

	/// <summary>The roles the license lets users be assigned.</summary>
	[JsonPropertyName("assignableRoles")]
	public IReadOnlyList<string> AssignableRoles { get; init; } = [];

	/// <summary>Notes about the license.</summary>
	[JsonPropertyName("notes")]
	public string? Notes { get; init; }
}
