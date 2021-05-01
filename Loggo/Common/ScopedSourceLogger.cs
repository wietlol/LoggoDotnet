using System;
using System.Collections.Generic;
using System.Linq;
using Loggo.Api;

namespace Loggo.Common
{
	public class ScopedSourceLogger : ILogger<CommonLog>
	{
		public ILogger<CommonLog> Logger { get; }
		public Func<LogSource, LogSource> Converter { get; }

		public ScopedSourceLogger(ILogger<CommonLog> logger, Func<LogSource, LogSource> converter)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger), $"'{nameof(logger)}' is not allowed to be null.");
			Converter = converter ?? throw new ArgumentNullException(nameof(converter), $"'{nameof(converter)}' is not allowed to be null.");
		}

		public void Log(CommonLog log) =>
			Logger.Log(log.With(source: Converter(log.Source)));

		public void LogAll(IReadOnlyCollection<CommonLog> logs) =>
			Logger.LogAll(
				logs
					.Select(it => it.With(source: Converter(it.Source)))
					.ToList()
			);

		public void Flush() =>
			Logger.Flush();

		public void Dispose() =>
			Logger.Dispose();
	}
}
