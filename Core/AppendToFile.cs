using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf.ServiceModel;
using ProtoBuf;
using System.IO;
using ProtoBuf.Meta;

namespace SomeBasicFileStoreApp.Core
{
	public class AppendToFile : Model
	{

		static AppendToFile()
		{
		}

		private string v;

		public AppendToFile(string v)
		{
			this.v = v;
		}

		public void Batch(IEnumerable<Command> commands)
		{
			
            using (var fs = File.Open(v, FileMode.Append, FileAccess.Write, FileShare.Read))
			{
				Serializer.Serialize(fs, commands);
			}
		}

		public IEnumerable<Command> ReadAll()
		{
			using (var fs = File.Open(v, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Serializer.Deserialize<IEnumerable<Command>>(fs);
			}
		}
	}
}
