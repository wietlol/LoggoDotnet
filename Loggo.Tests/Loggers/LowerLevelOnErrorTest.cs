using System.Collections.Generic;
using Loggo.Common;
using Loggo.Core;
using Loggo.Core.Loggers;
using Xunit;
using Xunit.Abstractions;

namespace Loggo.Tests.Loggers
{
	public class LowerLevelOnErrorTest
	{
		[Fact]
		public void AssertThat_Errors_CauseTracesToBeLoggedAsWell()
		{
			IList<CommonLog> logList = new List<CommonLog>();
			var filter = new TriggerableLevelFilter<CommonLog>(
				CommonLogSeverity.Information,
				CommonLogSeverity.Error,
				CommonLogSeverity.Trace
			);
			using var logger = new FilteredLogger<CommonLog>(
				new GenericLogger<CommonLog>(log => logList.Add(log)),
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
