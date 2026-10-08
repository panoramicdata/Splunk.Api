using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>How to run one statement of an SPL2 dispatch. Leave a property <see langword="null"/> for Splunk's default.</summary>
public sealed class Spl2DispatchQuery
{
	/// <summary>The earliest time (<c>earliest</c>); Splunk's default is <c>-24h@h</c>.</summary>
	[JsonPropertyName("earliest")]
	public string? Earliest { get; init; }

	/// <summary>The latest time (<c>latest</c>); Splunk's default is <c>now</c>.</summary>
	[JsonPropertyName("latest")]
	public string? Latest { get; init; }

	/// <summary>The time zone of the time range (<c>timezone</c>), for example <c>Etc/UTC</c>.</summary>
	[JsonPropertyName("timezone")]
	public string? Timezone { get; init; }

	/// <summary>The time relative times are measured from, in epoch seconds (<c>relativeTimeAnchor</c>).</summary>
	[JsonPropertyName("relativeTimeAnchor")]
	public string? RelativeTimeAnchor { get; init; }

	/// <summary>Whether to collect an event summary (<c>collectEventSummary</c>).</summary>
	[JsonPropertyName("collectEventSummary")]
	public bool? CollectEventSummary { get; init; }

	/// <summary>Whether to collect a field summary (<c>collectFieldSummary</c>).</summary>
	[JsonPropertyName("collectFieldSummary")]
	public bool? CollectFieldSummary { get; init; }

	/// <summary>Whether to collect timeline buckets (<c>collectTimeBuckets</c>).</summary>
	[JsonPropertyName("collectTimeBuckets")]
	public bool? CollectTimeBuckets { get; init; }

	/// <summary>The ad hoc search level (<c>adhocSearchLevel</c>): <c>fast</c>, <c>smart</c> or <c>verbose</c>.</summary>
	[JsonPropertyName("adhocSearchLevel")]
	public string? AdhocSearchLevel { get; init; }
}
