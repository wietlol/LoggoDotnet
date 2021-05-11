using Loggo.Api;
using Loggo.Core;
using Loggo.Core.Loggers;

namespace Loggo.Example.SystemServiceProvider
{
	public class ExampleSingletonService
	{
		private ILogger Logger { get; }

		public ExampleSingletonService(ILogger logger)
		{
			Logger = new ScopedSourceLogger(logger, source => source.Plus("ExampleSingletonService"));
		}

		public void DoThings()
		{
			Logger.LogInformation(new EventId(918234, "doing-more-random-things"), new {});
		}
	}
}
