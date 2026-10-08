using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>
/// Changes the sharing and permissions of a knowledge object or configuration entity (<c>POST {endpoint}/{name}/acl</c>).
/// </summary>
public sealed class AclUpdateRequest : SplunkFormRequest
{
	/// <summary>The sharing level: <c>user</c>, <c>app</c> or <c>global</c>. Required by Splunk.</summary>
	[JsonPropertyName("sharing")]
	public required string Sharing { get; init; }

	/// <summary>The new owner, a user name or <c>nobody</c>. Required by Splunk.</summary>
	[JsonPropertyName("owner")]
	public required string Owner { get; init; }

	/// <summary>Roles granted read access; <c>*</c> for every role.</summary>
	[JsonPropertyName("perms.read")]
	public IReadOnlyList<string>? Read { get; init; }

	/// <summary>Roles granted write access; <c>*</c> for every role.</summary>
	[JsonPropertyName("perms.write")]
	public IReadOnlyList<string>? Write { get; init; }
}
