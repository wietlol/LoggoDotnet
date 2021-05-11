using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class LazyLogger : ILogger
	{
		public ILoggerFactory Factory { get; }
		private ILogger Logger { get; set; }
		private Object FactoryLock { get; } = new Object();

		public LazyLogger(ILoggerFactory factory)
		{
			Factory = factory ?? throw new ArgumentNullException(nameof(factory), $"'{nameof(factory)}' is not allowed to be null.");
		}

		private ILogger GetLogger()
		{
			if (Logger == null)
				lock (FactoryLock)
					Logger ??= Factory.CreateLogger();

			return Logger;
		}

		public void Log(LogEntry log) =>
			GetLogger().Log(log);

		public void LogAll(IReadOnlyCollection<LogEntry> logs) =>
			GetLogger().LogAll(logs);

		public void Flush() =>
			GetLogger().Flush();

		public void Dispose() =>
			GetLogger().Dispose();
	}
}
