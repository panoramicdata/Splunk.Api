using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>Index volumes, the logical drives indexes are stored on (<c>data/index-volumes</c>).</summary>
public interface IIndexVolumes
{
	/// <summary>Lists the volumes (<c>GET data/index-volumes</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per volume.</returns>
	[Get("services/data/index-volumes")]
	Task<SplunkFeed<IndexVolume>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one volume (<c>GET data/index-volumes/{name}</c>).</summary>
	/// <param name="name">The volume name, for example <c>_splunk_summaries</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the volume.</returns>
	[Get("services/data/index-volumes/{name}")]
	Task<SplunkFeed<IndexVolume>> GetAsync(string name, CancellationToken cancellationToken);
}
