using System;
using System.Collections.Generic;
using System.Linq;
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
			if (!ProcessLog(log))
				Logger.Log(log);
		}

		public void LogAll(IReadOnlyCollection<LogEntry> logs)
		{
			List<LogEntry> unprocessedLogs = logs
				.Where(it => !ProcessLog(it))
				.ToList();
			Logger.LogAll(unprocessedLogs);
		}

		private Boolean ProcessLog(LogEntry log)
		{
			if (log.Data is IScopeResult scope)
			{
				Scopes.Add(scope);
				return false;
			}

			return true;
		}

		public void Flush()
		{
			Logger.Flush();
		}

		public void Dispose()
		{
			if (Scopes.Count > 0)
			{
				IScopeResult last = Scopes.Last();
				Int32 elapsedMilliseconds = last.End != null
					? (Int32) (last.End.Value - last.Start).TotalMilliseconds
					: 0;
				Logger.LogInformation(
					new EventId(1462484660, "aggregated-scopes"),
					new
					{
						scopes = Scopes,
						elapsedMilliseconds,
						message = $"Scope '{last.Source}' ran for {elapsedMilliseconds}ms, starting at {last.Start:O}.",
					}
				);
				
				// clear collected scopes when disposing to avoid duplicate aggregated logging
				Scopes.Clear();
			}

			Logger.Dispose();
		}
	}
}
