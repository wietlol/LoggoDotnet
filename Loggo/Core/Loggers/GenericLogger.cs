using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class GenericLogger : ILogger
	{
		public Action<IReadOnlyCollection<LogEntry>> LogFunction { get; }
		public Action CloseFunction { get; }
		public Action FlushFunction { get; }

		public GenericLogger(Action<LogEntry> logFunction, Action closeFunction = null, Action flushFunction = null)
		{
			if (logFunction == null)
				throw new ArgumentNullException(nameof(logFunction), $"'{nameof(logFunction)}' is not allowed to be null.");
			LogFunction = logs =>
			{
				foreach (LogEntry log in logs)
					logFunction(log);
			};
			CloseFunction = closeFunction;
			FlushFunction = flushFunction;
		}

		public GenericLogger(Action<IReadOnlyCollection<LogEntry>> logFunction, Action closeFunction = null, Action flushFunction = null)
		{
			LogFunction = logFunction ?? throw new ArgumentNullException(nameof(logFunction), $"'{nameof(logFunction)}' is not allowed to be null.");
			CloseFunction = closeFunction;
			FlushFunction = flushFunction;
		}

		public void Log(LogEntry log) =>
			LogAll(new[] {log});

		public void LogAll(IReadOnlyCollection<LogEntry> logs) =>
			LogFunction(logs);

		public void Flush() =>
			FlushFunction?.Invoke();

		public void Dispose() =>
			CloseFunction?.Invoke();
	}
}
