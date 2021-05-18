using System;
using Loggo.Api;

namespace Loggo.Core
{
	public class ScopeResult : IScopeResult
	{
		public LogSource Source { get; }
		public EventId EventId { get; }
		public DateTime Start { get; }
		public DateTime? End { get; }

		public Int64? ElapsedMilliseconds => (Int64?) (End - Start)?.TotalMilliseconds;
		public String Message => $"Scope '{Source}' ran for {ElapsedMilliseconds}ms, starting at {Start}.";

		public ScopeResult(
			LogSource source,
			EventId eventId,
			DateTime start,
			DateTime? end)
		{
			Source = source;
			EventId = eventId;
			Start = start;
			End = end;
		}

		public override String ToString() =>
			Message;
	}
}
