using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>The type of a KV store collection field (<c>field.&lt;name&gt;</c>), enforced when the collection has <c>enforceTypes</c>.</summary>
public enum KvStoreFieldType
{
	/// <summary>Not recognised.</summary>
	Unknown = 0,

	/// <summary>A JSON array (<c>array</c>).</summary>
	[JsonStringEnumMemberName("array")]
	Array,

	/// <summary>A number, stored as a double (<c>number</c>).</summary>
	[JsonStringEnumMemberName("number")]
	Number,

	/// <summary>A boolean (<c>bool</c>).</summary>
	[JsonStringEnumMemberName("bool")]
	Bool,

	/// <summary>A string (<c>string</c>).</summary>
	[JsonStringEnumMemberName("string")]
	[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Named after Splunk's field type.")]
	String,

	/// <summary>An IP address range, stored in canonical form (<c>cidr</c>).</summary>
	[JsonStringEnumMemberName("cidr")]
	Cidr,

	/// <summary>A time (<c>time</c>).</summary>
	[JsonStringEnumMemberName("time")]
	Time
}
