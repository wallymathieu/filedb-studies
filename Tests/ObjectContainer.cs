using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;

namespace SomeBasicFileStoreApp.Tests
{
    internal class ObjectContainer:IDisposable
	{
		private ICommandHandler[] handlers;
        private PersistToFileHandler _persistToFile;
		private readonly IRepository _repository = new Repository();
		public ObjectContainer()
		{
            _persistToFile = new PersistToFileHandler(new AppendToFile("testfile.db"), 100);
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
	}
}