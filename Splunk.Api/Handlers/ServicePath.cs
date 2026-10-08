namespace Splunk.Api.Handlers;

/// <summary>Locates the Splunk endpoint path within a request URL, below the client's base address.</summary>
internal static class ServicePath
{
	private const string ServicesPrefix = "services/";
	private const string ServicesNsPrefix = "servicesNS/";

	/// <summary>
	/// The request path relative to <paramref name="baseUri"/>, for example <c>services/server/info</c>, or
	/// <see langword="null"/> when the request is not below the base address.
	/// </summary>
	public static string? Relative(Uri baseUri, Uri requestUri)
	{
		var basePath = baseUri.AbsolutePath.EndsWith('/') ? baseUri.AbsolutePath : baseUri.AbsolutePath + "/";
		var path = requestUri.AbsolutePath;
		return path.StartsWith(basePath, StringComparison.Ordinal) ? path[basePath.Length..] : null;
	}

	/// <summary>
	/// The endpoint path with its <c>services/</c> or <c>servicesNS/{owner}/{app}/</c> prefix removed, for example
	/// <c>search/jobs</c>; <see langword="null"/> when the path has neither prefix.
	/// </summary>
	public static string? Endpoint(string relativePath)
	{
		if (relativePath.StartsWith(ServicesPrefix, StringComparison.Ordinal))
		{
			return relativePath[ServicesPrefix.Length..];
		}

		if (!relativePath.StartsWith(ServicesNsPrefix, StringComparison.Ordinal))
		{
			return null;
		}

		// servicesNS/{owner}/{app}/{endpoint}
		var parts = relativePath.Split('/', 4);
		return parts.Length == 4 ? parts[3] : string.Empty;
	}

	/// <summary>Whether the relative path addresses the global <c>services/</c> context.</summary>
	public static bool IsGlobalServices(string relativePath) => relativePath.StartsWith(ServicesPrefix, StringComparison.Ordinal);

	/// <summary>Replaces the leading <c>services/</c> of a relative path with <paramref name="namespacePrefix"/>.</summary>
	public static string ToNamespace(string relativePath, string namespacePrefix) => namespacePrefix + relativePath[ServicesPrefix.Length..];
}
