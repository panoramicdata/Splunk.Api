using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The bucket to roll from hot to warm (<c>POST cluster/manager/control/control/roll-hot-buckets</c>).</summary>
public sealed class ClusterRollHotBucketRequest : SplunkFormRequest
{
	/// <summary>The bucket ID, for example <c>_audit~2~1A3889D7-954B-4CE6-B071-01B438DE9865</c>.</summary>
	[JsonPropertyName("bucket_id")]
	public required string BucketId { get; init; }
}
