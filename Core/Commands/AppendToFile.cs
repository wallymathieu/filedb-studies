using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace SomeBasicFileStoreApp.Core.Commands
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

        public virtual void Batch(IEnumerable<Command> commands)
		{
			
            using (var fs = File.Open(v, FileMode.Append, FileAccess.Write, FileShare.Read))
			{
				Serializer.Serialize(fs, commands);
			}
		}

        public virtual IEnumerable<Command> ReadAll()
		{
			using (var fs = File.Open(v, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Serializer.Deserialize<IEnumerable<Command>>(fs);
			}
		}
	}
}
