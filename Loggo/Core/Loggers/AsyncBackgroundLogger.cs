using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class AsyncBackgroundLogger<T> : ILogger<T>
	{
		public ILogger<T> Logger { get; }

		private ConcurrentQueue<T> LogQueue { get; } = new ConcurrentQueue<T>();
		private Task WorkerTask { get; }
		private Boolean IsRunning { get; set; }
		private Boolean MustFlush { get; set; }

		public AsyncBackgroundLogger(ILogger<T> logger, TimeSpan sleepDuration)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger), $"'{nameof(logger)}' is not allowed to be null.");
			IsRunning = true;
			MustFlush = false;
			WorkerTask = Task.Factory.StartNew(async () =>
			{
				while (IsRunning)
				{
					if (MustFlush)
					{
						MustFlush = false;
						logger.Flush();
					}

					PassLogs();
					await Task.Delay(sleepDuration);
				}

				PassLogs();
				logger.Dispose();
			});
		}

		private void PassLogs()
		{
			var logs = new List<T>();
			while (LogQueue.TryDequeue(out T log))
				logs.Add(log);
			Logger.LogAll(logs);
		}

		public void Log(T log) =>
			LogQueue.Enqueue(log);

		public void LogAll(IReadOnlyCollection<T> logs)
		{
			foreach (T log in logs)
				LogQueue.Enqueue(log);
		}

		public void Flush() =>
			MustFlush = true;

		public void Dispose() =>
			IsRunning = false;
	}
}
