using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicFileStoreApp.Core;
using System.Linq;
using System;
using System.Collections.Generic;
namespace SomeBasicFileStoreApp.Tests
{
	[TestFixture]
	public class CustomerDataTests
	{
		private IRepository _repository;

		[Test]
		public void CanGetCustomerById()
		{
			Assert.IsNotNull(_repository.GetCustomer(1));
		}

		[Test]
		public void CanGetProductById()
		{
			Assert.IsNotNull(_repository.GetProduct(1));
		}
		[Test]
		public void OrderContainsProduct()
		{
			Assert.True(_repository.GetOrder(1).Products.Any(p => p.Id == 1));
		}
		[Test]
		public void OrderHasACustomer()
		{
			Assert.IsNotNullOrEmpty(_repository.GetTheCustomerOrder(1).Firstname);
		}
		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			var testAdapter = new ObjectContainer();
			_repository = testAdapter.GetRepository();
			var commands = new GetCommands().Get();
            var handlers = testAdapter.GetAllHandlers();
			foreach (var command in commands)
			{
                foreach (var handler in handlers.Where(h=>h.CanHandle(command.GetType())))
				{
					handler.Handle(command);
				}
			}
		}
	}
}
