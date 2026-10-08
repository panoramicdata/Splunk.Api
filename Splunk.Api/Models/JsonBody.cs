namespace Splunk.Api.Models;

/// <summary>
/// Wraps a value to be sent as an <c>application/json</c> request body, for the few endpoints (such as KV store
/// collection data) that take JSON rather than form fields. The value is serialized with <see cref="SplunkJson.Options"/>.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <param name="Value">The value to send.</param>
public sealed record JsonBody<T>(T Value) : IJsonBody
{
	object? IJsonBody.Value => Value;

	Type IJsonBody.ValueType => typeof(T);
}
