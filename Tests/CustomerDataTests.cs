using SomeBasicFileStoreApp.Core;
using System.Linq;
using Xunit;

namespace SomeBasicFileStoreApp.Tests
{
    public class CustomerDataTests
    {
        private static IRepository _repository;
        private static ObjectContainer _container;
        [Fact]
        public void CanGetCustomerById()
        {
            Assert.NotNull(_repository.GetCustomer(1));
        }

        [Fact]
        public void CanGetProductById()
        {
            Assert.NotNull(_repository.GetProduct(1));
        }
        [Fact]
        public void OrderContainsProduct()
        {
            Assert.True(_repository.GetOrder(1).Products.Any(p => p.Id == 1));
        }
        [Fact]
        public void OrderHasACustomer()
        {
            Assert.False(string.IsNullOrWhiteSpace(_repository.GetTheCustomerOrder(1).Firstname));
        }
        static CustomerDataTests()
        {
            _container = new ObjectContainer();
            //_container.Boot();
            _repository = _container.GetRepository();
            var commands = new GetCommands().Get();
            _container.HandleAll(commands);
        }
    }
}
