using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>The status of the co-hosted KV store service (<c>kvstore/status</c>, <c>cohosted</c>).</summary>
public sealed class KvStoreCohostedStatus
{
	/// <summary>The status, for example <c>ready</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>The backing service.</summary>
	[JsonPropertyName("serviceInfo")]
	public KvStoreServiceInfo? ServiceInfo { get; init; }
}
