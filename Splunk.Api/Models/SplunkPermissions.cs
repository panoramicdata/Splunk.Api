using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>The roles that can read and write a Splunk object. <c>*</c> means every role.</summary>
public sealed class SplunkPermissions
{
	/// <summary>Roles with read access.</summary>
	[JsonPropertyName("read")]
	public IReadOnlyList<string> Read { get; init; } = [];

	/// <summary>Roles with write access.</summary>
	[JsonPropertyName("write")]
	public IReadOnlyList<string> Write { get; init; } = [];
}
