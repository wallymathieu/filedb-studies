using System.Collections.Generic;
using ProtoBuf;
using System.IO;
using SomeBasicFileStoreApp.Core.Commands;

namespace SomeBasicFileStoreApp.Core.Infrastructure.ProtoBuf
{
    public class AppendToFile : Model, IAppendBatch
    {
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
                fs.Flush();
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
