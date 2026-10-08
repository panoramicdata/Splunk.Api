using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Changes a SAML configuration (<c>POST authentication/providers/SAML/{stanza_name}</c>).</summary>
public sealed class SamlConfigurationUpdateRequest : SamlSettings
{
	/// <summary>The entity ID preconfigured by the identity provider.</summary>
	[JsonPropertyName("entityId")]
	public string? EntityId { get; init; }
}
