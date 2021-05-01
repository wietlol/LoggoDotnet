using System;
using System.Collections.Generic;
using System.Linq;
using Loggo.Api;

namespace Loggo.Common
{
	public class ScopedSequenceLogger : ILogger<CommonLog>
	{
		public ILogger<CommonLog> Logger { get; }
		public Func<Guid, Guid> Converter { get; }

		public ScopedSequenceLogger(ILogger<CommonLog> logger, Func<Guid, Guid> converter)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger), $"'{nameof(logger)}' is not allowed to be null.");
			Converter = converter ?? throw new ArgumentNullException(nameof(converter), $"'{nameof(converter)}' is not allowed to be null.");
		}

		public void Log(CommonLog log) =>
			Logger.Log(log.With(sequenceId: Converter(log.SequenceId)));

		public void LogAll(IReadOnlyCollection<CommonLog> logs) =>
			Logger.LogAll(
				logs
					.Select(it => it.With(sequenceId: Converter(it.SequenceId)))
					.ToList()
			);

		public void Flush() =>
			Logger.Flush();

		public void Dispose() =>
			Logger.Dispose();
	}
}
