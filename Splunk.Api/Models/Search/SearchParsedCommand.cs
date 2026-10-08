using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>One command of a parsed search (<see cref="SearchParseResult.Commands"/>).</summary>
public sealed class SearchParsedCommand
{
	/// <summary>The command name, for example <c>search</c> or <c>stats</c>.</summary>
	[JsonPropertyName("command")]
	public string Command { get; init; } = string.Empty;

	/// <summary>The command's arguments as written.</summary>
	[JsonPropertyName("rawargs")]
	public string? RawArguments { get; init; }

	/// <summary>The pipeline the command runs in, for example <c>streaming</c> or <c>report</c>.</summary>
	[JsonPropertyName("pipeline")]
	public string? Pipeline { get; init; }

	/// <summary>The parsed arguments; their shape depends on the command.</summary>
	[JsonPropertyName("args")]
	public JsonElement Arguments { get; init; }

	/// <summary>Whether the command generates events (rather than processing them).</summary>
	[JsonPropertyName("isGenerating")]
	public bool IsGenerating { get; init; }

	/// <summary>The stream type, for example <c>SP_STREAM</c> or <c>SP_STREAMREPORT</c>.</summary>
	[JsonPropertyName("streamType")]
	public string? StreamType { get; init; }

	/// <summary>Any other properties, such as <c>preStreamingOp</c>.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
