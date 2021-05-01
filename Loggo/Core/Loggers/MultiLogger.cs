using System;
using System.Collections.Generic;
using System.Linq;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class MultiLogger<T> : ILogger<T>
	{
		public IReadOnlyList<ILogger<T>> Loggers { get; }

		public MultiLogger(IEnumerable<ILogger<T>> loggers)
		{
			Loggers = loggers?.ToList() ?? throw new ArgumentNullException(nameof(loggers), $"'{nameof(loggers)}' is not allowed to be null.");
		}

		public void Log(T log) =>
			LogAll(new[] {log});

		public void LogAll(IReadOnlyCollection<T> logs)
		{
			foreach (ILogger<T> logger in Loggers)
				logger.LogAll(logs);
		}

		public void Flush()
		{
			foreach (ILogger<T> logger in Loggers)
				logger.Flush();
		}

		public void Dispose()
		{
			foreach (ILogger<T> logger in Loggers)
				logger.Dispose();
		}
	}
}
