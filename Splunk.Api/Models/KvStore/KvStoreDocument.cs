using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>
/// An optional base for KV store document types: it maps the <c>_key</c> and <c>_user</c> properties every stored document
/// has. Any type that serializes to a JSON object can be used as a document; this one only saves declaring the two.
/// </summary>
public abstract class KvStoreDocument
{
	/// <summary>The document's key (<c>_key</c>). Leave <see langword="null"/> on insert to let Splunk generate one.</summary>
	[JsonPropertyName("_key")]
	public string? Key { get; set; }

	/// <summary>The user the document belongs to (<c>_user</c>); <c>nobody</c> for shared documents.</summary>
	[JsonPropertyName("_user")]
	public string? User { get; set; }
}
