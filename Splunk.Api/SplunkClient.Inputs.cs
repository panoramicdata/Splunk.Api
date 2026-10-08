using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Ingest actions S3 destinations (<c>data/ingest/rfsdestinations</c>).</summary>
	public IIngestDestinations IngestDestinations => field ??= For<IIngestDestinations>();

	/// <summary>Ingest actions rulesets (<c>data/ingest/rulesets</c>).</summary>
	public IIngestRulesets IngestRulesets => field ??= For<IIngestRulesets>();

	/// <summary>Active Directory monitoring inputs, Windows only (<c>data/inputs/ad</c>).</summary>
	public IActiveDirectoryInputs ActiveDirectoryInputs => field ??= For<IActiveDirectoryInputs>();

	/// <summary>Every input of every kind (<c>data/inputs/all</c>).</summary>
	public IAllInputs AllInputs => field ??= For<IAllInputs>();

	/// <summary>HTTP Event Collector tokens and global settings (<c>data/inputs/http</c>).</summary>
	public IHecTokens HecTokens => field ??= For<IHecTokens>();

	/// <summary>Recent HTTP Event Collector senders (<c>data/inputs/http/connections</c>).</summary>
	public IHecConnections HecConnections => field ??= For<IHecConnections>();

	/// <summary>File and directory monitor inputs (<c>data/inputs/monitor</c>).</summary>
	public IMonitorInputs MonitorInputs => field ??= For<IMonitorInputs>();

	/// <summary>Files indexed once (<c>data/inputs/oneshot</c>).</summary>
	public IOneshotInputs OneshotInputs => field ??= For<IOneshotInputs>();

	/// <summary>Windows registry monitoring inputs (<c>data/inputs/registry</c>).</summary>
	public IRegistryInputs RegistryInputs => field ??= For<IRegistryInputs>();

	/// <summary>Scripted inputs (<c>data/inputs/script</c>).</summary>
	public IScriptedInputs ScriptedInputs => field ??= For<IScriptedInputs>();

	/// <summary>Cooked TCP inputs, the ports forwarders send to (<c>data/inputs/tcp/cooked</c>).</summary>
	public ICookedTcpInputs CookedTcpInputs => field ??= For<ICookedTcpInputs>();

	/// <summary>Raw TCP inputs (<c>data/inputs/tcp/raw</c>).</summary>
	public IRawTcpInputs RawTcpInputs => field ??= For<IRawTcpInputs>();

	/// <summary>Tokens forwarders present to receiving ports (<c>data/inputs/tcp/splunktcptoken</c>).</summary>
	public ISplunkTcpTokens SplunkTcpTokens => field ??= For<ISplunkTcpTokens>();

	/// <summary>The SSL settings of TCP inputs (<c>data/inputs/tcp/ssl</c>).</summary>
	public ITcpSslSettings TcpSslSettings => field ??= For<ITcpSslSettings>();

	/// <summary>UDP inputs (<c>data/inputs/udp</c>).</summary>
	public IUdpInputs UdpInputs => field ??= For<IUdpInputs>();

	/// <summary>Windows event log collections (<c>data/inputs/win-event-log-collections</c>).</summary>
	public IWindowsEventLogInputs WindowsEventLogInputs => field ??= For<IWindowsEventLogInputs>();

	/// <summary>WMI collections (<c>data/inputs/win-wmi-collections</c>).</summary>
	public IWmiInputs WmiInputs => field ??= For<IWmiInputs>();

	/// <summary>Windows Performance Monitor inputs (<c>data/inputs/win-perfmon</c>).</summary>
	public IPerfmonInputs PerfmonInputs => field ??= For<IPerfmonInputs>();

	/// <summary>Modular input kinds (<c>data/modular-inputs</c>).</summary>
	public IModularInputs ModularInputs => field ??= For<IModularInputs>();

	/// <summary>Data preview jobs (<c>indexing/preview</c>).</summary>
	public IIndexingPreviews IndexingPreviews => field ??= For<IIndexingPreviews>();

	/// <summary>Sending events through the management port (<c>receivers/simple</c>, <c>receivers/stream</c>).</summary>
	public IReceivers Receivers => field ??= For<IReceivers>();

	/// <summary>Ingestion pipeline sets (<c>server/pipelinesets</c>).</summary>
	public IPipelineSets PipelineSets => field ??= For<IPipelineSets>();
}
