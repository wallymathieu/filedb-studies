using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;
using System.Linq;
using System.Collections.Concurrent;
namespace SomeBasicFileStoreApp.Tests
{
    public class FakeAppendToFile:IAppendBatch
	{
        private readonly ConcurrentStack<Command[]> batches = new ConcurrentStack<Command[]>();
        public void Batch(IEnumerable<Command> commands)
        {
            batches.Push(commands.ToArray());
        }
        public IEnumerable<Command[]> Batches(){
            return batches.ToArray();
        }
	}
}