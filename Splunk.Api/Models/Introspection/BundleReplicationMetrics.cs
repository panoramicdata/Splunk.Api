using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>
/// Knowledge bundle replication metrics of a search head (<c>server/introspection/search/distributed</c>). The
/// <c>window_metrics</c> entry has the numbers; <c>per_searchhead_metrics</c> is empty on a standalone instance.
/// </summary>
public sealed class BundleReplicationMetrics : SplunkContent
{
	/// <summary>The number of bundle files (<c>bundle_file_count</c>).</summary>
	[JsonPropertyName("bundle_file_count")]
	public long? BundleFileCount { get; init; }

	/// <summary>The number of full (baseline) replications (<c>baseline_count</c>).</summary>
	[JsonPropertyName("baseline_count")]
	public long? BaselineCount { get; init; }

	/// <summary>The number of delta replications (<c>delta_count</c>).</summary>
	[JsonPropertyName("delta_count")]
	public long? DeltaCount { get; init; }

	/// <summary>The average replication size in bytes (<c>average_bytes</c>).</summary>
	[JsonPropertyName("average_bytes")]
	public double? AverageBytes { get; init; }

	/// <summary>The average replication time in milliseconds (<c>average_msecs</c>).</summary>
	[JsonPropertyName("average_msecs")]
	public double? AverageMsecs { get; init; }

	/// <summary>The average baseline bundle size (<c>average_baseline_file_size</c>).</summary>
	[JsonPropertyName("average_baseline_file_size")]
	public double? AverageBaselineFileSize { get; init; }

	/// <summary>The average baseline replication time in milliseconds (<c>average_baseline_msecs</c>).</summary>
	[JsonPropertyName("average_baseline_msecs")]
	public double? AverageBaselineMsecs { get; init; }

	/// <summary>The average delta bundle size (<c>average_delta_file_size</c>).</summary>
	[JsonPropertyName("average_delta_file_size")]
	public double? AverageDeltaFileSize { get; init; }

	/// <summary>The average delta replication time in milliseconds (<c>average_delta_msecs</c>).</summary>
	[JsonPropertyName("average_delta_msecs")]
	public double? AverageDeltaMsecs { get; init; }
}
