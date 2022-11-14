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
            await using var fs = File.Open(_filename, FileMode.Append, FileAccess.Write, FileShare.Read);
            await using var w = new StreamWriter(fs);
            await w.WriteLineAsync(_converter.Serialize(commands));
            await fs.FlushAsync();
        }

        public virtual async Task<IEnumerable<Command>> ReadAll()
        {
            await using var fs = File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var r = new StreamReader(fs);
            var commands = new List<Command>();
            while (await r.ReadLineAsync() is { } line)
            {
                commands.AddRange(_converter.Deserialize<IEnumerable<Command>>(line));
            }
            return commands;
        }
    }
}
