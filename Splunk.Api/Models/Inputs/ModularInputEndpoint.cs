using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>The parameters a modular input kind takes (<c>endpoint</c>).</summary>
public sealed class ModularInputEndpoint
{
	/// <summary>The parameters, keyed by name.</summary>
	[JsonPropertyName("args")]
	public IReadOnlyDictionary<string, ModularInputArgument> Arguments { get; init; } = new Dictionary<string, ModularInputArgument>();
}
