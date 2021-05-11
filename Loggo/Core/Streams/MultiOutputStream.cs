using System;
using System.Collections.Generic;
using System.Linq;
using Loggo.Api;

namespace Loggo.Core.Streams
{
	public class MultiOutputStream<T> : ILogOutputStream<T>
	{
		public IReadOnlyList<ILogOutputStream<T>> OutputStreams { get; }

		public MultiOutputStream(IEnumerable<ILogOutputStream<T>> outputStreams)
		{
			OutputStreams = outputStreams?.ToList() ?? throw new ArgumentNullException(nameof(outputStreams), $"'{nameof(outputStreams)}' is not allowed to be null.");
		}

		public void Write(T log)
		{
			foreach (ILogOutputStream<T> outputStream in OutputStreams)
				outputStream.Write(log);
		}

		public void WriteAll(IReadOnlyCollection<T> logs)
		{
			foreach (ILogOutputStream<T> outputStream in OutputStreams)
				outputStream.WriteAll(logs);
		}

		public void Flush()
		{
			foreach (ILogOutputStream<T> outputStream in OutputStreams)
				outputStream.Flush();
		}

		public void Dispose()
		{
			foreach (ILogOutputStream<T> outputStream in OutputStreams)
				outputStream.Dispose();
		}
	}
}
