using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Spl2;

namespace Splunk.Api.Interfaces;

/// <summary>SPL to SPL2 conversion (<c>orchestrator/v1/spl2/convert</c> and <c>orchestrator/v2/spl2/convert</c>).</summary>
/// <remarks>
/// Conversion runs in Splunk's SPL2 language server. Where that service is not running (as in the
/// <c>splunk/splunk</c> 10.6 Docker image), Splunk answers 500 with <c>Failed to open resource handle: uds:///.../lsp-N.sock</c>.
/// </remarks>
public interface ISpl2Conversion
{
	/// <summary>Converts an SPL search to SPL2 (<c>POST orchestrator/v1/spl2/convert</c>).</summary>
	/// <param name="request">The SPL search and runtime.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The SPL2 search and any notes. A search that cannot be converted raises <see cref="SplunkApiException"/> (400).</returns>
	[Post("services/orchestrator/v1/spl2/convert")]
	Task<Spl2ConversionResult> ConvertAsync([Body] JsonBody<Spl2ConversionRequest> request, CancellationToken cancellationToken);

	/// <summary>Converts an SPL search to SPL2, reporting warnings and errors (<c>POST orchestrator/v2/spl2/convert</c>).</summary>
	/// <param name="request">The SPL search and runtime.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The SPL2 search and any warning. A search that cannot be converted raises <see cref="SplunkApiException"/> (400).</returns>
	[Post("services/orchestrator/v2/spl2/convert")]
	Task<Spl2ConversionResultV2> ConvertV2Async([Body] JsonBody<Spl2ConversionRequest> request, CancellationToken cancellationToken);
}
