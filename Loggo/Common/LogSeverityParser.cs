using System;
using System.Globalization;
using Loggo.Api;

namespace Loggo.Common
{
	public class LogSeverityParser
	{
		public ILogSeverity Parse(String text, ILogSeverity defaultValue) =>
			Parse(text) ?? defaultValue;

		public ILogSeverity Parse(String text)
		{
			if (String.IsNullOrWhiteSpace(text))
				return null;

			if (Double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture.NumberFormat, out Double severity))
				return severity switch
				{
					7.0 => CommonLogSeverity.None,
					6.0 => CommonLogSeverity.Critical,
					5.0 => CommonLogSeverity.Error,
					4.0 => CommonLogSeverity.Warning,
					3.0 => CommonLogSeverity.Information,
					2.0 => CommonLogSeverity.Debug,
					1.0 => CommonLogSeverity.Trace,
					_ => new CommonLogSeverity($"unknown({severity})", severity),
				};

			return text.ToLowerInvariant() switch
			{
				"none" => CommonLogSeverity.None,
				"critical" => CommonLogSeverity.Critical,
				"err" => CommonLogSeverity.Error,
				"error" => CommonLogSeverity.Error,
				"warn" => CommonLogSeverity.Warning,
				"warning" => CommonLogSeverity.Warning,
				"info" => CommonLogSeverity.Information,
				"information" => CommonLogSeverity.Information,
				"debug" => CommonLogSeverity.Debug,
				"trace" => CommonLogSeverity.Trace,
				_ => null,
			};
		}
	}
}
