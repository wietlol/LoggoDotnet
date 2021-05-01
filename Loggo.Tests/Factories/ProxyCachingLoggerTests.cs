using System;
using System.Collections.Generic;
using Loggo.Api;
using Loggo.Common;
using Loggo.Core.Factories;
using Loggo.Core.Loggers;
using Loggo.Json;
using Xunit;
using Xunit.Abstractions;

namespace Loggo.Tests.Factories
{
	public class ProxyCachingLoggerTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public ProxyCachingLoggerTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void AssertThat_ProxyCachingLogger_AlwaysProxiesToTheLatestLogger()
		{
			var logList = new List<CommonLog>();
			var listLogger = new GenericLogger<CommonLog>(log => logList.Add(log));
			var factory = new ProxyCachingLoggerFactory<CommonLog>(
				new GenericLoggerFactory<CommonLog>(() =>
				{
					var sequenceId = Guid.NewGuid();
					return new ScopedSequenceLogger(listLogger, guid => sequenceId);
				})
			);
			ILoggerFactory<CommonLog> genericFactory = factory;

			Assert.Empty(logList);

			// without internal logger, it will make one on-demand
			using ILogger<CommonLog> firstLogger = new ScopedSourceLogger(genericFactory.CreateLogger(), source => source.Plus("outer-logger"));
			firstLogger.LogInformation(new EventId(1, "log-1"), new { });
			firstLogger.LogInformation(new EventId(1, "log-1"), new { });
			// assert both logs are registered
			Assert.Equal(2, logList.Count);
			// assert both logs have the same sequence and are not default sequence
			Assert.NotEqual(Guid.Empty, logList[0].SequenceId);
			Assert.Equal(logList[0].SequenceId, logList[1].SequenceId);

			// create a new scoped internal logger
			using (ILogger<CommonLog> _ = factory.CreateInternalLogger())
			{
				using ILogger<CommonLog> scopedLogger = new ScopedSourceLogger(genericFactory.CreateLogger(), source => source.Plus("scoped-logger-1"));

				scopedLogger.LogInformation(new EventId(2, "log-2"), new { });
				scopedLogger.LogInformation(new EventId(2, "log-2"), new { });
				// assert both logs are registered
				Assert.Equal(4, logList.Count);
				// assert both logs have the same sequence
				//	and are not default sequence
				//	and are not equal to previous sequence
				Assert.NotEqual(Guid.Empty, logList[2].SequenceId);
				Assert.Equal(logList[2].SequenceId, logList[3].SequenceId);
				Assert.NotEqual(logList[0].SequenceId, logList[2].SequenceId);

				firstLogger.LogInformation(new EventId(3, "log-3"), new { });
				firstLogger.LogInformation(new EventId(3, "log-3"), new { });
				// assert both logs are registered
				Assert.Equal(6, logList.Count);
				// assert both logs have the same sequence
				//	and are equal to the logs of the scoped logger
				Assert.Equal(logList[4].SequenceId, logList[5].SequenceId);
				Assert.Equal(logList[2].SequenceId, logList[4].SequenceId);
			}

			firstLogger.LogInformation(new EventId(4, "log-4"), new { });
			firstLogger.LogInformation(new EventId(4, "log-4"), new { });
			// assert both logs are registered
			Assert.Equal(8, logList.Count);
			// assert both logs have the same sequence
			//	and are equal to the logs of the outer scope
			Assert.Equal(logList[6].SequenceId, logList[7].SequenceId);
			Assert.Equal(logList[0].SequenceId, logList[6].SequenceId);

			// create a new scoped internal logger
			using (ILogger<CommonLog> _ = factory.CreateInternalLogger())
			{
				using ILogger<CommonLog> scopedLogger = new ScopedSourceLogger(genericFactory.CreateLogger(), source => source.Plus("scoped-logger-2"));

				scopedLogger.LogInformation(new EventId(5, "log-5"), new { });
				scopedLogger.LogInformation(new EventId(5, "log-5"), new { });
				// assert both logs are registered
				Assert.Equal(10, logList.Count);
				// assert both logs have the same sequence
				//	and are not default sequence
				//	and are not equal to previous sequence
				Assert.NotEqual(Guid.Empty, logList[2].SequenceId);
				Assert.Equal(logList[8].SequenceId, logList[9].SequenceId);
				Assert.NotEqual(logList[0].SequenceId, logList[8].SequenceId);

				firstLogger.LogInformation(new EventId(6, "log-6"), new { });
				firstLogger.LogInformation(new EventId(6, "log-6"), new { });
				// assert both logs are registered
				Assert.Equal(12, logList.Count);
				// assert both logs have the same sequence
				//	and are equal to the logs of the scoped logger
				Assert.Equal(logList[10].SequenceId, logList[11].SequenceId);
				Assert.Equal(logList[8].SequenceId, logList[10].SequenceId);
			}

			// std::out
			foreach (CommonLog log in logList)
				_testOutputHelper.WriteLine($"{log.EventId.Name} :: {log.SequenceId} :: {log.Source}");
			var stdLogger = new GenericLogger<String>(log => _testOutputHelper.WriteLine(log));
			ILogger<Object> jsonLogger = JsonLogger.CreateLogger(stdLogger);
			jsonLogger.LogAll(logList);
		}
	}
}
