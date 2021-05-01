using System;
using System.Collections.Generic;

namespace Loggo.Api
{
	public interface ILogger<in T> : IDisposable
	{
		void Flush();

		void Log(T log);

		void LogAll(IReadOnlyCollection<T> logs);
	}
}
