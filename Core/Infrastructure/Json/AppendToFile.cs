using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        public virtual async Task Batch(IEnumerable<Command> commands)
        {
            using (var fs = File.Open(_filename, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var w = new StreamWriter(fs))
            {
                w.WriteLine(_converter.Serialize(commands));
                await fs.FlushAsync();
            }
        }

        public virtual async Task<IEnumerable<Command>> ReadAll()
        {
            using (var fs = File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var r = new StreamReader(fs))
            {
                var commands = new List<Command>();
                string line;
                while (null != (line = await r.ReadLineAsync()))
                {
                    commands.AddRange(_converter.Deserialize<IEnumerable<Command>>(line));
                }
                return commands;
            }
        }
    }
}
