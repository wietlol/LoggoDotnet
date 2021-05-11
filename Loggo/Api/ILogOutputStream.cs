using System;
using System.Collections.Generic;

namespace Loggo.Api
{
	public interface ILogOutputStream<in T> : IDisposable
	{
		void Flush();
		
		void Write(T log);
		
		void WriteAll(IReadOnlyCollection<T> logs);
	}
}
