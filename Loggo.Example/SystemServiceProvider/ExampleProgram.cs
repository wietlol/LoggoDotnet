using Loggo.Api;
using Loggo.Core;
using Loggo.Core.Factories;
using Loggo.Core.Loggers;

namespace Loggo.Example.SystemServiceProvider
{
	public class ExampleProgram
	{
		private ILogger Logger { get; }
		private ProxyCachingLoggerFactory ProxyCachingLoggerFactory { get; }
		public ExampleTransientService ServiceA { get; }
		public ExampleSingletonService ServiceB { get; }
		
		public ExampleProgram(ILogger logger, ProxyCachingLoggerFactory proxyCachingLoggerFactory, ExampleTransientService serviceA, ExampleSingletonService serviceB)
		{
			Logger = new ScopedSourceLogger(logger, source => source.Plus("ExampleProgram"));
			ProxyCachingLoggerFactory = proxyCachingLoggerFactory;
			ServiceA = serviceA;
			ServiceB = serviceB;
		}

		public void Main()
		{
			Logger.LogInformation(new EventId(0, "program started"), new {});

			// logs made within this scope will have a different sequenceId
			using (ProxyCachingLoggerFactory.CreateInternalLogger())
			{
				ServiceA.DoThings();
			}
			
			// logs made within this scope will have a different sequenceId
			// logs made within this scope will include the "ImportantPart" source
			using (ProxyCachingLoggerFactory.CreateInternalLogger(logger => new ScopedSourceLogger(logger, source => source.Plus("ImportantPart"))))
			{
				ServiceB.DoThings();
			}
			
			Logger.LogInformation(new EventId(3, "program finished"), new {});
		}
	}
}
