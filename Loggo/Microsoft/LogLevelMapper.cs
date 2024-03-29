using System;
using Loggo.Api;
using Loggo.Core;
using Microsoft.Extensions.Logging;

namespace Loggo.Microsoft
{
	public static class LogLevelMapper
	{
		public static ILogSeverity Map(LogLevel value) =>
			value switch
			{
				LogLevel.Trace => CommonLogSeverity.Trace,
				LogLevel.Debug => CommonLogSeverity.Debug,
				LogLevel.Information => CommonLogSeverity.Information,
				LogLevel.Warning => CommonLogSeverity.Warning,
				LogLevel.Error => CommonLogSeverity.Error,
				LogLevel.Critical => CommonLogSeverity.Critical,
				LogLevel.None => CommonLogSeverity.None,
				_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
			};
	}
}
