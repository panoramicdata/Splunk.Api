namespace Splunk.Api.IntegrationTest;

/// <summary>Shares one <see cref="SplunkHecFixture"/> across the collector integration tests.</summary>
[CollectionDefinition(Name)]
public sealed class SplunkHecTestGroup : ICollectionFixture<SplunkHecFixture>
{
	public const string Name = "SplunkHec";
}
