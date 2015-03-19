using System.Collections.Generic;
using SomeBasicFileStoreApp.Core.Commands;
using System.Linq;
using System.Threading;
using SomeBasicFileStoreApp.Core.Infrastructure;

namespace SomeBasicFileStoreApp.Tests
{
    public class FakeAppendToFile : IAppendBatch
    {
        private readonly IList<Command[]> batches = new List<Command[]>();
        public void Batch(IEnumerable<Command> commands)
        {
            batches.Add(commands.ToArray());
            Thread.Sleep(100);
        }

        public IEnumerable<Command[]> Batches()
        {
            return batches.ToArray();
        }
    }
}