using Loggo.Api;
using Loggo.Common;

namespace Loggo.Example.SystemServiceProvider
{
	public class ExampleSingletonService
	{
		private ILogger<CommonLog> Logger { get; }

		public ExampleSingletonService(ILogger<CommonLog> logger)
		{
			Logger = new ScopedSourceLogger(logger, source => source.Plus("ExampleSingletonService"));
		}

		public void DoThings()
		{
			Logger.LogInformation(new EventId(918234, "doing-more-random-things"), new {});
		}
	}
}
