using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class FilteredLogger : ILogger
	{
		public ILogger Logger { get; }

		// if the filter returns true, the log will be delegated to the inner logger
		public Func<LogEntry, ILogFilter, Boolean> Filter { get; }

		private LinkedList<LogEntry> FilteredLogs { get; } = new LinkedList<LogEntry>();
		private ILogFilter LogFilterObject { get; }

		public FilteredLogger(ILogger logger, Func<LogEntry, Boolean> filter)
			: this(logger, (log, _) => filter(log))
		{
			if (filter == null)
				throw new ArgumentNullException(nameof(filter), $"'{nameof(filter)}' is not allowed to be null.");
			// empty proxy constructor
		}

		public FilteredLogger(ILogger logger, Func<LogEntry, ILogFilter, Boolean> filter)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger), $"'{nameof(logger)}' is not allowed to be null.");
			Filter = filter ?? throw new ArgumentNullException(nameof(filter), $"'{nameof(filter)}' is not allowed to be null.");
			LogFilterObject = new LogFilter(this);
		}

		public void Log(LogEntry log)
		{
			if (Filter(log, LogFilterObject))
				Logger.Log(log);
			else
				FilteredLogs.AddLast(log);
		}

		public void LogAll(IReadOnlyCollection<LogEntry> logs)
		{
			var logged = new List<LogEntry>();
			foreach (LogEntry log in logs)
			{
				if (Filter(log, LogFilterObject))
					logged.Add(log);
				else
					FilteredLogs.AddLast(log);
			}

			Logger.LogAll(logged);
		}

		private void ProcessFilteredLogs()
		{
			LinkedList<LogEntry> list = FilteredLogs;
			LinkedListNode<LogEntry> node = list.First;
			var logs = new List<LogEntry>();
			while (node != null)
			{
				LinkedListNode<LogEntry> next = node.Next;

				LogEntry log = node.Value;
				if (Filter(log, LogFilterObject))
				{
					list.Remove(node);
					logs.Add(log);
				}

				node = next;
			}

			if (logs.Count > 0)
				Logger.LogAll(logs);
		}

		public void Flush() =>
			Logger.Flush();

		public void Dispose()
		{
			FilteredLogs.Clear();
			Logger.Dispose();
		}

		private class LogFilter : ILogFilter
		{
			private FilteredLogger FilteredLogger { get; }

			public LogFilter(FilteredLogger filteredLogger)
			{
				FilteredLogger = filteredLogger;
			}

			public void FilterRulesChanged() =>
				FilteredLogger.ProcessFilteredLogs();
		}
	}

	public interface ILogFilter
	{
		void FilterRulesChanged();
	}
}
