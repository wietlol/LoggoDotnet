using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class ProxyLogger<T> : ILogger<T>
	{
		public Func<ILogger<T>> LoggerSupplier { get; }

		public ProxyLogger(Func<ILogger<T>> loggerSupplier) =>
			LoggerSupplier = loggerSupplier;

		public void Log(T log) =>
			LoggerSupplier().Log(log);

		public void LogAll(IReadOnlyCollection<T> logs) =>
			LoggerSupplier().LogAll(logs);

		public void Flush() =>
			LoggerSupplier().Flush();

		public void Dispose() =>
			LoggerSupplier().Dispose();
	}
}
