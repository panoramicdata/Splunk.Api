using Refit;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Splunk.Api.Serialization;

/// <summary>
/// The base of the Refit content serializers: responses are read as JSON with <see cref="SplunkJson.Options"/>; each
/// derived serializer decides how request bodies are written.
/// </summary>
internal abstract class SplunkJsonResponseSerializer : IHttpContentSerializer
{
	private readonly SystemTextJsonContentSerializer _json = new(SplunkJson.Options);

	/// <inheritdoc />
	public abstract HttpContent ToHttpContent<T>(T item);

	/// <inheritdoc />
	public Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
		=> _json.FromHttpContentAsync<T>(content, cancellationToken);

	/// <inheritdoc />
	public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
		=> _json.GetFieldNameForProperty(propertyInfo);

	/// <summary>Wraps JSON text as UTF-8 <c>application/json</c> content.</summary>
	protected static StringContent JsonContent(string text)
		=> new(text, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
}
