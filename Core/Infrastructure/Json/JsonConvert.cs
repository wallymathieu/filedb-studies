using Newtonsoft.Json;
using SomeBasicFileStoreApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace SomeBasicFileStoreApp.Core.Infrastructure.Json
{
    public class JsonConvertCommands
    {
        private readonly JsonSerializerSettings _settings;

        public JsonConvertCommands()
        {
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto, 
                SerializationBinder = new ShortNameSerializationBinder(typeof(Command))
            };
        }

        public T Deserialize<T>(string val)
        {

            return JsonConvert.DeserializeObject<T>(val, _settings);
        }
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        private class ShortNameSerializationBinder : ISerializationBinder
        {
            private readonly Type _type;
            private readonly IDictionary<string, Type> _types;

            public ShortNameSerializationBinder(Type type)
            {
                _type = type;
                _types = type.Assembly.GetTypes()
                    .Where(type.IsAssignableFrom)
                    .ToDictionary(t => t.Name, t => t);
            }

            public void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                if (_type.IsAssignableFrom(serializedType))
                {
                    assemblyName = null;
                    typeName = serializedType.Name;
                }
                else
                {
                    assemblyName = serializedType.Assembly.FullName;
                    typeName = serializedType.FullName;
                }
            }

            public Type BindToType(string assemblyName, string typeName)
            {
                if (_types.ContainsKey(typeName))
                {
                    return _types[typeName];
                }

                return Type.GetType($"{typeName}, {assemblyName}", true);
            }
        }
    }
}