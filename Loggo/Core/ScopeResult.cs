using System;
using Loggo.Api;

namespace Loggo.Core
{
	public class ScopeResult : IScopeResult
	{
		public LogSource Source { get; }
		public DateTime Start { get; }
		public DateTime? End { get; }

		public Int64? ElapsedMilliseconds => (Int64?) (End - Start)?.TotalMilliseconds;
		public String Message => $"Scope '{Source}' ran for {ElapsedMilliseconds}ms, starting at {Start}.";

		public ScopeResult(LogSource source, DateTime start, DateTime? end)
		{
			Source = source;
			Start = start;
			End = end;
		}
	}
}
