using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>A licenser message, alert or persisted warning (<c>licenser/messages</c>); the entry name is the message ID.</summary>
public sealed class LicenseMessage : SplunkContent
{
	/// <summary>
	/// The category: <c>license_window</c>, <c>pool_over_quota</c>, <c>stack_over_quota</c>, <c>orphan_peer</c>,
	/// <c>pool_warning_count</c> or <c>pool_violated_peer_count</c>.
	/// </summary>
	[JsonPropertyName("category")]
	public string? Category { get; init; }

	/// <summary>The severity: <c>INFO</c>, <c>WARN</c> or <c>ERROR</c>.</summary>
	[JsonPropertyName("severity")]
	public string? Severity { get; init; }

	/// <summary>The message text.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>When the message was created.</summary>
	[JsonPropertyName("create_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? CreateTime { get; init; }

	/// <summary>The license pool the message is about, if any.</summary>
	[JsonPropertyName("pool_id")]
	public string? PoolId { get; init; }

	/// <summary>The license stack the message is about, if any.</summary>
	[JsonPropertyName("stack_id")]
	public string? StackId { get; init; }

	/// <summary>The license peer the message is about, if any.</summary>
	[JsonPropertyName("peer_id")]
	public string? PeerId { get; init; }
}
