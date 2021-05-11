using System;
using Loggo.Api;

namespace Loggo.Core.Monitoring
{
	public class MonitoredScope : IDisposable
	{
		public ILogger Logger { get; }
		public ILogSeverity Severity { get; }
		public EventId EventId { get; }
		public DateTime Start { get; }
		public LogSource Source { get; }

		public MonitoredScope(
			ILogger logger,
			ILogSeverity severity,
			EventId eventId,
			DateTime start,
			LogSource source)
		{
			Logger = logger;
			Severity = severity;
			EventId = eventId;
			Start = start;
			Source = source;
		}

		public void Dispose()
		{
			Logger.Log(
				Severity,
				EventId,
				new ScopeResult(
					Source,
					Start,
					DateTime.UtcNow
				),
				source: Source
			);
		}
	}
}
