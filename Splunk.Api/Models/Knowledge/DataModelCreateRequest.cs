using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a data model (<c>POST datamodel/model</c>).</summary>
public sealed class DataModelCreateRequest : SplunkFormRequest
{
	/// <summary>The data model name (its id, used in URLs and <c>| datamodel</c> searches).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The data model definition as JSON (<c>description</c>): <c>modelName</c>, <c>displayName</c>, <c>description</c>,
	/// <c>objects</c> (each with <c>objectName</c>, <c>parentName</c>, <c>fields</c>, <c>constraints</c>...) and
	/// <c>objectNameList</c>.
	/// </summary>
	[JsonPropertyName("description")]
	public required string Description { get; init; }

	/// <summary>
	/// The acceleration settings as JSON (<c>acceleration</c>), for example
	/// <c>{"enabled":true,"earliest_time":"-1mon","cron_schedule":"0 */12 * * *"}</c>.
	/// </summary>
	[JsonPropertyName("acceleration")]
	public string? Acceleration { get; init; }
}
