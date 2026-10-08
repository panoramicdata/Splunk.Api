using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Configuration file stanzas (<c>configs/conf-{file}</c>).</summary>
	public IConfigs Configs => field ??= For<IConfigs>();

	/// <summary>Configuration files, stanzas and keys (<c>properties</c>).</summary>
	public IConfigProperties ConfigProperties => field ??= For<IConfigProperties>();
}
