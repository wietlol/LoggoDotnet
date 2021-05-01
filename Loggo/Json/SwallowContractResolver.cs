using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Loggo.Json
{
	public class SwallowContractResolver : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty property = base.CreateProperty(member, memberSerialization);
			property.ValueProvider = new SwallowValueProvider(property.ValueProvider, property.DefaultValue);
			return property;
		}
	}
}
