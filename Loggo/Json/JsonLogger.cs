using System;
using System.Collections.Generic;
using Loggo.Api;
using Loggo.Core.Loggers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Loggo.Json
{
	public static class JsonLogger
	{
		public static JsonSerializerSettings DefaultSettings => new JsonSerializerSettings
		{
			ContractResolver = new SwallowContractResolver
			{
				NamingStrategy = new SnakeCaseNamingStrategy(),
			},
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			Error = (se, ev) => ev.ErrorContext.Handled = true,
			Converters = new List<JsonConverter>
			{
				new ModuleTypeConverter(),
				new ExceptionTypeConverter(),
			},
		};

		public static ILogger<Object> CreateLogger(ILogger<String> logger, JsonSerializerSettings settings = null) =>
			new ConverterLogger<Object, String>(
				logger,
				it => JsonConvert.SerializeObject(it, settings ?? DefaultSettings)
			);
	}
}
