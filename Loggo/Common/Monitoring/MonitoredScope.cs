using System;
using Loggo.Api;

namespace Loggo.Common.Monitoring
{
	public class MonitoredScope : IDisposable
	{
		public ILogger<CommonLog> Logger { get; }
		public ILogSeverity Severity { get; }
		public EventId EventId { get; }
		public DateTime Start { get; }
		public LogSource Source { get; }

		public MonitoredScope(
			ILogger<CommonLog> logger,
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
			DateTime end = DateTime.UtcNow;
			var elapsedMilliseconds = (Int64) (end - Start).TotalMilliseconds;
			Logger.Log(
				Severity,
				EventId,
				new
				{
					start = Start,
					end,
					source = Source,
					eventType = "monitored-scope",
					elapsedMilliseconds,
					message = $"Scope '{EventId.Name}' ran for {elapsedMilliseconds}ms, starting at {Start}",
				},
				source: Source
			);
		}
	}
}
