using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Factories
{
	public class ProxySingletonLoggerFactory : ILoggerFactory
	{
		public ILoggerFactory LoggerFactory { get; }
		private ILogger Logger { get; set; }
		private Object FactoryLock { get; } = new Object();

		public ProxySingletonLoggerFactory(ILoggerFactory loggerFactory)
		{
			LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory), $"'{nameof(loggerFactory)}' is not allowed to be null.");
		}

		public ILogger CreateLogger() =>
			new ProxySingletonLogger(this);

		private ILogger GetLogger()
		{
			if (Logger == null)
				lock (FactoryLock)
					Logger ??= LoggerFactory.CreateLogger();
			return Logger;
		}

		private class ProxySingletonLogger : ILogger
		{
			private ProxySingletonLoggerFactory Factory { get; }

			public ProxySingletonLogger(ProxySingletonLoggerFactory factory)
			{
				Factory = factory;
			}

			public void Log(LogEntry log) =>
				Factory.GetLogger().Log(log);

			public void LogAll(IReadOnlyCollection<LogEntry> logs) =>
				Factory.GetLogger().LogAll(logs);

			public void Flush() =>
				Factory.GetLogger().Flush();

			public void Dispose()
			{
				// nothing to do
			}
		}
	}
}
