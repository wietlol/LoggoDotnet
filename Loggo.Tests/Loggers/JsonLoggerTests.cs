using System;
using Loggo.Api;
using Loggo.Core;
using Loggo.Core.Loggers;
using Loggo.Core.Streams;
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
			ILogOutputStream<Object> outputStream = JsonLogOutput.CreateLogOutputStream(
				new GenericOutputStream<String>(log => _testOutputHelper.WriteLine(log))
			);
			using var logger = new OutputLogger(outputStream);

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
