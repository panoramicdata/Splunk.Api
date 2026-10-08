using Splunk.Api.Models.Inputs;
using System.Text.Json;

namespace Splunk.Api.Serialization;

/// <summary>
/// Refit content serializer for the HTTP Event Collector: a sequence of <see cref="HecEvent"/> is written as the
/// collector's batch format (JSON objects one after another, not an array); any other body as JSON. Responses are read
/// as JSON with <see cref="SplunkJson.Options"/>.
/// </summary>
internal sealed class HecContentSerializer : SplunkJsonResponseSerializer
{
	/// <inheritdoc />
	public override HttpContent ToHttpContent<T>(T item)
	{
		ArgumentNullException.ThrowIfNull(item);
		return JsonContent(item is IEnumerable<HecEvent> events
			? string.Join('\n', events.Select(e => JsonSerializer.Serialize(e, SplunkJson.Options)))
			: JsonSerializer.Serialize(item, SplunkJson.Options));
	}
}
