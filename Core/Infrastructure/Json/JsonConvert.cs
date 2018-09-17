﻿using Newtonsoft.Json;
using SomeBasicFileStoreApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SomeBasicFileStoreApp.Core.Infrastructure.Json
{
    public class JsonConvertCommands
    {
        private SerializationBinder _binder;
        public JsonConvertCommands()
        {
            _binder = new ShortNameSerializationBinder(typeof(Command));
        }

        public T Deserialize<T>(string val)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Binder = _binder };
            return JsonConvert.DeserializeObject<T>(val, settings);
        }
        public string Serialize<T>(T obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, Binder = _binder };
            return JsonConvert.SerializeObject(obj, settings);
        }
        public class ShortNameSerializationBinder : SerializationBinder
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

            public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
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

            public override Type BindToType(string assemblyName, string typeName)
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