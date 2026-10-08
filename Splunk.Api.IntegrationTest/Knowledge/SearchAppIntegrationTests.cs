namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>The base of the knowledge object tests, which create their objects in the search app (<c>nobody/search</c>).</summary>
public abstract class SearchAppIntegrationTests(SplunkFixture fixture) : IDisposable
{
	/// <summary>The shared fixture.</summary>
	protected SplunkFixture Fixture { get; } = fixture;

	/// <summary>A client in the <c>nobody/search</c> namespace.</summary>
	protected SplunkClient App { get; } = fixture.Client.InNamespace("nobody", "search");

	/// <summary>The test's cancellation token.</summary>
	protected static CancellationToken Token => TestContext.Current.CancellationToken;

	/// <inheritdoc />
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>Disposes <see cref="App"/>.</summary>
	/// <param name="disposing">Whether this is called from <see cref="Dispose()"/>.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			App.Dispose();
		}
	}
}
