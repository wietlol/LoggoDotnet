using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class OutputLogger : ILogger
	{
		public ILogOutputStream<LogEntry> OutputStream { get; }

		public OutputLogger(ILogOutputStream<LogEntry> outputStream)
		{
			OutputStream = outputStream;
		}

		public void Log(LogEntry log) =>
			OutputStream.Write(log);

		public void LogAll(IReadOnlyCollection<LogEntry> logs) =>
			OutputStream.WriteAll(logs);

		public void Flush() =>
			OutputStream.Flush();

		public void Dispose() =>
			OutputStream.Dispose();
	}
}
