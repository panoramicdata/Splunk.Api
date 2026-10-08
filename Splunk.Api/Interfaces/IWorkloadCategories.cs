using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.WorkloadManagement;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Workload categories, <c>search</c>, <c>ingest</c> and <c>misc</c> (<c>workloads/categories</c>). Workload management
/// needs Linux; reading needs <c>list_workload_pools</c>, changing <c>edit_workload_pools</c>. Not on Splunk Cloud.
/// </summary>
public interface IWorkloadCategories
{
	/// <summary>Lists the categories and their resource allocation (<c>GET workloads/categories</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per category.</returns>
	[Get("services/workloads/categories")]
	Task<SplunkFeed<WorkloadCategory>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Changes a category's CPU and memory weights (<c>POST workloads/categories/{name}</c>).</summary>
	/// <param name="name">The category: <c>search</c>, <c>ingest</c> or <c>misc</c>.</param>
	/// <param name="request">The weights.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated category.</returns>
	/// <remarks>The reference lists this as <c>POST workloads/categories</c>; the category is addressed by name in the path.</remarks>
	[Post("services/workloads/categories/{name}")]
	Task<SplunkFeed<WorkloadCategory>> UpdateAsync(string name, [Body] WorkloadWeightsRequest request, CancellationToken cancellationToken);
}
