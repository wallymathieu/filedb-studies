using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;


namespace SomeBasicFileStoreApp.Tests
{
    public class FakeAppendToFile:IAppendBatch
	{
        private readonly ConcurrentQueue<Command[]> batches = new ConcurrentQueue<Command[]>();
        public void Batch(IEnumerable<Command> commands)
        {
            batches.Enqueue(commands.ToArray());
            Thread.Sleep(100);
        }

        public IEnumerable<Command[]> Batches()
        {
            return batches.ToArray();
        }
	}
}