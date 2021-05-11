using System;
using Loggo.Api;

namespace Loggo.Core.Factories
{
	public class GenericLoggerFactory : ILoggerFactory
	{
		public Func<ILogger> CreateFunction { get; }

		public GenericLoggerFactory(Func<ILogger> createFunction)
		{
			CreateFunction = createFunction ?? throw new ArgumentNullException(nameof(createFunction), $"'{nameof(createFunction)}' is not allowed to be null.");
		}

		public ILogger CreateLogger() =>
			CreateFunction();
	}
}
