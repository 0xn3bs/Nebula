using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public class SerializationHelper
    {
        public static JsonSerializerSettings GetJsonDeserializationSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            settings.Binder = TypeRegistry.Instance;
            return settings;
        }

        public static string SerializeToJson(object rpc)
        {
            JsonSerializerSettings settings = GetJsonSerializationSettings();
            var serializedJson = JsonConvert.SerializeObject(rpc, settings);
            return serializedJson;
        }

        private static JsonSerializerSettings GetJsonSerializationSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            return settings;
        }

        public static T DeserializeFromJson<T>(string value)
        {
            var jsonSerializationSettings = GetJsonDeserializationSettings();
            return JsonConvert.DeserializeObject<T>(value, jsonSerializationSettings);
        }
    }
}
