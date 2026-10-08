using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>
/// The base of every typed entry content. Splunk returns many properties, varying by version, app and configuration;
/// any not modelled by the derived type are kept in <see cref="AdditionalProperties"/> rather than lost.
/// </summary>
public abstract class SplunkContent
{
	/// <summary>The ACL Splunk embeds in the content (<c>eai:acl</c>); usually also available as <see cref="SplunkEntry{T}.Acl"/>.</summary>
	[JsonPropertyName("eai:acl")]
	public SplunkAcl? EaiAcl { get; init; }

	/// <summary>The app context the object was read from (<c>eai:appName</c>).</summary>
	[JsonPropertyName("eai:appName")]
	public string? EaiAppName { get; init; }

	/// <summary>The user context the object was read from (<c>eai:userName</c>).</summary>
	[JsonPropertyName("eai:userName")]
	public string? EaiUserName { get; init; }

	/// <summary>Whether the object is disabled, where the endpoint reports it.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>Every property Splunk returned that the derived type does not model, keyed by its wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
