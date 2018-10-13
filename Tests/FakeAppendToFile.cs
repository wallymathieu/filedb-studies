using System.Collections.Generic;
using SomeBasicFileStoreApp.Core.Commands;
using System.Linq;
using SomeBasicFileStoreApp.Core.Infrastructure;
using System.Threading.Tasks;

namespace SomeBasicFileStoreApp.Tests
{
    public class FakeAppendToFile : IAppendBatch
    {
        private readonly IList<Command[]> _batches = new List<Command[]>();
        public async Task Batch(IEnumerable<Command> commands)
        {
            await Task.Delay(100);
            _batches.Add(commands.ToArray());
        }

        public IEnumerable<Command[]> Batches()
        {
            return _batches.ToArray();
        }

        public async Task<IEnumerable<Command>> ReadAll()
        {
            await Task.Delay(100);
            return _batches.SelectMany(b => b);
        }
    }
}