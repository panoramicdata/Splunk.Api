using Splunk.Api.Models.Knowledge;
using System.Text.Json;

namespace Splunk.Api.Test.Groups;

/// <summary>Pins model behaviour that captured responses alone do not reach.</summary>
public class KnowledgeModelTests
{
	[Fact]
	public void NullLists_ReadAsEmpty()
	{
		// Splunk 10.6 reports "fields_array": null for a file lookup whose file is missing.
		Read<LookupDefinition>("""{"fields_array":null}""").Fields.Should().BeEmpty();
		Read<LookupTableFile>("""{"fields_array":null}""").Fields.Should().BeEmpty();
		Read<EventType>("""{"tags":null}""").Tags.Should().BeEmpty();
	}

	private static T Read<T>(string json) => JsonSerializer.Deserialize<T>(json, SplunkJson.Options)!;
}
