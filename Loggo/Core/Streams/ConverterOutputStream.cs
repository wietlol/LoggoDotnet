using System;
using System.Collections.Generic;
using Loggo.Api;

namespace Loggo.Core.Streams
{
	public class ConverterOutputStream<TInput, TOutput> : ILogOutputStream<TInput>
	{
		public ILogOutputStream<TOutput> OutputStream { get; }
		public Func<TInput, TOutput> Converter { get; }

		public ConverterOutputStream(ILogOutputStream<TOutput> outputStream, Func<TInput, TOutput> converter)
		{
			OutputStream = outputStream ?? throw new ArgumentNullException(nameof(outputStream), $"'{nameof(outputStream)}' is not allowed to be null.");
			Converter = converter ?? throw new ArgumentNullException(nameof(converter), $"'{nameof(converter)}' is not allowed to be null.");
		}

		public void Write(TInput log) =>
			OutputStream.Write(Converter(log));

		public void WriteAll(IReadOnlyCollection<TInput> logs)
		{
			foreach (TInput log in logs)
				OutputStream.Write(Converter(log));
		}

		public void Flush() =>
			OutputStream.Flush();

		public void Dispose() =>
			OutputStream.Dispose();
	}
}
