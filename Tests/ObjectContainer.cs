using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;

namespace SomeBasicFileStoreApp.Tests
{
    internal class ObjectContainer:IDisposable
	{
		private ICommandHandler[] handlers;
        private PersistCommandsHandler _persistToFile;
		private readonly IRepository _repository = new Repository();
        private readonly FakeAppendToFile _fakeAppendToFile;
		
        public ObjectContainer()
		{
            _fakeAppendToFile = new FakeAppendToFile();
            _persistToFile = new PersistCommandsHandler(_fakeAppendToFile);
			handlers =  new ICommandHandler[] {
                new RepositoryCommandHandler(_repository),
                _persistToFile
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

		public IEnumerable<ICommandHandler> GetAllHandlers()
		{
			return handlers;
		}

        public void Dispose()
        {
            _persistToFile.Stop();
        }

        public IEnumerable<Command[]> BatchesPersisted()
        {
            return _fakeAppendToFile.Batches();
        }
	}
}