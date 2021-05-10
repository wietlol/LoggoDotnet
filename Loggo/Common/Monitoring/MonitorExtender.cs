using System;
using System.Threading.Tasks;
using Loggo.Api;

namespace Loggo.Common.Monitoring
{
	public static class MonitorExtender
	{	
		public static async Task<T> WithinMonitoredScopeAsync<T>(
			this ILogger<CommonLog> logger,
			EventId eventId,
			LogSource source,
			Func<Task<T>> body,
			ILogSeverity severity = null
		)
		{
			using var monitoredScope = new MonitoredScope(logger, severity ?? CommonLogSeverity.Information, eventId, DateTime.UtcNow, source);
			return await body();
		}
		
		public static T WithinMonitoredScope<T>(
			this ILogger<CommonLog> logger,
			EventId eventId,
			LogSource source,
			Func<T> body,
			ILogSeverity severity = null
		)
		{
			using var monitoredScope = new MonitoredScope(logger, severity ?? CommonLogSeverity.Information, eventId, DateTime.UtcNow, source);
			return body();
		}
	}
}
