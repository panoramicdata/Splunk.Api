using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>The datasets SPL2 can read (<c>orchestrator/v1/datasets</c>).</summary>
	public ISpl2Datasets Spl2Datasets => field ??= For<ISpl2Datasets>();

	/// <summary>SPL to SPL2 conversion (<c>orchestrator/v1/spl2/convert</c>, <c>orchestrator/v2/spl2/convert</c>).</summary>
	public ISpl2Conversion Spl2Conversion => field ??= For<ISpl2Conversion>();

	/// <summary>SPL2 modules, their permissions and dispatch (<c>orchestrator/v1/spl2/modules</c>).</summary>
	public ISpl2Modules Spl2Modules => field ??= For<ISpl2Modules>();
}
