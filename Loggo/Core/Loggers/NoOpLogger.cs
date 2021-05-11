using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class NoOpLogger : ILogger
	{
		public void Log(LogEntry log)
		{
			// do nothing
		}

		public void LogAll(IReadOnlyCollection<LogEntry> logs)
		{
			// do nothing
		}

		public void Flush()
		{
			// do nothing
		}

		public void Dispose()
		{
			// do nothing
		}
	}
}
