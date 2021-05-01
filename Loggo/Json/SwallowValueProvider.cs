using System;
using Newtonsoft.Json.Serialization;

namespace Loggo.Json
{
	public class SwallowValueProvider : IValueProvider
	{
		public IValueProvider ValueProvider { get; }
		public Object DefaultValue { get; }

		public SwallowValueProvider(IValueProvider valueProvider, Object defaultValue)
		{
			ValueProvider = valueProvider;
			DefaultValue = defaultValue;
		}

		public void SetValue(Object target, Object value)
		{
			try
			{
				ValueProvider.SetValue(target, value);
			}
			catch
			{
				// swallow
			}
		}

		public Object GetValue(Object target)
		{
			try
			{
				return ValueProvider.GetValue(target);
			}
			catch
			{
				return DefaultValue;
			}
		}
	}
}
