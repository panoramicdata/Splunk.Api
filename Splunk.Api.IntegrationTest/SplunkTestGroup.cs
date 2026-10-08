namespace Splunk.Api.IntegrationTest;

/// <summary>Shares one <see cref="SplunkFixture"/> (and so one Splunk session) across every integration test class.</summary>
[CollectionDefinition(Name)]
public sealed class SplunkTestGroup : ICollectionFixture<SplunkFixture>
{
	public const string Name = "Splunk";
}
