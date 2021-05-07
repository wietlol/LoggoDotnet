using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Factories
{
	public class ProxyCachingLoggerFactory<T> : ILoggerFactory<T>
	{
		public ILoggerFactory<T> Factory { get; }

		private Stack<ILogger<T>> LoggerCache { get; } = new Stack<ILogger<T>>();

		public ProxyCachingLoggerFactory(ILoggerFactory<T> factory) =>
			Factory = factory ?? throw new ArgumentNullException(nameof(factory), $"'{nameof(factory)}' is not allowed to be null.");

		public ILogger<T> CreateLogger() =>
			new ProxyCachedLogger(this);

		public ILogger<T> GetOrCreateLogger() =>
			PeekCachedLogger()
			?? CreateInternalLogger();

		public ILogger<T> CreateInternalLogger() =>
			CreateInternalLogger(logger => logger);

		public ILogger<T> CreateInternalLogger(Func<ILogger<T>, ILogger<T>> mapper) =>
			PushCachedLogger(new ProxyInternalLogger(this, mapper(Factory.CreateLogger())));

		public ILogger<T> PeekCachedLogger() =>
			LoggerCache.Count > 0
				? LoggerCache.Peek()
				: null;

		public ILogger<T> PopCachedLogger() =>
			LoggerCache.Count > 0
				? LoggerCache.Pop()
				: null;

		private ILogger<T> PushCachedLogger(ILogger<T> logger)
		{
			LoggerCache.Push(logger);
			return logger;
		}

		private class ProxyInternalLogger : ILogger<T>
		{
			private ProxyCachingLoggerFactory<T> Factory { get; }
			private ILogger<T> Logger { get; }

			public ProxyInternalLogger(ProxyCachingLoggerFactory<T> factory, ILogger<T> logger)
			{
				Factory = factory;
				Logger = logger;
			}

			public void Log(T log) =>
				Logger.Log(log);

			public void LogAll(IReadOnlyCollection<T> logs) =>
				Logger.LogAll(logs);

			public void Flush() =>
				Logger.Flush();

			public void Dispose()
			{
				Logger.Dispose();
				Factory.PopCachedLogger();
			}
		}

		private class ProxyCachedLogger : ILogger<T>
		{
			private ProxyCachingLoggerFactory<T> Factory { get; }

			public ProxyCachedLogger(ProxyCachingLoggerFactory<T> factory) =>
				Factory = factory;

			public void Log(T log) =>
				Factory.GetOrCreateLogger().Log(log);

			public void LogAll(IReadOnlyCollection<T> logs) =>
				Factory.GetOrCreateLogger().LogAll(logs);

			public void Flush() =>
				Factory.GetOrCreateLogger().Flush();

			public void Dispose()
			{
				// nothing to do
			}
		}
	}
}
