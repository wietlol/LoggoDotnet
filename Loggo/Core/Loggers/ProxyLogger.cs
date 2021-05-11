using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class ProxyLogger : ILogger
	{
		public Func<ILogger> LoggerSupplier { get; }

		public ProxyLogger(Func<ILogger> loggerSupplier) =>
			LoggerSupplier = loggerSupplier;

		public void Log(LogEntry log) =>
			LoggerSupplier().Log(log);

		public void LogAll(IReadOnlyCollection<LogEntry> logs) =>
			LoggerSupplier().LogAll(logs);

		public void Flush() =>
			LoggerSupplier().Flush();

		public void Dispose() =>
			LoggerSupplier().Dispose();
	}
}
