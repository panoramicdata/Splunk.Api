using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Configuration;

/// <summary>Creates an empty <c>.conf</c> file (<c>POST properties</c>).</summary>
public sealed class PropertiesFileCreateRequest : SplunkFormRequest
{
	/// <summary>The file name without <c>.conf</c> (<c>__conf</c>).</summary>
	[JsonPropertyName("__conf")]
	public required string FileName { get; init; }
}
