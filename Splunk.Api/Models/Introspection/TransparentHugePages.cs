using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The Linux transparent huge pages settings (<c>server/sysinfo</c>, <c>transparent_hugepages</c>).</summary>
public sealed class TransparentHugePages
{
	/// <summary>The enabled setting, for example <c>never</c> or <c>madvise</c> (<c>enabled</c>).</summary>
	[JsonPropertyName("enabled")]
	public string? Enabled { get; init; }

	/// <summary>The defrag setting (<c>defrag</c>).</summary>
	[JsonPropertyName("defrag")]
	public string? Defrag { get; init; }

	/// <summary>Splunk's verdict on the settings, for example <c>ok</c> (<c>effective_state</c>).</summary>
	[JsonPropertyName("effective_state")]
	public string? EffectiveState { get; init; }
}
