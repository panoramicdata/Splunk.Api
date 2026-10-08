using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>Indexes with bucket-level size information (<c>data/indexes-extended</c>).</summary>
public interface IIndexesExtended
{
	/// <summary>Lists the indexes with bucket information (<c>GET data/indexes-extended</c>).</summary>
	/// <param name="options">Paging, filtering and the data type, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per index.</returns>
	[Get("services/data/indexes-extended")]
	Task<SplunkFeed<SplunkIndexExtended>> ListAsync([Query] IndexListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one index with bucket information (<c>GET data/indexes-extended/{name}</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the index.</returns>
	[Get("services/data/indexes-extended/{name}")]
	Task<SplunkFeed<SplunkIndexExtended>> GetAsync(string name, CancellationToken cancellationToken);
}
