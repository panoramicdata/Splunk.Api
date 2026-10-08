using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

/// <summary>Pins that each knowledge endpoint group is created once per client and then reused.</summary>
public class KnowledgeClientTests
{
	public static TheoryData<string> GroupNames =>
	[
		nameof(SplunkClient.DataModelSummaries),
		nameof(SplunkClient.LookupTableFiles),
		nameof(SplunkClient.CalculatedFields),
		nameof(SplunkClient.FieldExtractions),
		nameof(SplunkClient.FieldAliases),
		nameof(SplunkClient.AutomaticLookups),
		nameof(SplunkClient.SourcetypeRenames),
		nameof(SplunkClient.FieldTransforms),
		nameof(SplunkClient.LookupDefinitions),
		nameof(SplunkClient.MetricSchemas),
		nameof(SplunkClient.StatsdExtractions),
		nameof(SplunkClient.GlobalBanners),
		nameof(SplunkClient.Panels),
		nameof(SplunkClient.Views),
		nameof(SplunkClient.DataModels),
		nameof(SplunkClient.DirectoryEntries),
		nameof(SplunkClient.MonitoringConsoleBookmarks),
		nameof(SplunkClient.EventTypes),
		nameof(SplunkClient.SearchFields),
		nameof(SplunkClient.SearchTags)
	];

	[Theory]
	[MemberData(nameof(GroupNames))]
	public void Group_IsCreatedOnceAndReused(string name)
	{
		using var client = TestClient.Create(new StubHandler());
		var property = typeof(SplunkClient).GetProperty(name)!;

		var first = property.GetValue(client);
		var second = property.GetValue(client);

		first.Should().NotBeNull().And.BeSameAs(second);
	}
}
