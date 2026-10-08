using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A revision of a view (<c>data/ui/views/{name}/history</c> and <c>.../revision</c>).</summary>
/// <remarks>History entries are named by position (<c>0000000</c> is the latest); a revision read by id is named <c>revision</c>.</remarks>
public sealed class ViewRevision : SplunkContent
{
	/// <summary>The revision id, a SHA-1 hash; pass it to <c>GetRevisionAsync</c>.</summary>
	[JsonPropertyName("sha")]
	public string? Sha { get; init; }

	/// <summary>The change message given with <c>eai:changelog</c>, or empty.</summary>
	[JsonPropertyName("message")]
	public string? Message { get; init; }

	/// <summary>The user who made the change.</summary>
	[JsonPropertyName("user")]
	public string? User { get; init; }

	/// <summary>The e-mail recorded for the change (Splunk records the user name).</summary>
	[JsonPropertyName("email")]
	public string? Email { get; init; }

	/// <summary>When the change was made.</summary>
	[JsonPropertyName("time")]
	public DateTimeOffset? Time { get; init; }

	/// <summary>The view source at this revision (<c>eai:data</c>); returned only when a revision is read by id.</summary>
	[JsonPropertyName("eai:data")]
	public string? Data { get; init; }
}
