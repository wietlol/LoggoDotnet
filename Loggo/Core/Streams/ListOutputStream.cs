using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Streams
{
	public class ListOutputStream<T> : ILogOutputStream<T>
	{
		public IList<T> Logs { get; }

		public ListOutputStream(IList<T> logs)
		{
			Logs = logs;
		}

		public void Write(T log) =>
			Logs.Add(log);

		public void WriteAll(IReadOnlyCollection<T> logs)
		{
			foreach (T log in logs)
				Logs.Add(log);
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
