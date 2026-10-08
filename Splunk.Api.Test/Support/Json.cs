using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Test.Support;

/// <summary>Runs converters directly against a <see cref="Utf8JsonReader"/>, so every token type reaches them.</summary>
internal static class Json
{
	/// <summary>Reads <paramref name="json"/> (one JSON value) with <paramref name="converter"/>.</summary>
	public static T? Read<T>(JsonConverter<T> converter, string json, bool segmented = false)
	{
		var reader = segmented
			? new Utf8JsonReader(Segmented(Encoding.UTF8.GetBytes(json)))
			: new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
		reader.Read();
		return converter.Read(ref reader, typeof(T), SplunkJson.Options);
	}

	/// <summary>Writes <paramref name="value"/> with <paramref name="converter"/> and returns the JSON text.</summary>
	public static string Write<T>(JsonConverter<T> converter, T value)
	{
		using var stream = new MemoryStream();
		using (var writer = new Utf8JsonWriter(stream))
		{
			converter.Write(writer, value, SplunkJson.Options);
		}

		return Encoding.UTF8.GetString(stream.ToArray());
	}

	/// <summary>A sequence of one-byte segments, so a token's value arrives as a value sequence rather than a span.</summary>
	private static ReadOnlySequence<byte> Segmented(byte[] bytes)
	{
		var first = new Segment(bytes.AsMemory(0, 1), 0);
		var last = first;
		for (var i = 1; i < bytes.Length; i++)
		{
			last = last.Append(bytes.AsMemory(i, 1));
		}

		return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
	}

	private sealed class Segment : ReadOnlySequenceSegment<byte>
	{
		public Segment(ReadOnlyMemory<byte> memory, long runningIndex)
		{
			Memory = memory;
			RunningIndex = runningIndex;
		}

		public Segment Append(ReadOnlyMemory<byte> memory)
		{
			var next = new Segment(memory, RunningIndex + Memory.Length);
			Next = next;
			return next;
		}
	}
}
