using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Splunk.Api.Test.Support;

/// <summary>A logger that keeps every formatted message, with its level.</summary>
internal sealed class CapturingLogger : ILogger
{
	private readonly ConcurrentQueue<(LogLevel Level, string Message)> _entries = new();

	public IReadOnlyList<(LogLevel Level, string Message)> Entries => [.. _entries];

	public IReadOnlyList<string> Messages => [.. _entries.Select(e => e.Message)];

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

	public bool IsEnabled(LogLevel logLevel) => true;

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		=> _entries.Enqueue((logLevel, formatter(state, exception)));
}
