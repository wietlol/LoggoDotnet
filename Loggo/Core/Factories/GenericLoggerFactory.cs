using System;
using Loggo.Api;

namespace Loggo.Core.Factories
{
	public class GenericLoggerFactory<T> : ILoggerFactory<T>
	{
		public Func<ILogger<T>> CreateFunction { get; }

		public GenericLoggerFactory(Func<ILogger<T>> createFunction)
		{
			CreateFunction = createFunction ?? throw new ArgumentNullException(nameof(createFunction), $"'{nameof(createFunction)}' is not allowed to be null.");
		}

		public ILogger<T> CreateLogger() =>
			CreateFunction();
	}
}
