using Refit;
using Splunk.Api.Models.Inputs;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Splunk.Api.Serialization;

/// <summary>
/// Refit content serializer for the HTTP Event Collector: a sequence of <see cref="HecEvent"/> is written as the
/// collector's batch format (JSON objects one after another, not an array); any other body as JSON. Responses are read
/// as JSON with <see cref="SplunkJson.Options"/>.
/// </summary>
internal sealed class HecContentSerializer : IHttpContentSerializer
{
	private readonly SystemTextJsonContentSerializer _json = new(SplunkJson.Options);

	/// <inheritdoc />
	public HttpContent ToHttpContent<T>(T item)
	{
		ArgumentNullException.ThrowIfNull(item);
		var text = item is IEnumerable<HecEvent> events
			? string.Join('\n', events.Select(e => JsonSerializer.Serialize(e, SplunkJson.Options)))
			: JsonSerializer.Serialize(item, SplunkJson.Options);
		return new StringContent(text, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
	}

	/// <inheritdoc />
	public Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
		=> _json.FromHttpContentAsync<T>(content, cancellationToken);

	/// <inheritdoc />
	public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
		=> _json.GetFieldNameForProperty(propertyInfo);
}
