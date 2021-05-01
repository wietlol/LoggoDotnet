using System;
using System.Collections.Generic;
using System.Linq;
using Loggo.Api;

namespace Loggo.Common
{
	public static class LoggerFunctions
	{
		private static LogSource DefaultSource { get; } = new LogSource(Enumerable.Empty<String>());
		private static Guid DefaultSequenceId { get; } = Guid.Empty;

		public static void Log(
			this ILogger<CommonLog> logger,
			ILogSeverity severity,
			EventId eventId,
			Object data,
			Exception exception = null,
			LogSource source = null,
			Guid? sequenceId = null,
			IDictionary<String, Object> metadata = null
		) =>
			logger.Log(new CommonLog(
				severity,
				DateTime.UtcNow,
				source ?? DefaultSource,
				sequenceId ?? DefaultSequenceId,
				eventId,
				data,
				metadata ?? new Dictionary<String, Object>(),
				exception
			));

		public static void LogTrace(
			this ILogger<CommonLog> logger,
			EventId eventId,
			Object data,
			Exception exception = null,
			LogSource source = null,
			Guid? sequenceId = null,
			IDictionary<String, Object> metadata = null
		) =>
			logger.Log(CommonLogSeverity.Trace, eventId, data, exception, source, sequenceId, metadata);

		public static void LogDebug(
			this ILogger<CommonLog> logger,
			EventId eventId,
			Object data,
			Exception exception = null,
			LogSource source = null,
			Guid? sequenceId = null,
			IDictionary<String, Object> metadata = null
		) =>
			logger.Log(CommonLogSeverity.Debug, eventId, data, exception, source, sequenceId, metadata);

		public static void LogInformation(
			this ILogger<CommonLog> logger,
			EventId eventId,
			Object data,
			Exception exception = null,
			LogSource source = null,
			Guid? sequenceId = null,
			IDictionary<String, Object> metadata = null
		) =>
			logger.Log(CommonLogSeverity.Information, eventId, data, exception, source, sequenceId, metadata);

		public static void LogWarning(
			this ILogger<CommonLog> logger,
			EventId eventId,
			Object data,
			Exception exception = null,
			LogSource source = null,
			Guid? sequenceId = null,
			IDictionary<String, Object> metadata = null
		) =>
			logger.Log(CommonLogSeverity.Warning, eventId, data, exception, source, sequenceId, metadata);

		public static void LogError(
			this ILogger<CommonLog> logger,
			EventId eventId,
			Object data,
			Exception exception = null,
			LogSource source = null,
			Guid? sequenceId = null,
			IDictionary<String, Object> metadata = null
		) =>
			logger.Log(CommonLogSeverity.Error, eventId, data, exception, source, sequenceId, metadata);

		public static void LogCritical(
			this ILogger<CommonLog> logger,
			EventId eventId,
			Object data,
			Exception exception = null,
			LogSource source = null,
			Guid? sequenceId = null,
			IDictionary<String, Object> metadata = null
		) =>
			logger.Log(CommonLogSeverity.Critical, eventId, data, exception, source, sequenceId, metadata);
	}
}
