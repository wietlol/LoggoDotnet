using System;

namespace Loggo.Api
{
	public interface ILogSeverity
	{
		String Name { get; }
		Double Value { get; }
	}
}
