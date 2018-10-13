using System.Collections.Generic;
using ProtoBuf;
using System.IO;
using System.Threading.Tasks;
using SomeBasicFileStoreApp.Core.Commands;

namespace SomeBasicFileStoreApp.Core.Infrastructure.ProtoBuf
{
    public class AppendToFile : IAppendBatch
    {
        private readonly string _filename;

        public AppendToFile(string filename)
        {
            this._filename = filename;
        }

        public virtual async Task Batch(IEnumerable<Command> commands)
        {
            using (var fs = File.Open(_filename, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                Serializer.Serialize(fs, commands);
                await fs.FlushAsync();
            }
        }

        public virtual Task<IEnumerable<Command>> ReadAll()
        {
            using (var fs = File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Task.FromResult(Serializer.Deserialize<IEnumerable<Command>>(fs));
            }
        }
    }
}
