using Refit;

namespace Splunk.Api.Interfaces;

/// <summary>The metrics processor (<c>admin/metrics-reload</c>).</summary>
public interface IMetricsProcessor
{
	/// <summary>
	/// Reloads the metrics processor after a metrics-related configuration change (<c>POST admin/metrics-reload/_reload</c>).
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the processor has reloaded.</returns>
	[Post("services/admin/metrics-reload/_reload")]
	Task ReloadAsync(CancellationToken cancellationToken);
}
