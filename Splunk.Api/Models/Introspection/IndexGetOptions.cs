using Refit;

namespace Splunk.Api.Models.Introspection;

/// <summary>Options for <c>GET data/indexes/{name}</c>.</summary>
public sealed class IndexGetOptions
{
	/// <summary>When <see langword="true"/>, leaves out the detailed properties for a faster answer (<c>summarize</c>).</summary>
	[AliasAs("summarize")]
	public bool? Summarize { get; init; }
}
