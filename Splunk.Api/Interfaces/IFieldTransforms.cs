using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Field transformations, the <c>transforms.conf</c> stanzas used by <c>REPORT-</c> extractions (<c>data/transforms/extractions</c>).</summary>
public interface IFieldTransforms
{
	/// <summary>Lists field transformations (<c>GET data/transforms/extractions</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per transformation.</returns>
	[Get("services/data/transforms/extractions")]
	Task<SplunkFeed<FieldTransform>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a field transformation (<c>POST data/transforms/extractions</c>).</summary>
	/// <param name="request">The name and settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new transformation.</returns>
	/// <remarks>The reference marks <c>SOURCE_KEY</c> as required; Splunk 10.6 defaults it to <c>_raw</c> when it is left out.</remarks>
	[Post("services/data/transforms/extractions")]
	Task<SplunkFeed<FieldTransform>> CreateAsync([Body] FieldTransformCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one field transformation (<c>GET data/transforms/extractions/{name}</c>).</summary>
	/// <param name="name">The stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/transforms/extractions/{name}")]
	Task<SplunkFeed<FieldTransform>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Replaces a field transformation's settings (<c>POST data/transforms/extractions/{name}</c>).</summary>
	/// <param name="name">The stanza name.</param>
	/// <param name="request">The complete new settings: settings left out return to their defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated transformation.</returns>
	[Post("services/data/transforms/extractions/{name}")]
	Task<SplunkFeed<FieldTransform>> UpdateAsync(string name, [Body] FieldTransformUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a field transformation (<c>DELETE data/transforms/extractions/{name}</c>).</summary>
	/// <param name="name">The stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the transformation is deleted.</returns>
	[Delete("services/data/transforms/extractions/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
