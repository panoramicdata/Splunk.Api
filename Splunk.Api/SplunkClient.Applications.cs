using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Installed apps (<c>apps/local</c>).</summary>
	public IApps Apps => field ??= For<IApps>();

	/// <summary>App templates (<c>apps/apptemplates</c>).</summary>
	public IAppTemplates AppTemplates => field ??= For<IAppTemplates>();
}
