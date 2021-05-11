using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class ThreadedBackgroundLogger : ILogger
	{
		public ILogger Logger { get; }

		private ConcurrentQueue<LogEntry> LogQueue { get; } = new ConcurrentQueue<LogEntry>();
		private Thread WorkerThread { get; }
		private Boolean IsRunning { get; set; }
		private Boolean MustFlush { get; set; }

		public ThreadedBackgroundLogger(ILogger logger, TimeSpan sleepDuration)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger), $"'{nameof(logger)}' is not allowed to be null.");
			IsRunning = true;
			MustFlush = false;
			WorkerThread = new Thread(() =>
			{
				while (IsRunning)
				{
					if (MustFlush)
					{
						MustFlush = false;
						logger.Flush();
					}

					PassLogs();
					Thread.Sleep(sleepDuration);
				}

				PassLogs();
				logger.Dispose();
			});
			WorkerThread.Start();
		}

		private void PassLogs()
		{
			var logs = new List<LogEntry>();
			while (LogQueue.TryDequeue(out LogEntry log))
				logs.Add(log);
			Logger.LogAll(logs);
		}

		public void Log(LogEntry log) =>
			LogQueue.Enqueue(log);

		public void LogAll(IReadOnlyCollection<LogEntry> logs)
		{
			foreach (LogEntry log in logs)
				LogQueue.Enqueue(log);
		}

		public void Flush() =>
			MustFlush = true;

		public void Dispose() =>
			IsRunning = false;
	}
}
