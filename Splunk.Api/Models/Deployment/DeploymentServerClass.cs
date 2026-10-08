using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A server class of the deployment server (<c>deployment/server/serverclasses</c>).</summary>
/// <remarks>Whitelist and blacklist entries (<c>whitelist.0</c>, ...) and <c>repositoryList</c> are in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class DeploymentServerClass : DeploymentServerClassContent
{
	/// <summary>The number of blacklist entries.</summary>
	[JsonPropertyName("blacklist-size")]
	public int? BlacklistSize { get; init; }

	/// <summary>The number of downloads in progress.</summary>
	[JsonPropertyName("currentDownloads")]
	public int? CurrentDownloads { get; init; }

	/// <summary>When the server class was loaded.</summary>
	[JsonPropertyName("loadTime")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LoadTime { get; init; }

	/// <summary>The number of whitelist entries.</summary>
	[JsonPropertyName("whitelist-size")]
	public int? WhitelistSize { get; init; }
}
