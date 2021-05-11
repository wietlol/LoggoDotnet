using System;
using System.Collections.Generic;
using Loggo.Api;
using Loggo.Core.Factories;
using Loggo.Core.Loggers;
using Loggo.Core.Streams;

namespace Loggo.Core
{
	public class LoggerDsl
	{
		public static ILoggerFactory LoggerFactory(Func<ILogger> create) =>
			new GenericLoggerFactory(create);
		
		public static ILogger Logger(Action<LogEntry> log) =>
			new GenericLogger(log);
		
		public static ILogger Logger(Action<IReadOnlyCollection<LogEntry>> log) =>
			new GenericLogger(log);

		public static ILogOutputStream<T> LogOutput<T>(Action<T> log) =>
			new GenericOutputStream<T>(log);

		public static ILogOutputStream<T> LogOutput<T>(Action<IReadOnlyCollection<T>> log) =>
			new GenericOutputStream<T>(log);

		public static ILoggerFactory LoggerAsFactory(Action<LogEntry> log)
		{
			ILogger logger = Logger(log);
			return new GenericLoggerFactory(() => logger);
		}

		public static ILoggerFactory LoggerAsFactory(Action<IReadOnlyCollection<LogEntry>> log)
		{
			ILogger logger = Logger(log);
			return new GenericLoggerFactory(() => logger);
		}
	}
}
