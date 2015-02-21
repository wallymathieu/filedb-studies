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

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			var testAdapter = new ObjectContainer();
			_repository = testAdapter.GetRepository();
			var commands = new GetCommands().Get();
			var handlers = testAdapter.GetAllHandlers().ToArray();
			foreach (var command in commands)
			{
				var handler = handlers.Single(h => h.CanHandle(command));
				handler.Handle(command);
			}
		}
	}
}
