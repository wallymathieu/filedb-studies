using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeBasicFileStoreApp.Core.Infrastructure.Json
{
	public class JsonConvertCommands
	{
		public T Deserialize<T>(string val)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			return JsonConvert.DeserializeObject<T>(val, settings);
		}
		public string Serialize<T>(T obj)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			return JsonConvert.SerializeObject(obj, settings);
		}
	}
}