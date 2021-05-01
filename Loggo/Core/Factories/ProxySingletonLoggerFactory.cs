using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Factories
{
	public class ProxySingletonLoggerFactory<T> : ILoggerFactory<T>
	{
		public ILoggerFactory<T> LoggerFactory { get; }
		private ILogger<T> Logger { get; set; }
		private Object FactoryLock { get; } = new Object();

		public ProxySingletonLoggerFactory(ILoggerFactory<T> loggerFactory)
		{
			LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory), $"'{nameof(loggerFactory)}' is not allowed to be null.");
		}

		public ILogger<T> CreateLogger() =>
			new ProxySingletonLogger(this);

		private ILogger<T> GetLogger()
		{
			if (Logger == null)
				lock (FactoryLock)
					Logger ??= LoggerFactory.CreateLogger();
			return Logger;
		}

		private class ProxySingletonLogger : ILogger<T>
		{
			private ProxySingletonLoggerFactory<T> Factory { get; }

			public ProxySingletonLogger(ProxySingletonLoggerFactory<T> factory)
			{
				Factory = factory;
			}

			public void Log(T log) =>
				Factory.GetLogger().Log(log);

			public void LogAll(IReadOnlyCollection<T> logs) =>
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
