using System.IO;
using Loggo.Core.Loggers;
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
				using (var logger = new FileLogger(() => file))
					logger.Log("<log>");
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
