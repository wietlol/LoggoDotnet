using Loggo.Common;

namespace Loggo.Microsoft
{
	public class EventIdMapper
	{
		public EventId Map(global::Microsoft.Extensions.Logging.EventId value) =>
			new EventId(
				value.Id,
				value.Name
			);
	}
}
