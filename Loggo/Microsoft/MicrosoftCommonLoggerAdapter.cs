using System;
using System.Collections.Generic;
using Loggo.Common;
using Loggo.Common.Monitoring;
using Microsoft.Extensions.Logging;
using EventId = Microsoft.Extensions.Logging.EventId;

namespace Loggo.Microsoft
{
	public class MicrosoftCommonLoggerAdapter : ILogger
	{
		private static Common.EventId DefaultScopeEventId { get; } = new Common.EventId(1857276255, "internal-scope");

		public Api.ILogger<CommonLog> Logger { get; }

		private LogLevelMapper LogLevelMapper { get; } = new LogLevelMapper();
		private EventIdMapper EventIdMapper { get; } = new EventIdMapper();

		public MicrosoftCommonLoggerAdapter(Api.ILogger<CommonLog> logger)
		{
			Logger = logger;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, String> formatter)
		{
			Logger.Log(
				LogLevelMapper.Map(logLevel),
				EventIdMapper.Map(eventId),
				MapState(logLevel, state, exception, formatter),
				exception
			);
		}

		private static Object MapState<TState>(LogLevel logLevel, TState state, Exception exception, Func<TState, Exception, String> formatter) =>
			state is IReadOnlyList<KeyValuePair<String, Object>> && formatter != null
				? (Object) new {message = $"[{logLevel}] {formatter(state, exception)}"}
				: state;

		public Boolean IsEnabled(LogLevel logLevel) =>
			true;

		public IDisposable BeginScope<TState>(TState state) =>
			state switch
			{
				Common.EventId commonEventId => new MonitoredScope(Logger, CommonLogSeverity.Information, commonEventId, DateTime.UtcNow, new LogSource("internal-scope")),
				EventId eventId => new MonitoredScope(Logger, CommonLogSeverity.Information, EventIdMapper.Map(eventId), DateTime.UtcNow, new LogSource("internal-scope")),
				_ => new MonitoredScope(Logger, CommonLogSeverity.Information, DefaultScopeEventId, DateTime.UtcNow, new LogSource("internal-scope")),
			};
	}
}
