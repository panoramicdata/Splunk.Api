using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Data models and the pivots over them (<c>datamodel/model</c>, <c>datamodel/pivot</c>).</summary>
public interface IDataModels
{
	/// <summary>Lists data models (<c>GET datamodel/model</c>).</summary>
	/// <param name="options">Paging and filtering, and <c>concise</c>; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per data model. Listings carry a summary of each definition.</returns>
	[Get("services/datamodel/model")]
	Task<SplunkFeed<DataModel>> ListAsync([Query] DataModelListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a data model (<c>POST datamodel/model</c>).</summary>
	/// <param name="request">The name, JSON definition and optional acceleration settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new data model.</returns>
	[Post("services/datamodel/model")]
	Task<SplunkFeed<DataModel>> CreateAsync([Body] DataModelCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one data model (<c>GET datamodel/model/{name}</c>).</summary>
	/// <param name="name">The data model name.</param>
	/// <param name="options"><c>concise</c>; <see langword="null"/> for Splunk's default.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/datamodel/model/{name}")]
	Task<SplunkFeed<DataModel>> GetAsync(string name, [Query] DataModelGetOptions? options, CancellationToken cancellationToken);

	/// <summary>Changes a data model's definition or acceleration, or validates a definition (<c>POST datamodel/model/{name}</c>).</summary>
	/// <param name="name">The data model name.</param>
	/// <param name="request">The new definition, acceleration settings and <c>provisional</c> flag.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated (or, when provisional, validated) data model.</returns>
	[Post("services/datamodel/model/{name}")]
	Task<SplunkFeed<DataModel>> UpdateAsync(string name, [Body] DataModelUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a data model (<c>DELETE datamodel/model/{name}</c>).</summary>
	/// <param name="name">The data model name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the data model is deleted.</returns>
	[Delete("services/datamodel/model/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Translates a pivot over a data model into searches (<c>GET datamodel/pivot/{name}</c>).</summary>
	/// <param name="name">The data model name.</param>
	/// <param name="options">The pivot, as a <c>| pivot</c> search or as JSON.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry carrying the pivot's searches.</returns>
	/// <remarks>The reference's heading reads <c>datamodel/pivot</c>, but the endpoint takes the data model name in the path.</remarks>
	[Get("services/datamodel/pivot/{name}")]
	Task<SplunkFeed<Pivot>> GetPivotAsync(string name, [Query] PivotOptions options, CancellationToken cancellationToken);
}
