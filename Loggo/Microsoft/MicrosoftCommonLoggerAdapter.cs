using System;
using System.Collections.Generic;
using Loggo.Api;
using Loggo.Core;
using Loggo.Core.Monitoring;
using Microsoft.Extensions.Logging;
using EventId = Microsoft.Extensions.Logging.EventId;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Loggo.Microsoft
{
	public class MicrosoftCommonLoggerAdapter : ILogger
	{
		private static Api.EventId DefaultScopeEventId { get; } = new Api.EventId(1857276255, "internal-scope");

		public Api.ILogger Logger { get; }
		public String CategoryName { get; }

		public MicrosoftCommonLoggerAdapter(Api.ILogger logger, String categoryName = null)
		{
			Logger = logger;
			CategoryName = categoryName;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, String> formatter)
		{
			Logger.Log(
				LogLevelMapper.Map(logLevel),
				EventIdMapper.Map(eventId),
				MapState(logLevel, state, exception, formatter),
				exception,
				CategoryName != null ? new LogSource(CategoryName) : default
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
				Api.EventId commonEventId => new MonitoredScope(Logger, CommonLogSeverity.Information, commonEventId, DateTime.UtcNow, new LogSource("internal-scope")),
				EventId eventId => new MonitoredScope(Logger, CommonLogSeverity.Information, EventIdMapper.Map(eventId), DateTime.UtcNow, new LogSource("internal-scope")),
				_ => new MonitoredScope(Logger, CommonLogSeverity.Information, DefaultScopeEventId, DateTime.UtcNow, new LogSource("internal-scope")),
			};
	}
}
