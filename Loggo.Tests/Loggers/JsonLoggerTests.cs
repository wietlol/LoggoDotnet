using System;
using Loggo.Api;
using Loggo.Common;
using Loggo.Core.Loggers;
using Loggo.Json;
using Xunit;
using Xunit.Abstractions;

namespace Loggo.Tests.Loggers
{
	public class JsonLoggerTests
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public JsonLoggerTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void AssertThat_Exceptions_CanBeConvertedToJson()
		{
			var stdLogger = new GenericLogger<String>(log => _testOutputHelper.WriteLine(log));
			ILogger<Object> logger = JsonLogger.CreateLogger(stdLogger);

			try
			{
				throw new Exception("test");
			}
			catch (Exception ex)
			{
				logger.LogError(new EventId(0, ""), new { }, ex);
			}
		}
	}
}
