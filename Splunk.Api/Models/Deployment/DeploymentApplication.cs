using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>An app the deployment server distributes (<c>deployment/server/applications</c>).</summary>
/// <remarks>Whitelist and blacklist entries (<c>whitelist.0</c>, ...) are in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class DeploymentApplication : DeploymentServerClassContent
{
	/// <summary>The path of the compressed app bundle.</summary>
	[JsonPropertyName("archive")]
	public string? Archive { get; init; }

	/// <summary>When the deployment server last loaded the app, as Splunk formats it (for example <c>Wed Jul 31 14:17:23 2013</c>); an app mapped to no server class is not loaded.</summary>
	[JsonPropertyName("loadtime")]
	public string? LoadTime { get; init; }

	/// <summary>The server class the app was last mapped to.</summary>
	[JsonPropertyName("serverclass")]
	public string? ServerClass { get; init; }

	/// <summary>The server classes the app belongs to.</summary>
	[JsonPropertyName("serverclasses")]
	public IReadOnlyList<string> ServerClasses { get; init; } = [];

	/// <summary>The size of the compressed bundle, in bytes.</summary>
	[JsonPropertyName("size")]
	public long? Size { get; init; }
}
