using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The distributed search settings (<c>search/distributed/config</c>).</summary>
/// <remarks>Deprecated keys (<c>autoAddServers</c>, <c>heartbeat*</c>, <c>skipOurselves</c>, <c>ttl</c>) are in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class DistributedSearchConfig : SplunkContent
{
	/// <summary>Whether distributed search is on.</summary>
	[JsonPropertyName("dist_search_enabled")]
	public bool? DistributedSearchEnabled { get; init; }

	/// <summary>The configured search peers, comma-separated.</summary>
	[JsonPropertyName("servers")]
	public string? Servers { get; init; }

	/// <summary>Whether knowledge bundles are replicated to peers (rather than mounted).</summary>
	[JsonPropertyName("shareBundles")]
	public bool? ShareBundles { get; init; }

	/// <summary>The connection timeout to peers, in seconds.</summary>
	[JsonPropertyName("connectionTimeout")]
	public int? ConnectionTimeout { get; init; }

	/// <summary>The send timeout, in seconds.</summary>
	[JsonPropertyName("sendTimeout")]
	public int? SendTimeout { get; init; }

	/// <summary>The receive timeout, in seconds.</summary>
	[JsonPropertyName("receiveTimeout")]
	public int? ReceiveTimeout { get; init; }

	/// <summary>The connection timeout when reading a peer's <c>server/info</c>, in seconds.</summary>
	[JsonPropertyName("statusTimeout")]
	public int? StatusTimeout { get; init; }

	/// <summary>Deprecated: use the connection, send and receive timeouts.</summary>
	[JsonPropertyName("serverTimeout")]
	public int? ServerTimeout { get; init; }

	/// <summary>Whether peers that time out are dropped.</summary>
	[JsonPropertyName("removedTimedOutServers")]
	public bool? RemovedTimedOutServers { get; init; }

	/// <summary>How often, in seconds, dropped peers are checked again.</summary>
	[JsonPropertyName("checkTimedOutServersFrequency")]
	public int? CheckTimedOutServersFrequency { get; init; }

	/// <summary>File names that are not replicated.</summary>
	[JsonPropertyName("blacklistNames")]
	public string? BlacklistNames { get; init; }

	/// <summary>URLs that are not replicated.</summary>
	[JsonPropertyName("blacklistURLs")]
	public string? BlacklistUrls { get; init; }
}
