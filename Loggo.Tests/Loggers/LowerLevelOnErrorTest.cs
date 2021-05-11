using System.Collections.Generic;
using Loggo.Api;
using Loggo.Core;
using Loggo.Core.Loggers;
using Loggo.Core.Streams;
using Xunit;

namespace Loggo.Tests.Loggers
{
	public class LowerLevelOnErrorTest
	{
		[Fact]
		public void AssertThat_Errors_CauseTracesToBeLoggedAsWell()
		{
			IList<LogEntry> logList = new List<LogEntry>();
			var filter = new TriggerableLevelFilter(
				CommonLogSeverity.Information,
				CommonLogSeverity.Error,
				CommonLogSeverity.Trace
			);
			var outputStream = new ListOutputStream<LogEntry>(logList);
			using var logger = new FilteredLogger(
				new OutputLogger(outputStream),
				filter.Apply
			);

			logger.LogTrace(new EventId(1, ""), new { });
			// assert nothing has been written
			Assert.Equal(0, logList.Count);

			logger.LogInformation(new EventId(2, ""), new { });
			// assert information has been written
			Assert.Equal(1, logList.Count);
			Assert.Equal(2, logList[0].EventId.Id);

			logger.LogError(new EventId(3, ""), new { });
			// assert information, trace and error have been written
			Assert.Equal(3, logList.Count);
			Assert.Equal(2, logList[0].EventId.Id);
			Assert.Equal(1, logList[1].EventId.Id);
			Assert.Equal(3, logList[2].EventId.Id);

			logger.LogTrace(new EventId(4, ""), new { });
			// assert information, trace, error an trace have been written
			Assert.Equal(4, logList.Count);
			Assert.Equal(2, logList[0].EventId.Id);
			Assert.Equal(1, logList[1].EventId.Id);
			Assert.Equal(3, logList[2].EventId.Id);
			Assert.Equal(4, logList[3].EventId.Id);
		}
	}
}
