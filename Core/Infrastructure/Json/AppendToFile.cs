using System.Collections.Generic;
using System.IO;
using SomeBasicFileStoreApp.Core.Commands;
namespace SomeBasicFileStoreApp.Core.Infrastructure.Json
{
    public class AppendToFile : IAppendBatch
    {
        private JsonConvertCommands c;
        private readonly string v;

        public AppendToFile(string v)
        {
            this.v = v;
            this.c = new JsonConvertCommands();
        }

        public virtual void Batch(IEnumerable<Command> commands)
        {
            using (var fs = File.Open(v, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var w = new StreamWriter(fs))
            {
                w.WriteLine(c.Serialize(commands));
                fs.Flush();
            }
        }

        public virtual IEnumerable<Command> ReadAll()
        {
            using (var fs = File.Open(v, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var r = new StreamReader(fs))
            {
                string line;
                while (null != (line = r.ReadLine()))
                {
                    foreach (var command in c.Deserialize<IEnumerable<Command>>(line))
                    {
                        yield return command;
                    }
                }
            }
        }
    }
}
