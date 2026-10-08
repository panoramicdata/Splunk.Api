using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>A custom (Python) search command (<c>data/commands</c>), as configured in commands.conf.</summary>
public sealed class SearchCommand : SplunkContent
{
	/// <summary>The script that implements the command.</summary>
	[JsonPropertyName("filename")]
	public string? Filename { get; init; }

	/// <summary>The implementation type, for example <c>python</c>.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>Whether the command is streaming.</summary>
	[JsonPropertyName("streaming")]
	public bool Streaming { get; init; }

	/// <summary>Whether the command generates events.</summary>
	[JsonPropertyName("generating")]
	public bool Generating { get; init; }

	/// <summary>Whether generated events are in time order.</summary>
	[JsonPropertyName("generates_timeorder")]
	public bool GeneratesTimeOrder { get; init; }

	/// <summary>Whether the command keeps the events it is given.</summary>
	[JsonPropertyName("retainsevents")]
	public bool RetainsEvents { get; init; }

	/// <summary>Whether the command reorders columns.</summary>
	[JsonPropertyName("changes_colorder")]
	public bool ChangesColumnOrder { get; init; }

	/// <summary>The fields the command needs, comma-separated; <c>*</c> for all.</summary>
	[JsonPropertyName("required_fields")]
	public string? RequiredFields { get; init; }

	/// <summary>The most events passed to the command.</summary>
	[JsonPropertyName("maxinputs")]
	public long MaxInputs { get; init; }

	/// <summary>Whether the command receives the caller's session key.</summary>
	[JsonPropertyName("passauth")]
	public bool PassAuth { get; init; }

	/// <summary>Whether Splunk flags the command as risky (warning users before it runs).</summary>
	[JsonPropertyName("is_risky")]
	public bool IsRisky { get; init; }

	/// <summary>Whether the command receives raw arguments.</summary>
	[JsonPropertyName("supports_rawargs")]
	public bool SupportsRawArgs { get; init; }

	/// <summary>The Python version required, for example <c>latest</c>.</summary>
	[JsonPropertyName("python.required")]
	public string? PythonRequired { get; init; }
}
