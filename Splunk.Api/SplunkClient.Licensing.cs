using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Installed licenses (<c>licenser/licenses</c>).</summary>
	public ILicenses Licenses => field ??= For<ILicenses>();

	/// <summary>License groups (<c>licenser/groups</c>).</summary>
	public ILicenseGroups LicenseGroups => field ??= For<ILicenseGroups>();

	/// <summary>License stacks (<c>licenser/stacks</c>).</summary>
	public ILicenseStacks LicenseStacks => field ??= For<ILicenseStacks>();

	/// <summary>License pools (<c>licenser/pools</c>).</summary>
	public ILicensePools LicensePools => field ??= For<ILicensePools>();

	/// <summary>License peers registered with this license manager (<c>licenser/peers</c>).</summary>
	public ILicensePeers LicensePeers => field ??= For<ILicensePeers>();

	/// <summary>This instance's license state as a peer (<c>licenser/localpeer</c>).</summary>
	public ILicenseLocalPeer LicenseLocalPeer => field ??= For<ILicenseLocalPeer>();

	/// <summary>Licenser messages (<c>licenser/messages</c>).</summary>
	public ILicenseMessages LicenseMessages => field ??= For<ILicenseMessages>();

	/// <summary>License usage (<c>licenser/usage</c>).</summary>
	public ILicenseUsage LicenseUsage => field ??= For<ILicenseUsage>();
}
