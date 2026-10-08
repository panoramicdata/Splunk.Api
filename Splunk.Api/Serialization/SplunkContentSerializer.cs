using Refit;
using Splunk.Api.Models;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Splunk.Api.Serialization;

/// <summary>
/// Refit content serializer for Splunk: request bodies are written as <c>application/x-www-form-urlencoded</c> form fields
/// (see <see cref="SplunkFormRequest"/>), or as JSON when wrapped in <see cref="JsonBody{T}"/>; responses are read as JSON
/// with <see cref="SplunkJson.Options"/>.
/// </summary>
internal sealed class SplunkContentSerializer : IHttpContentSerializer
{
	private readonly SystemTextJsonContentSerializer _json = new(SplunkJson.Options);

	/// <inheritdoc />
	public HttpContent ToHttpContent<T>(T item)
	{
		ArgumentNullException.ThrowIfNull(item);
		if (item is IJsonBody json)
		{
			var text = JsonSerializer.Serialize(json.Value, json.ValueType, SplunkJson.Options);
			return new StringContent(text, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
		}

		// FormUrlEncodedContent is buffered (a ByteArrayContent), so the request can be replayed on retry or re-login.
		return new FormUrlEncodedContent(SplunkFormEncoder.Encode(item));
	}

	/// <inheritdoc />
	public Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
		=> _json.FromHttpContentAsync<T>(content, cancellationToken);

	/// <inheritdoc />
	public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
		=> _json.GetFieldNameForProperty(propertyInfo);
}
