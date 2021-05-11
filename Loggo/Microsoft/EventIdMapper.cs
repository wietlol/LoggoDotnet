using Loggo.Api;

namespace Loggo.Microsoft
{
	public static class EventIdMapper
	{
		public static EventId Map(global::Microsoft.Extensions.Logging.EventId value) =>
			new EventId(
				value.Id,
				value.Name ?? "unknown"
			);
	}
}
