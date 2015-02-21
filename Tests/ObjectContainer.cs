using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core;

namespace SomeBasicFileStoreApp.Tests
{
	internal class ObjectContainer 
	{
		private ICommandHandler[] handlers;
		private readonly IRepository _repository = new Repository();
		public ObjectContainer()
		{
			handlers = new ICommandHandler[] {
				new AddCustomerCommandHandler(_repository),
				new AddOrderCommandHandler(_repository),
				new AddProductCommandHandler(_repository)
			};
        }
		public IRepository GetRepository()
		{
			return _repository;
		}

		public IEnumerable<ICommandHandler> GetAllHandlers()
		{
			return handlers;
		}
	}
}