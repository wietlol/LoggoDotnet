using System;
using System.Collections.Generic;
using Loggo.Api;
using Loggo.Core.Factories;
using Loggo.Core.Loggers;

namespace Loggo.Core
{
	public class LoggerDsl
	{
		public static ILoggerFactory<T> LoggerFactory<T>(Func<ILogger<T>> create) =>
			new GenericLoggerFactory<T>(create);
		
		public static ILogger<T> Logger<T>(Action<T> log) =>
			new GenericLogger<T>(log);
		
		public static ILogger<T> Logger<T>(Action<IReadOnlyCollection<T>> log) =>
			new GenericLogger<T>(log);

		public static ILoggerFactory<T> LoggerAsFactory<T>(Action<T> log)
		{
			ILogger<T> logger = Logger(log);
			return new GenericLoggerFactory<T>(() => logger);
		}

		public static ILoggerFactory<T> LoggerAsFactory<T>(Action<IReadOnlyCollection<T>> log)
		{
			ILogger<T> logger = Logger(log);
			return new GenericLoggerFactory<T>(() => logger);
		}
	}
}
