using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The deployment client configuration of this instance (<c>deployment/client</c>).</summary>
/// <remarks><see cref="SplunkContent.Disabled"/> reports whether this instance is a deployment client.</remarks>
public sealed class DeploymentClientConfig : SplunkContent
{
	/// <summary>The server classes and apps this client belongs to, as <c>serverclass:app</c>.</summary>
	[JsonPropertyName("serverClasses")]
	public IReadOnlyList<string> ServerClasses { get; init; } = [];

	/// <summary>The deployment server, as <c>host:port</c>.</summary>
	[JsonPropertyName("targetUri")]
	public string? TargetUri { get; init; }
}
