using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Streams
{
	public class GenericOutputStream<T> : ILogOutputStream<T>
	{
		public Action<IReadOnlyCollection<T>> WriteFunction { get; }
		public Action CloseFunction { get; }
		public Action FlushFunction { get; }

		public GenericOutputStream(Action<T> writeFunction, Action closeFunction = null, Action flushFunction = null)
		{
			if (writeFunction == null)
				throw new ArgumentNullException(nameof(writeFunction), $"'{nameof(writeFunction)}' is not allowed to be null.");
			WriteFunction = logs =>
			{
				foreach (T log in logs)
					writeFunction(log);
			};
			CloseFunction = closeFunction;
			FlushFunction = flushFunction;
		}

		public GenericOutputStream(Action<IReadOnlyCollection<T>> writeFunction, Action closeFunction = null, Action flushFunction = null)
		{
			WriteFunction = writeFunction ?? throw new ArgumentNullException(nameof(writeFunction), $"'{nameof(writeFunction)}' is not allowed to be null.");
			CloseFunction = closeFunction;
			FlushFunction = flushFunction;
		}

		public void Write(T log) =>
			WriteAll(new[] {log});

		public void WriteAll(IReadOnlyCollection<T> logs) =>
			WriteFunction(logs);

		public void Flush() =>
			FlushFunction?.Invoke();

		public void Dispose() =>
			CloseFunction?.Invoke();
	}
}
