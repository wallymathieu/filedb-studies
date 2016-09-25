using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicFileStoreApp.Core;
using System.Linq;
using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core.Domain;

namespace SomeBasicFileStoreApp.Tests
{
    [TestFixture]
    public class CustomerDataTests
    {
        private IRepository _repository;
        private ObjectContainer _container;
        [Test]
        public void CanGetCustomerById()
        {
            Assert.IsNotNull(_repository.GetCustomer(new CustomerId(1)));
        }

        [Test]
        public void CanGetProductById()
        {
            Assert.IsNotNull(_repository.GetProduct(new ProductId(1)));
        }
        [Test]
        public void OrderContainsProduct()
        {
            Assert.True(_repository.GetOrder(new OrderId( 1)).Products.Any(p => p.Id.Equals(new ProductId(1))));
        }
        [Test]
        public void OrderHasACustomer()
        {
            Assert.IsNotNullOrEmpty(_repository.GetTheCustomerOrder(new OrderId(1)).FirstName);
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _container = new ObjectContainer();
            //_container.Boot();
            _repository = _container.GetRepository();
            var commands = CoreFsTests.GetCommands.Get().ToArray();
            _container.HandleAll(commands.Select(c=>c.Command));
        }
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _container.Dispose();
        }
    }
}
