using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates a SAML configuration (<c>POST authentication/providers/SAML</c>).</summary>
/// <remarks>
/// The reference also marks <see cref="SamlSettings.IdpSsoUrl"/> required; a live Splunk 10.6 requires only the name
/// and entity ID (the URL can come from <see cref="SamlSettings.IdpMetadataFile"/>).
/// </remarks>
public sealed class SamlConfigurationCreateRequest : SamlSettings
{
	/// <summary>The configuration stanza name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The entity ID preconfigured by the identity provider.</summary>
	[JsonPropertyName("entityId")]
	public required string EntityId { get; init; }
}
