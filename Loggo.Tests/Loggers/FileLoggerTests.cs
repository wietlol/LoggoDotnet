using System;
using System.IO;
using Loggo.Api;
using Loggo.Core;
using Loggo.Core.Loggers;
using Loggo.Core.Streams;
using Xunit;

namespace Loggo.Tests.Loggers
{
	public class FileLoggerTests
	{
		[Fact]
		public void AssertThat_FileLogger_CreatesNewFiles()
		{
			var file = new FileInfo("./test.log");

			try
			{
				Assert.False(file.Exists, "file already existed");

				using (var logger = new OutputLogger(
					new ConverterOutputStream<LogEntry, String>(
						new FileOutputStream(() => file),
						entry => entry.ToString()
					)
				))
				{
					logger.LogInformation(new EventId(0, "test"), new {});
				}
				
				file.Refresh();
				Assert.True(file.Exists, "file wasn't created");
			}
			finally
			{
				file.Delete();
			}
		}
	}
}
