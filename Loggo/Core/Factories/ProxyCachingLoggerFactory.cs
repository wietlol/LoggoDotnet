using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Factories
{
	public class ProxyCachingLoggerFactory : ILoggerFactory
	{
		public ILoggerFactory Factory { get; }

		private Stack<ILogger> LoggerCache { get; } = new Stack<ILogger>();

		public ProxyCachingLoggerFactory(ILoggerFactory factory) =>
			Factory = factory ?? throw new ArgumentNullException(nameof(factory), $"'{nameof(factory)}' is not allowed to be null.");

		public ILogger CreateLogger() =>
			new ProxyCachedLogger(this);

		public ILogger GetOrCreateLogger() =>
			PeekCachedLogger()
			?? CreateInternalLogger();

		public ILogger CreateInternalLogger() =>
			CreateInternalLogger(logger => logger);

		public ILogger CreateInternalLogger(Func<ILogger, ILogger> mapper) =>
			PushCachedLogger(new ProxyInternalLogger(this, mapper(Factory.CreateLogger())));

		public ILogger PeekCachedLogger() =>
			LoggerCache.Count > 0
				? LoggerCache.Peek()
				: null;

		public ILogger PopCachedLogger() =>
			LoggerCache.Count > 0
				? LoggerCache.Pop()
				: null;

		private ILogger PushCachedLogger(ILogger logger)
		{
			LoggerCache.Push(logger);
			return logger;
		}

		private class ProxyInternalLogger : ILogger
		{
			private ProxyCachingLoggerFactory Factory { get; }
			private ILogger Logger { get; }

			public ProxyInternalLogger(ProxyCachingLoggerFactory factory, ILogger logger)
			{
				Factory = factory;
				Logger = logger;
			}

			public void Log(LogEntry log) =>
				Logger.Log(log);

			public void LogAll(IReadOnlyCollection<LogEntry> logs) =>
				Logger.LogAll(logs);

			public void Flush() =>
				Logger.Flush();

			public void Dispose()
			{
				Logger.Dispose();
				Factory.PopCachedLogger();
			}
		}

		private class ProxyCachedLogger : ILogger
		{
			private ProxyCachingLoggerFactory Factory { get; }

			public ProxyCachedLogger(ProxyCachingLoggerFactory factory) =>
				Factory = factory;

			public void Log(LogEntry log) =>
				Factory.GetOrCreateLogger().Log(log);

			public void LogAll(IReadOnlyCollection<LogEntry> logs) =>
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
