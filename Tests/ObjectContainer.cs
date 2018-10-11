using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;
using System.Linq;

namespace SomeBasicFileStoreApp.Tests
{
    internal class ObjectContainer : IDisposable
    {
        private CommandHandler[] handlers;
        private PersistCommandsHandler _persistToFile;
        private readonly IRepository _repository = new Repository();
        private readonly FakeAppendToFile _fakeAppendToFile;

        public ObjectContainer()
        {
            _fakeAppendToFile = new FakeAppendToFile();
            _persistToFile = new PersistCommandsHandler(new []{_fakeAppendToFile});
            handlers = new CommandHandler[] {
                c => c.Handle(_repository),
                _persistToFile.Handle
            };
        }

        public void Boot()
        {
            _persistToFile.Start();
        }

        public IRepository GetRepository()
        {
            return _repository;
        }

        public void Dispose()
        {
            _persistToFile.Stop();
        }

        public IEnumerable<Command[]> BatchesPersisted()
        {
            return _fakeAppendToFile.Batches();
        }

        public void HandleAll(IEnumerable<Command> commands)
        {
            foreach (var command in commands)
            {
                foreach (var handler in handlers)
                {
                    handler.Invoke(command);
                }
            }
        }
    }
}