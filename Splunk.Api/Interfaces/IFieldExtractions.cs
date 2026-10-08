using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Search-time field extractions, the <c>EXTRACT-</c> and <c>REPORT-</c> attributes in <c>props.conf</c> (<c>data/props/extractions</c>).</summary>
/// <remarks>An entry is named <c>{stanza} : EXTRACT-{name}</c> or <c>{stanza} : REPORT-{name}</c>; pass that name to the single-entry operations.</remarks>
public interface IFieldExtractions
{
	/// <summary>Lists field extractions (<c>GET data/props/extractions</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per extraction.</returns>
	[Get("services/data/props/extractions")]
	Task<SplunkFeed<FieldExtraction>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a field extraction (<c>POST data/props/extractions</c>).</summary>
	/// <param name="request">The name, stanza, type and regular expression or transforms.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new extraction.</returns>
	[Post("services/data/props/extractions")]
	Task<SplunkFeed<FieldExtraction>> CreateAsync([Body] FieldExtractionCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one field extraction (<c>GET data/props/extractions/{name}</c>).</summary>
	/// <param name="name">The entry name, for example <c>access_combined : EXTRACT-port</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/props/extractions/{name}")]
	Task<SplunkFeed<FieldExtraction>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a field extraction's regular expression or transforms (<c>POST data/props/extractions/{name}</c>).</summary>
	/// <param name="name">The entry name.</param>
	/// <param name="request">The new value.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated extraction.</returns>
	[Post("services/data/props/extractions/{name}")]
	Task<SplunkFeed<FieldExtraction>> UpdateAsync(string name, [Body] FieldExtractionUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a field extraction (<c>DELETE data/props/extractions/{name}</c>).</summary>
	/// <param name="name">The entry name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the extraction is deleted.</returns>
	[Delete("services/data/props/extractions/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
