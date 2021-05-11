using System;
using Loggo.Api;
using Loggo.Core.Loggers;

namespace Loggo.Core
{
	public class TriggerableLevelFilter
	{
		public Double TriggerLevel { get; }
		public Double TargetLevel { get; }

		private Double CurrentLevel { get; set; }

		public TriggerableLevelFilter(ILogSeverity startLevel, ILogSeverity triggerLevel, ILogSeverity targetLevel)
			: this(startLevel.Value, triggerLevel.Value, targetLevel.Value)
		{
			// empty proxy constructor
		}

		public TriggerableLevelFilter(Double startLevel, Double triggerLevel, Double targetLevel)
		{
			CurrentLevel = startLevel;
			TriggerLevel = triggerLevel;
			TargetLevel = targetLevel;
		}

		public Boolean Apply(LogEntry log, ILogFilter logFilter)
		{
			Double logValue = log.Severity.Value;
			if (logValue >= TriggerLevel && TargetLevel < CurrentLevel)
			{
				CurrentLevel = TargetLevel;
				logFilter.FilterRulesChanged();
			}

			return logValue >= CurrentLevel;
		}
	}
}
