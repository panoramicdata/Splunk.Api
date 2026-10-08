using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>The service that backs a co-hosted KV store (<c>kvstore/status</c>, <c>cohosted.serviceInfo</c>).</summary>
public sealed class KvStoreServiceInfo
{
	/// <summary>The service URL.</summary>
	[JsonPropertyName("service")]
	public string? Service { get; init; }

	/// <summary>The service type, for example <c>Pdl</c>.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }
}
