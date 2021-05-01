using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Loggers
{
	public class NoOpLogger : ILogger<Object>
	{
		public void Log(Object log)
		{
			// do nothing
		}

		public void LogAll(IReadOnlyCollection<Object> logs)
		{
			// do nothing
		}

		public void Flush()
		{
			// do nothing
		}

		public void Dispose()
		{
			// do nothing
		}
	}
}
