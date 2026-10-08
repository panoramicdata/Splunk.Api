using Refit;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Options for listing lookup definitions (<c>GET data/transforms/lookups</c>).</summary>
/// <remarks>
/// The reference also documents <c>replicate_delta</c> here, but Splunk 10.6 refuses it ("Argument replicate_delta is not
/// supported by this handler"), so it is not offered.
/// </remarks>
public sealed class LookupDefinitionListOptions : ListOptions
{
	/// <summary>When <see langword="true"/>, each file lookup reports its file's size (<c>getsize</c>).</summary>
	[AliasAs("getsize")]
	public bool? GetSize { get; init; }
}
