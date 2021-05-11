using System;
using System.Collections.Generic;

namespace Loggo.Api
{
	public interface ILogger : IDisposable
	{
		void Flush();

		void Log(LogEntry log);

		void LogAll(IReadOnlyCollection<LogEntry> logs);
	}
}
