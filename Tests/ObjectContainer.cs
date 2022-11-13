using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;
using Newtonsoft.Json;

namespace SomeBasicFileStoreApp.Tests
{
    internal class ObjectContainer : IDisposable
    {
        private readonly PersistCommandsHandler _persistToFile;
        private readonly IRepository _repository = new Repository();
        private readonly FakeAppendToFile _fakeAppendToFile;

        public ObjectContainer()
        {
            _fakeAppendToFile = new FakeAppendToFile();
            _persistToFile = new PersistCommandsHandler(new []{_fakeAppendToFile});
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

        public void PersistAll(IEnumerable<Command> commands)
        {
            foreach (var command in commands)
                _persistToFile.Append(command);
        }

        public void HandleAll(IEnumerable<Command> commands)
        {
            foreach (var command in commands)
            {
                var r = command.Run(_repository);
                if (!r) throw new Exception("Could not handle "+JsonConvert.SerializeObject(command));
            }
        }
    }
}