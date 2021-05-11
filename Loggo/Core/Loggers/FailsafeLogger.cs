using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class FailsafeLogger : ILogger
	{
		private static EventId FailsafeErrorEventId { get; } = new EventId(1972263977, "internal-logging-error");

		public ILogger Logger { get; }

		public FailsafeLogger(ILogger logger)
		{
			Logger = logger;
		}

		public void Log(LogEntry log)
		{
			try
			{
				Logger.Log(log);
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public void LogAll(IReadOnlyCollection<LogEntry> logs)
		{
			try
			{
				Logger.LogAll(logs);
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public void Flush()
		{
			try
			{
				Logger.Flush();
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		public void Dispose()
		{
			try
			{
				Logger.Dispose();
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}

		private void LogError(Exception ex) =>
			Logger.LogCritical(
				FailsafeErrorEventId,
				new { },
				ex
			);
	}
}
