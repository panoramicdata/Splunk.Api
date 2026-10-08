using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>
/// KV store server or replica set status (<c>server/introspection/kvstore/serverstatus</c> and <c>replicasetstats</c>):
/// a JSON document Splunk returns as a string.
/// </summary>
public sealed class KvStoreIntrospectionData : SplunkContent
{
	/// <summary>The raw JSON document (<c>data</c>), for example <c>{"database":"kvservice","status":"healthy"}</c>.</summary>
	[JsonPropertyName("data")]
	public string? Data { get; init; }

	/// <summary>Parses <see cref="Data"/>.</summary>
	/// <returns>The document, or <see langword="null"/> when there is none.</returns>
	public JsonElement? ParseData()
	{
		if (string.IsNullOrEmpty(Data))
		{
			return null;
		}

		using var document = JsonDocument.Parse(Data);
		return document.RootElement.Clone();
	}
}
