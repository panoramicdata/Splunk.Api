using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The result of the installed file integrity check (<c>server/status/installed-file-integrity</c>).</summary>
public sealed class FileIntegrityStatus : SplunkContent
{
	/// <summary>Whether a check result is available (<c>check_ready</c>).</summary>
	[JsonPropertyName("check_ready")]
	public bool CheckReady { get; init; }

	/// <summary>The files that differ from the installation manifest; <see langword="null"/> when none do (<c>check_failures</c>).</summary>
	[JsonPropertyName("check_failures")]
	public JsonElement? CheckFailures { get; init; }
}
