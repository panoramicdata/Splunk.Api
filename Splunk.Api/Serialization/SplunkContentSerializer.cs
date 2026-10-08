using Splunk.Api.Models;
using System.Text.Json;

namespace Splunk.Api.Serialization;

/// <summary>
/// Refit content serializer for Splunk: request bodies are written as <c>application/x-www-form-urlencoded</c> form fields
/// (see <see cref="SplunkFormRequest"/>), or as JSON when wrapped in <see cref="JsonBody{T}"/>; responses are read as JSON
/// with <see cref="SplunkJson.Options"/>.
/// </summary>
internal sealed class SplunkContentSerializer : SplunkJsonResponseSerializer
{
	/// <inheritdoc />
	public override HttpContent ToHttpContent<T>(T item)
	{
		ArgumentNullException.ThrowIfNull(item);
		if (item is IJsonBody json)
		{
			return JsonContent(JsonSerializer.Serialize(json.Value, json.ValueType, SplunkJson.Options));
		}

		// FormUrlEncodedContent is buffered (a ByteArrayContent), so the request can be replayed on retry or re-login.
		return new FormUrlEncodedContent(SplunkFormEncoder.Encode(item));
	}
}
