using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class LazyLogger<T> : ILogger<T>
	{
		public ILoggerFactory<T> Factory { get; }
		private ILogger<T> Logger { get; set; }
		private Object FactoryLock { get; } = new Object();

		public LazyLogger(ILoggerFactory<T> factory)
		{
			Factory = factory ?? throw new ArgumentNullException(nameof(factory), $"'{nameof(factory)}' is not allowed to be null.");
		}

		private ILogger<T> GetLogger()
		{
			if (Logger == null)
				lock (FactoryLock)
					Logger ??= Factory.CreateLogger();

			return Logger;
		}

		public void Log(T log) =>
			GetLogger().Log(log);

		public void LogAll(IReadOnlyCollection<T> logs) =>
			GetLogger().LogAll(logs);

		public void Flush() =>
			GetLogger().Flush();

		public void Dispose() =>
			GetLogger().Dispose();
	}
}
