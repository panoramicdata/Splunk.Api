namespace Splunk.Api.Models;

/// <summary>
/// The base of every request body sent as <c>application/x-www-form-urlencoded</c>, the form Splunk's POST endpoints take.
/// </summary>
/// <remarks>
/// <para>
/// Each public readable property becomes a form field named by its <see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute"/>
/// or, without one, by its name in snake_case. <see langword="null"/> properties are left out, so only what is set is
/// sent (send <see cref="string.Empty"/> to clear a value). Booleans are sent as <c>true</c>/<c>false</c>, numbers in the
/// invariant culture, enums by their <see cref="System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute"/> name,
/// and a sequence of values as the same field repeated once per value.
/// </para>
/// <para>
/// Many Splunk endpoints take open-ended parameter families (such as <c>action.&lt;name&gt;.*</c> on saved searches or
/// <c>args.*</c> on search jobs). Put those in <see cref="AdditionalParameters"/>; a name that a typed property also sets
/// is rejected rather than sent twice.
/// </para>
/// </remarks>
public abstract class SplunkFormRequest
{
	/// <summary>
	/// Extra form fields sent as given, after the typed properties. A <see langword="null"/> value is sent as an empty
	/// string.
	/// </summary>
	public IDictionary<string, string?> AdditionalParameters { get; init; } = new Dictionary<string, string?>();
}
