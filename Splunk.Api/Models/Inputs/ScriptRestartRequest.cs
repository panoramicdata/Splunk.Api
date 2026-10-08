using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Restarts a scripted input (<c>POST data/inputs/script/restart</c>).</summary>
public sealed class ScriptRestartRequest : SplunkFormRequest
{
	/// <summary>The path of an existing scripted input's script.</summary>
	[JsonPropertyName("script")]
	public required string Script { get; init; }
}
