using System;
using Microsoft.Extensions.Logging;

namespace Loggo.Microsoft
{
	public class GenericLoggerProvider : ILoggerProvider
	{
		public Func<String, ILogger> Provider { get; }

		public GenericLoggerProvider(Func<String, ILogger> provider)
		{
			Provider = provider;
		}

		public ILogger CreateLogger(String categoryName) =>
			Provider(categoryName);

		public void Dispose()
		{
			// nothing to do
		}
	}
}
