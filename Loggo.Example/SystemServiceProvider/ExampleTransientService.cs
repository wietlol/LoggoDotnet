using Loggo.Api;
using Loggo.Common;

namespace Loggo.Example.SystemServiceProvider
{
	public class ExampleTransientService
	{
		private ILogger<CommonLog> Logger { get; }

		public ExampleTransientService(ILogger<CommonLog> logger)
		{
			Logger = new ScopedSourceLogger(logger, source => source.Plus("ExampleTransientService"));
		}

		public void DoThings()
		{
			Logger.LogInformation(new EventId(7824123, "doing-random-things"), new {});
		}
	}
}
