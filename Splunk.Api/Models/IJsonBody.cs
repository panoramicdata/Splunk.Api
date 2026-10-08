namespace Splunk.Api.Models;

/// <summary>A request body sent as JSON rather than form fields. Implemented by <see cref="JsonBody{T}"/>.</summary>
public interface IJsonBody
{
	/// <summary>The value to serialize.</summary>
	object? Value { get; }

	/// <summary>The declared type of <see cref="Value"/>, used to serialize it.</summary>
	Type ValueType { get; }
}
