using Refit;

namespace Splunk.Api.Models.Introspection;

/// <summary>Options for <c>GET server/status/installed-file-integrity</c>.</summary>
public sealed class FileIntegrityOptions
{
	/// <summary>Whether to run the check again rather than return the last result (<c>refresh</c>).</summary>
	[AliasAs("refresh")]
	public bool? Refresh { get; init; }

	/// <summary>Limits the result to files whose paths match this regular expression (<c>regex_filter</c>).</summary>
	[AliasAs("regex_filter")]
	public string? RegexFilter { get; init; }
}
