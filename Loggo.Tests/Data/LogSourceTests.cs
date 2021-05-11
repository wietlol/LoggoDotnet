using System;
using System.Collections.Generic;
using Loggo.Api;
using Xunit;

namespace Loggo.Tests.Data
{
	public class LogSourceTests
	{
		[Fact]
		public void AssertThat_IdenticalLogSources_AreEqual()
		{
			var source1 = new LogSource("source");
			var source2 = new LogSource("source");

			Assert.True(source1.Equals(source2), "source1.Equals(source2)");
			Assert.True(source1 == source2, "source1 == source2");
			Assert.Equal(source1, source2);
		}

		[Fact]
		public void AssertThat_SimilarLogSources_AreEqual()
		{
			var source1 = new LogSource(new[] {"source"});
			var source2 = new LogSource(new LinkedList<String>(new List<String> {"source"}));

			Assert.True(source1.Equals(source2), "source1.Equals(source2)");
			Assert.True(source1 == source2, "source1 == source2");
			Assert.Equal(source1, source2);
		}

		[Fact]
		public void AssertThat_LogSource_StringRepresentation_IsMeaningful()
		{
			var source = new LogSource(new[] {"source-1", "source-2", "source-3"});

			Assert.Equal("[source-1, source-2, source-3]", source.ToString());
		}
	}
}
