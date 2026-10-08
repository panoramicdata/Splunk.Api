using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A new server class (<c>POST deployment/server/serverclasses</c>).</summary>
/// <remarks>Whitelist and blacklist filters are numbered families (<c>whitelist.0</c>, <c>whitelist.1</c>, <c>blacklist.0</c>, ...; ordinals start at 0 and are consecutive): set them in <see cref="SplunkFormRequest.AdditionalParameters"/>.</remarks>
public sealed class DeploymentServerClassCreateRequest : DeploymentServerClassSettings
{
	/// <summary>The server class name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
