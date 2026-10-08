using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Workload categories (<c>workloads/categories</c>).</summary>
	public IWorkloadCategories WorkloadCategories => field ??= For<IWorkloadCategories>();

	/// <summary>Workload pools (<c>workloads/pools</c>).</summary>
	public IWorkloadPools WorkloadPools => field ??= For<IWorkloadPools>();

	/// <summary>Workload and admission rules (<c>workloads/rules</c>).</summary>
	public IWorkloadRules WorkloadRules => field ??= For<IWorkloadRules>();

	/// <summary>Workload management on/off and cgroup setup (<c>workloads/config</c>).</summary>
	public IWorkloadConfig WorkloadConfig => field ??= For<IWorkloadConfig>();

	/// <summary>Workload management policies (<c>workloads/policy</c>).</summary>
	public IWorkloadPolicy WorkloadPolicy => field ??= For<IWorkloadPolicy>();

	/// <summary>Workload management status (<c>workloads/status</c>).</summary>
	public IWorkloadStatus WorkloadStatus => field ??= For<IWorkloadStatus>();
}
