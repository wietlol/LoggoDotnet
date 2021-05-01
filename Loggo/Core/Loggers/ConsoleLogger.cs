using System;
using System.Collections.Generic;
using System.IO;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class ConsoleLogger : ILogger<String>
	{
		public TextWriter Writer { get; }

		public ConsoleLogger()
			: this(Console.Out)
		{
			// empty proxy constructor
		}

		public ConsoleLogger(TextWriter writer)
		{
			Writer = writer ?? throw new ArgumentNullException(nameof(writer), $"'{nameof(writer)}' is not allowed to be null.");
		}

		public void Log(String log) =>
			Writer.WriteLine(log);

		public void LogAll(IReadOnlyCollection<String> logs)
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
