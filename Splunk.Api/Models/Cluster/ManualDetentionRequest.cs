using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Puts a peer or search head cluster member into manual detention or takes it out.</summary>
public sealed class ManualDetentionRequest : SplunkFormRequest
{
	/// <summary>The detention state.</summary>
	[JsonPropertyName("manual_detention")]
	public required ManualDetentionMode ManualDetention { get; init; }
}
