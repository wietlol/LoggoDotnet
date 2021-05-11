using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class ScopeAggregatorLogger : ILogger
	{
		public ILogger Logger { get; }
		private IList<IScopeResult> Scopes { get; } = new List<IScopeResult>();

		public ScopeAggregatorLogger(ILogger logger)
		{
			Logger = logger;
		}

		public void Log(LogEntry log)
		{
			ProcessLog(log);
			Logger.Log(log);
		}

		public void LogAll(IReadOnlyCollection<LogEntry> logs)
		{
			foreach (LogEntry log in logs)
				ProcessLog(log);
			Logger.LogAll(logs);
		}

		private void ProcessLog(LogEntry log)
		{
			if (log.Data is IScopeResult scope)
				Scopes.Add(scope);
		}

		public void Flush()
		{
			Logger.Flush();
		}

		public void Dispose()
		{
			Logger.LogInformation(
				new EventId(1462484660, "aggregated-scopes"),
				new
				{
					scopes = Scopes,
				}
			);

			Logger.Dispose();
		}
	}
}
