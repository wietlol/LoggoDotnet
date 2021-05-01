using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Common
{
	public class CommonLog : IMeasurableLog
	{
		public ILogSeverity Severity { get; }
		public DateTime Moment { get; }
		public LogSource Source { get; }
		public Guid SequenceId { get; }
		public EventId EventId { get; }
		public Object Data { get; }
		public IDictionary<String, Object> Metadata { get; }
		public Exception Exception { get; }

		public CommonLog(
			ILogSeverity severity,
			DateTime moment,
			LogSource source,
			Guid sequenceId,
			EventId eventId,
			Object data,
			IDictionary<String, Object> metadata,
			Exception exception)
		{
			Severity = severity ?? throw new ArgumentNullException(nameof(severity), $"'{nameof(severity)}' is not allowed to be null.");
			Moment = moment;
			Source = source ?? throw new ArgumentNullException(nameof(source), $"'{nameof(source)}' is not allowed to be null.");
			SequenceId = sequenceId;
			EventId = eventId;
			Data = data ?? throw new ArgumentNullException(nameof(data), $"'{nameof(data)}' is not allowed to be null.");
			Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata), $"'{nameof(metadata)}' is not allowed to be null.");
			Exception = exception;
		}

		public CommonLog With(
			ILogSeverity severity = null,
			DateTime? moment = null,
			LogSource source = null,
			Guid? sequenceId = null,
			EventId? eventId = null,
			Object data = null,
			IDictionary<String, Object> metadata = null,
			Exception exception = null
		) =>
			new CommonLog(
				severity ?? Severity,
				moment ?? Moment,
				source ?? Source,
				sequenceId ?? SequenceId,
				eventId ?? EventId,
				data ?? Data,
				metadata ?? Metadata,
				exception ?? Exception
			);
	}
}
