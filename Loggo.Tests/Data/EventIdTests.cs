using Loggo.Common;
using Xunit;

namespace Loggo.Tests.Data
{
	public class EventIdTests
	{
		[Fact]
		public void AssertThat_IdenticalEventIds_AreEqual()
		{
			var eventId1 = new EventId(42, "event");
			var eventId2 = new EventId(42, "event");

			Assert.True(eventId1.Equals(eventId2), "eventId1.Equals(eventId2)");
			Assert.True(eventId1 == eventId2, "eventId1 == eventId2");
			Assert.Equal(eventId1, eventId2);
		}

		[Fact]
		public void AssertThat_EventId_StringRepresentation_IsMeaningful()
		{
			var eventId = new EventId(42, "event");

			Assert.Equal("<42::event>", eventId.ToString());
		}
	}
}
