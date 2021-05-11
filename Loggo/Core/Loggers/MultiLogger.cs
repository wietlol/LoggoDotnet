using System;
using System.Collections.Generic;
using System.Linq;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class MultiLogger : ILogger
	{
		public IReadOnlyList<ILogger> Loggers { get; }

		public MultiLogger(IEnumerable<ILogger> loggers)
		{
			Loggers = loggers?.ToList() ?? throw new ArgumentNullException(nameof(loggers), $"'{nameof(loggers)}' is not allowed to be null.");
		}

		public void Log(LogEntry log)
		{
			foreach (ILogger logger in Loggers)
				logger.Log(log);
		}

		public void LogAll(IReadOnlyCollection<LogEntry> logs)
		{
			foreach (ILogger logger in Loggers)
				logger.LogAll(logs);
		}

		public void Flush()
		{
			foreach (ILogger logger in Loggers)
				logger.Flush();
		}

		public void Dispose()
		{
			foreach (ILogger logger in Loggers)
				logger.Dispose();
		}
	}
}
