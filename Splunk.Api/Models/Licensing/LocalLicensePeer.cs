using Splunk.Api.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Licensing;

/// <summary>
/// This instance's license state as a license peer (<c>licenser/localpeer</c>). The deprecated <c>master_*</c> and
/// <c>slave_*</c> duplicates are in <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class LocalLicensePeer : SplunkContent
{
	/// <summary>This instance's GUID.</summary>
	[JsonPropertyName("peer_id")]
	public string? PeerId { get; init; }

	/// <summary>This instance's server name.</summary>
	[JsonPropertyName("peer_label")]
	public string? PeerLabel { get; init; }

	/// <summary>The license manager's URI, or <c>self</c> when this instance is its own manager.</summary>
	[JsonPropertyName("manager_uri")]
	public string? ManagerUri { get; init; }

	/// <summary>The license manager's GUID.</summary>
	[JsonPropertyName("manager_guid")]
	public string? ManagerGuid { get; init; }

	/// <summary>The GUIDs of the licenses in use.</summary>
	[JsonPropertyName("guid")]
	public IReadOnlyList<string> LicenseGuids { get; init; } = [];

	/// <summary>The hashes of the licenses in use.</summary>
	[JsonPropertyName("license_keys")]
	public IReadOnlyList<string> LicenseKeys { get; init; } = [];

	/// <summary>Each licensed feature and its state, for example <c>ENABLED</c> or <c>DISABLED_DUE_TO_LICENSE</c>.</summary>
	[JsonPropertyName("features")]
	public IReadOnlyDictionary<string, string> Features { get; init; } = new Dictionary<string, string>();

	/// <summary>The add-ons on this instance, keyed by name, with their type and parameters.</summary>
	[JsonPropertyName("add_ons")]
	public IReadOnlyDictionary<string, JsonElement>? AddOns { get; init; }

	/// <summary>When this instance last tried to contact the manager.</summary>
	[JsonPropertyName("last_manager_contact_attempt_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastManagerContactAttemptTime { get; init; }

	/// <summary>When this instance last contacted the manager successfully.</summary>
	[JsonPropertyName("last_manager_contact_success_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastManagerContactSuccessTime { get; init; }

	/// <summary>When license usage tracking was last serviced; <see langword="null"/> if never.</summary>
	[JsonPropertyName("last_trackerdb_service_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastTrackerDbServiceTime { get; init; }

	/// <summary>The connection timeout to the manager, in seconds.</summary>
	[JsonPropertyName("connection_timeout")]
	public int ConnectionTimeout { get; init; }

	/// <summary>The receive timeout from the manager, in seconds.</summary>
	[JsonPropertyName("receive_timeout")]
	public int ReceiveTimeout { get; init; }

	/// <summary>The send timeout to the manager, in seconds.</summary>
	[JsonPropertyName("send_timeout")]
	public int SendTimeout { get; init; }

	/// <summary>The number of usage rows above which source and host values are squashed before reporting.</summary>
	[JsonPropertyName("squash_threshold")]
	public int SquashThreshold { get; init; }
}
