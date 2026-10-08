using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>Changes to how the deployment server distributes an app (<c>POST deployment/server/applications/{name}</c>).</summary>
/// <remarks>Whitelist and blacklist filters are numbered families (<c>whitelist.0</c>, <c>whitelist.1</c>, <c>blacklist.0</c>, ...; ordinals start at 0 and are consecutive): set them in <see cref="SplunkFormRequest.AdditionalParameters"/>.</remarks>
public sealed class DeploymentApplicationUpdateRequest : DeploymentServerClassSettings
{
	/// <summary>The server class to map the app to; leave unset when <see cref="Deinstall"/> is <see langword="true"/>.</summary>
	[JsonPropertyName("serverclass")]
	public string? ServerClass { get; init; }

	/// <summary>Whether to remove the app from every server class and from the clients.</summary>
	[JsonPropertyName("deinstall")]
	public bool? Deinstall { get; init; }

	/// <summary>Whether to remove the mapping of the app to <see cref="ServerClass"/>.</summary>
	[JsonPropertyName("unmap")]
	public bool? Unmap { get; init; }
}
