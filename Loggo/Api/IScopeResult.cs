using System;

namespace Loggo.Api
{
	public interface IScopeResult
	{
		LogSource Source { get; }
		DateTime Start { get; }
		DateTime? End { get; }
	}
}
