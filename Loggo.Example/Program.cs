using System;
using Loggo.Api;
using Loggo.Common;
using Loggo.Core;
using Loggo.Core.Factories;
using Loggo.Core.Loggers;
using Loggo.Json;

namespace Loggo.Example
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			ILoggerFactory<CommonLog> loggerFactory = CreateLoggerFactory();
			using ILogger<CommonLog> logger = loggerFactory.CreateLogger();
			using ILogger<CommonLog> logger2 = loggerFactory.CreateLogger();
			using ILogger<CommonLog> logger3 = loggerFactory.CreateLogger();

			using var actualLogger = new ScopedSourceLogger(logger, source => source.Plus("test"));

			logger.LogTrace(new EventId(12345, "test"), new {message = "text"});
			logger2.LogInformation(new EventId(12345, "test"), new {message = "text"});
			logger3.LogTrace(new EventId(12345, "test"), new {message = "text"});
			logger3.LogError(new EventId(12345, "test"), new {message = "text"});
			logger3.LogTrace(new EventId(12345, "test"), new {message = "text"});
		}

		private static ILoggerFactory<CommonLog> CreateLoggerFactory() =>
			Factory(CreateLogger)
				.Let(it => new ProxyCachingLoggerFactory<CommonLog>(it))
				// above singleton
				// below scoped
				.Map(it =>
				{
					var sequenceId = Guid.NewGuid();
					return new ScopedSequenceLogger(it, _ => sequenceId);
				})
				.Let(it => Factory(() =>
				{
					var filter = new TriggerableLevelFilter<CommonLog>(
						CommonLogSeverity.Information,
						CommonLogSeverity.Error,
						CommonLogSeverity.Trace);
					return new FilteredLogger<CommonLog>(it.CreateLogger(), filter.Apply);
				}))
				.Map(it => new FailsafeLogger(it));

		private static ILogger<CommonLog> CreateLogger() =>
			new ConsoleLogger()
				.Let(it => JsonLogger.CreateLogger(it))
				.Let(it => new ScopedSourceLogger(it, source => source.Plus("example")));

		private static ILoggerFactory<CommonLog> Factory(Func<ILogger<CommonLog>> method) =>
			new GenericLoggerFactory<CommonLog>(method);

		private static ILoggerFactory<CommonLog> Map(this ILoggerFactory<CommonLog> factory, Func<ILogger<CommonLog>, ILogger<CommonLog>> mapper) =>
			Factory(() => mapper(factory.CreateLogger()));

		public static R Let<T, R>(this T self, Func<T, R> mapper) =>
			mapper(self);
	}
}
