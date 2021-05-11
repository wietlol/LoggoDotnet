using Loggo.Api;
using Loggo.Core;
using Loggo.Core.Loggers;

namespace Loggo.Example.SystemServiceProvider
{
	public class ExampleTransientService
	{
		private ILogger Logger { get; }

		public ExampleTransientService(ILogger logger)
		{
			Logger = new ScopedSourceLogger(logger, source => source.Plus("ExampleTransientService"));
		}

		public void DoThings()
		{
			Logger.LogInformation(new EventId(7824123, "doing-random-things"), new {});
		}
	}
}
