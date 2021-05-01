using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class GenericLogger<T> : ILogger<T>
	{
		public Action<IReadOnlyCollection<T>> LogFunction { get; }
		public Action CloseFunction { get; }
		public Action FlushFunction { get; }

		public GenericLogger(Action<T> logFunction, Action closeFunction = null, Action flushFunction = null)
		{
			if (logFunction == null)
				throw new ArgumentNullException(nameof(logFunction), $"'{nameof(logFunction)}' is not allowed to be null.");
			LogFunction = logs =>
			{
				foreach (T log in logs)
					logFunction(log);
			};
			CloseFunction = closeFunction;
			FlushFunction = flushFunction;
		}

		public GenericLogger(Action<IReadOnlyCollection<T>> logFunction, Action closeFunction = null, Action flushFunction = null)
		{
			LogFunction = logFunction ?? throw new ArgumentNullException(nameof(logFunction), $"'{nameof(logFunction)}' is not allowed to be null.");
			CloseFunction = closeFunction;
			FlushFunction = flushFunction;
		}

		public void Log(T log)
		{
			LogAll(new[] {log});
		}

		public void LogAll(IReadOnlyCollection<T> logs)
		{
			LogFunction(logs);
		}

		public void Flush()
		{
			FlushFunction?.Invoke();
		}

		public void Dispose()
		{
			CloseFunction?.Invoke();
		}
	}
}
