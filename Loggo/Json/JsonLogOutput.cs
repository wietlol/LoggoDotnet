using System;
using System.Collections.Generic;
using Loggo.Api;
using Loggo.Core.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Loggo.Json
{
	public static class JsonLogOutput
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

		public static ILogOutputStream<Object> CreateLogOutputStream(ILogOutputStream<String> outputStream, JsonSerializerSettings settings = null) =>
			new ConverterOutputStream<Object, String>(
				outputStream,
				it => JsonConvert.SerializeObject(it, settings ?? DefaultSettings)
			);
	}
}
