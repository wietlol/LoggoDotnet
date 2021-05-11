using System;
using Loggo.Api;
using Loggo.Core;
using Loggo.Core.Factories;
using Loggo.Core.Loggers;
using Loggo.Core.Streams;
using Loggo.Json;
using Microsoft.Extensions.DependencyInjection;
using static Loggo.Core.LoggerDsl;

namespace Loggo.Example.SystemServiceProvider
{
	public static class DependencyInjection
	{
		public static IServiceProvider ServiceProvider { get; } = BuildServiceProvider();

		public static IServiceProvider BuildServiceProvider() =>
			new ServiceCollection()
				// aliasing the ProxyCachingLoggerFactory as transient ILoggerFactory
				.AddTransient<ILoggerFactory>(provider =>
					provider.GetRequiredService<ProxyCachingLoggerFactory>())

				// wrapping the logger with a filter
				// it will filter with a minimum log level of "Information"
				// if there is a log with a minimum log level of "Error"
				// then all previous logs and following logs with a minimum level of "Trace" will also be logged
				// new filters (and therefore an empty log cache) will be made because the factory is transient 
				.Decorate<ILoggerFactory>(factory =>
					LoggerFactory(() =>
					{
						var filter = new TriggerableLevelFilter(
							CommonLogSeverity.Information,
							CommonLogSeverity.Error,
							CommonLogSeverity.Trace);
						return new FilteredLogger(factory.CreateLogger(), filter.Apply);
					})
				)
				// wrapping the logger with a try-catch
				// when an error is caused by a log, that error is logged instead
				.Decorate<ILoggerFactory>(factory =>
					LoggerFactory(() => new FailsafeLogger(factory.CreateLogger()))
				)

				// registering the loggers
				.AddSingleton(provider =>
					// provide a proxy layer for loggers so that loggers that were created earlier,
					//    will still use the appropriate logger for the current execution context
					new ProxyCachingLoggerFactory(
						LoggerFactory(() =>
						{
							// create a sequenceId, a unique identifier to correlate logs within the same execution 
							var sequenceId = Guid.NewGuid();
							return new ScopedSequenceLogger(
								// create a source to identify where logs came from once they are collected in a central database
								new ScopedSourceLogger(
									// change type of pipeline
									new OutputLogger(
										// format structured logs as json
										JsonLogOutput.CreateLogOutputStream(
											// write json logs to the console
											new ConsoleOutputStream()
										)
									),
									source => source.Plus("SystemServiceProvider-Example")
								),
								guid => sequenceId
							);
						})
					)
				)

				// allowing services to ask for an ILogger
				.AddScoped(provider =>
					provider.GetRequiredService<ILoggerFactory>()
						.CreateLogger())

				// providing other services
				.AddTransient<ExampleProgram>()
				.AddTransient<ExampleTransientService>()
				.AddTransient<ExampleSingletonService>()
				.BuildServiceProvider();
	}
}
