using System.Reflection;

namespace Splunk.Api.Test.Core;

/// <summary>Keeps the endpoint interfaces implementable outside Splunk.Api (by callers' own classes and by mocking libraries).</summary>
public class InterfaceSurfaceTests
{
	private static readonly List<Type> Interfaces = [.. typeof(SplunkClient).Assembly.GetExportedTypes()
		.Where(t => t.IsInterface && t.Namespace == "Splunk.Api.Interfaces")];

	[Fact]
	public void EndpointInterfaces_HaveNoNonPublicMembers()
	{
		var nonPublic = Interfaces
			.SelectMany(t => t.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
				.Select(m => $"{t.Name}.{m.Name}"))
			.ToList();

		Interfaces.Should().NotBeEmpty();
		nonPublic.Should().BeEmpty("a non-public interface member cannot be implemented by an assembly without InternalsVisibleTo, so callers could not implement or mock the interface");
	}

	[Fact]
	public void EndpointInterfaces_AreAllPublic()
		=> typeof(SplunkClient).Assembly.GetTypes()
			.Where(t => t.IsInterface && t.Namespace == "Splunk.Api.Interfaces" && !t.IsPublic)
			.Should().BeEmpty("every endpoint interface is exposed by a public SplunkClient property");
}
