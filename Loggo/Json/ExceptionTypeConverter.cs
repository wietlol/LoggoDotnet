using System;
using Newtonsoft.Json;

namespace Loggo.Json
{
	public class ExceptionTypeConverter : JsonConverter<Exception>
	{
		public override void WriteJson(JsonWriter writer, Exception value, JsonSerializer serializer) =>
			serializer.Serialize(writer, new
			{
				TypeName = value.GetType().Name,
				value.Message,
				value.Data,
				value.InnerException,
				value.StackTrace,
				value.HelpLink,
				value.Source,
				value.HResult,
				DisplayText = value.ToString(),
			});

		public override Exception ReadJson(JsonReader reader, Type objectType, Exception existingValue, Boolean hasExistingValue, JsonSerializer serializer) =>
			throw new NotImplementedException();
	}
}
