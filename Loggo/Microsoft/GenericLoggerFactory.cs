using System;
using Microsoft.Extensions.Logging;

namespace Loggo.Microsoft
{
	public class GenericLoggerFactory : ILoggerFactory
	{
		public Func<String, ILogger> Factory { get; }

		public GenericLoggerFactory(Func<String, ILogger> factory)
		{
			Factory = factory;
		}

		public ILogger CreateLogger(String categoryName) =>
			Factory(categoryName);

		public void AddProvider(ILoggerProvider provider)
		{
			// nothing to do
		}

		public void Dispose()
		{
			// nothing to do
		}
	}
}
