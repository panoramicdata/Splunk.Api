using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>Enters or leaves KV store maintenance mode (<c>POST kvstore/control/maintenance</c>).</summary>
public sealed class KvStoreMaintenanceRequest : SplunkFormRequest
{
	/// <summary><see langword="true"/> to enter maintenance mode, <see langword="false"/> to leave it (<c>mode</c>).</summary>
	[JsonPropertyName("mode")]
	public required bool Mode { get; init; }
}
