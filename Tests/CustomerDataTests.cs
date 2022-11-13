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
            Assert.Contains(_repository.GetOrder(1).Products, p => p.Id == 1);
        }
        [Fact]
        public void OrderHasACustomer()
        {
            Assert.False(string.IsNullOrWhiteSpace(_repository.GetOrder(1).Customer.Name.First));
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
