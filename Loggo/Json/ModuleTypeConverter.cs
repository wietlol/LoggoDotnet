using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Loggo.Json
{
	public class ModuleTypeConverter : JsonConverter<Module>
	{
		public override void WriteJson(JsonWriter writer, Module value, JsonSerializer serializer) =>
			writer.WriteValue("System.Reflection.Module (truncated property because of noise data)");

		public override Module ReadJson(JsonReader reader, Type objectType, Module existingValue, Boolean hasExistingValue, JsonSerializer serializer) =>
			throw new NotImplementedException();
	}
}
