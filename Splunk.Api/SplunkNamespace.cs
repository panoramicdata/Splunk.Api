namespace Splunk.Api;

/// <summary>
/// A Splunk namespace: the user (owner) and app context of a request, sent as <c>servicesNS/{Owner}/{App}/...</c>.
/// </summary>
/// <remarks>
/// <c>-</c> is Splunk's wildcard: <c>servicesNS/-/-/</c> lists objects from every user and app the caller can see, and
/// <c>nobody</c> as the owner addresses objects shared at app or global level.
/// </remarks>
/// <param name="Owner">The user context, a user name, <c>nobody</c> or the wildcard <c>-</c>.</param>
/// <param name="App">The app context, an app name or the wildcard <c>-</c>.</param>
public sealed record SplunkNamespace(string Owner, string App)
{
	/// <summary>The wildcard namespace <c>-/-</c>: every user and every app.</summary>
	public static SplunkNamespace All { get; } = new("-", "-");

	/// <summary>A namespace for objects shared in an app, owned by <c>nobody</c>.</summary>
	/// <param name="app">The app name.</param>
	/// <returns>The namespace <c>nobody/{app}</c>.</returns>
	public static SplunkNamespace Shared(string app) => new("nobody", app);

	/// <summary>The path prefix this namespace selects, with each part escaped as one segment.</summary>
	internal string PathPrefix => $"servicesNS/{Segment(Owner, nameof(Owner))}/{Segment(App, nameof(App))}/";

	private static string Segment(string value, string name)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value, name);
		// EscapeDataString leaves dots alone, and Uri removes "." and ".." segments, which would leave servicesNS.
		return value is "." or ".."
			? throw new ArgumentException($"The namespace {name} cannot be \"{value}\".", name)
			: Uri.EscapeDataString(value);
	}

	/// <inheritdoc />
	public override string ToString() => $"{Owner}/{App}";
}
