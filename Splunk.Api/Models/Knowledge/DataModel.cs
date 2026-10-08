using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A data model (<c>datamodel/model</c>).</summary>
public sealed class DataModel : SplunkContent
{
	/// <summary>The name shown in Splunk Web (<c>displayName</c>).</summary>
	[JsonPropertyName("displayName")]
	public string? DisplayName { get; init; }

	/// <summary>
	/// The data model definition as JSON (<c>description</c>): <c>modelName</c>, <c>displayName</c>, <c>description</c>,
	/// <c>objects</c> and so on. Listings return a summary with <c>objectSummary</c> instead of the objects.
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The acceleration settings as JSON (<c>acceleration</c>): <c>enabled</c>, <c>earliest_time</c>, <c>cron_schedule</c> and more.</summary>
	[JsonPropertyName("acceleration")]
	public string? Acceleration { get; init; }

	/// <summary>Whether the data model can be accelerated (<c>acceleration.allowed</c>).</summary>
	[JsonPropertyName("acceleration.allowed")]
	public bool? AccelerationAllowed { get; init; }

	/// <summary>A hash of the current definition (<c>eai:digest</c>).</summary>
	[JsonPropertyName("eai:digest")]
	public string? Digest { get; init; }

	/// <summary>The object type, <c>datamodel</c> (<c>eai:type</c>).</summary>
	[JsonPropertyName("eai:type")]
	public string? Type { get; init; }

	/// <summary>The dataset type (<c>dataset.type</c>).</summary>
	[JsonPropertyName("dataset.type")]
	public string? DatasetType { get; init; }

	/// <summary>Whether only the fields the model defines are returned (<c>strict_fields</c>).</summary>
	[JsonPropertyName("strict_fields")]
	public bool? StrictFields { get; init; }

	/// <summary>The tags the model's searches may use, comma-separated (<c>tags_whitelist</c>).</summary>
	[JsonPropertyName("tags_whitelist")]
	public string? TagsWhitelist { get; init; }
}
