using System.Collections.Generic;
using System.IO;
using SomeBasicFileStoreApp.Core.Commands;
namespace SomeBasicFileStoreApp.Core.Infrastructure.Json
{
    public class AppendToFile : IAppendBatch
    {
        private readonly JsonConvertCommands _converter;
        private readonly string _filename;

        public AppendToFile(string filename)
        {
            _filename = filename;
            _converter = new JsonConvertCommands();
        }

        public virtual void Batch(IEnumerable<Command> commands)
        {
            using (var fs = File.Open(_filename, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var w = new StreamWriter(fs))
            {
                w.WriteLine(_converter.Serialize(commands));
                fs.Flush();
            }
        }

        public virtual IEnumerable<Command> ReadAll()
        {
            using (var fs = File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var r = new StreamReader(fs))
            {
                string line;
                while (null != (line = r.ReadLine()))
                {
                    foreach (var command in _converter.Deserialize<IEnumerable<Command>>(line))
                    {
                        yield return command;
                    }
                }
            }
        }
    }
}
