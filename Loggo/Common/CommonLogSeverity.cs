using System;
using Loggo.Api;

namespace Loggo.Common
{
	public class CommonLogSeverity : ILogSeverity
	{
		public static ILogSeverity None { get; } = new CommonLogSeverity("None", 7.0);
		public static ILogSeverity Critical { get; } = new CommonLogSeverity("Critical", 6.0);
		public static ILogSeverity Error { get; } = new CommonLogSeverity("Error", 5.0);
		public static ILogSeverity Warning { get; } = new CommonLogSeverity("Warning", 4.0);
		public static ILogSeverity Information { get; } = new CommonLogSeverity("Information", 3.0);
		public static ILogSeverity Debug { get; } = new CommonLogSeverity("Debug", 2.0);
		public static ILogSeverity Trace { get; } = new CommonLogSeverity("Trace", 1.0);

		public String Name { get; }
		public Double Value { get; }

		public CommonLogSeverity(String name, Double value)
		{
			Name = name;
			Value = value;
		}
	}
}
