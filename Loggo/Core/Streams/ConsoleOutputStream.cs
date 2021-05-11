using System;
using System.Collections.Generic;
using System.IO;
using Loggo.Api;

namespace Loggo.Core.Streams
{
	public class ConsoleOutputStream : ILogOutputStream<String>
	{
		public TextWriter Writer { get; }

		public ConsoleOutputStream()
			: this(Console.Out)
		{
			// empty proxy constructor
		}

		public ConsoleOutputStream(TextWriter writer)
		{
			Writer = writer ?? throw new ArgumentNullException(nameof(writer), $"'{nameof(writer)}' is not allowed to be null.");
		}

		public void Write(String log) =>
			Writer.WriteLine(log);

		public void WriteAll(IReadOnlyCollection<String> logs)
		{
			foreach (String log in logs)
				Writer.WriteLine(log);
		}

		public void Flush() =>
			Writer.Flush();

		public void Dispose() =>
			Flush();
	}
}
