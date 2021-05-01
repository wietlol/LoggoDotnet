using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class ConverterLogger<I, O> : ILogger<I>
	{
		public ILogger<O> Logger { get; }
		public Func<I, O> Converter { get; }

		public ConverterLogger(ILogger<O> logger, Func<I, O> converter)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger), $"'{nameof(logger)}' is not allowed to be null.");
			Converter = converter ?? throw new ArgumentNullException(nameof(converter), $"'{nameof(converter)}' is not allowed to be null.");
		}

		public void Log(I log) =>
			Logger.Log(Converter(log));

		public void LogAll(IReadOnlyCollection<I> logs)
		{
			foreach (I log in logs)
				Logger.Log(Converter(log));
		}

		public void Flush() =>
			Logger.Flush();

		public void Dispose() =>
			Logger.Dispose();
	}
}
