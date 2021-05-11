using System;
using System.Collections.Generic;
using System.Linq;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class AutoBulkLogger : ILogger
	{
		public ILogger Logger { get; }
		public Int32 MaxBufferSize { get; }

		private LogEntry[] Buffer { get; }
		private Int32 BufferSize { get; set; }

		public AutoBulkLogger(ILogger logger, Int32 maxBufferSize)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger), $"'{nameof(logger)}' is not allowed to be null.");
			MaxBufferSize = maxBufferSize;
			Buffer = new LogEntry[maxBufferSize];
			BufferSize = 0;
		}

		public void Log(LogEntry log)
		{
			if (BufferSize >= MaxBufferSize)
				Flush(log);
			else
				Buffer[BufferSize++] = log;
		}

		public void LogAll(IReadOnlyCollection<LogEntry> logs)
		{
			if (BufferSize + logs.Count > MaxBufferSize)
				Flush(logs);
			else
				foreach (LogEntry log in logs)
					Buffer[BufferSize++] = log;
		}

		private void Flush(LogEntry log)
		{
			Logger.LogAll(
				Buffer
					.Take(BufferSize)
					.Concat(new[] {log})
					.ToList()
			);
			BufferSize = 0;
		}

		private void Flush(IEnumerable<LogEntry> logs)
		{
			Logger.LogAll(
				Buffer
					.Take(BufferSize)
					.Concat(logs)
					.ToList()
			);
			BufferSize = 0;
		}

		public void Flush()
		{
			if (BufferSize > 0)
			{
				Logger.LogAll(
					Buffer
						.Take(BufferSize)
						.ToList()
				);
				BufferSize = 0;
			}

			Logger.Flush();
		}

		public void Dispose()
		{
			Flush();
			Logger.Dispose();
		}
	}
}
