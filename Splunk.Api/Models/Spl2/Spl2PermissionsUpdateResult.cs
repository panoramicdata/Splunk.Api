using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>The reply to replacing an SPL2 module's permissions: <c>{"code":201}</c>.</summary>
public sealed class Spl2PermissionsUpdateResult
{
	/// <summary>The status code Splunk reports, 201 on success.</summary>
	[JsonPropertyName("code")]
	public int Code { get; init; }
}
