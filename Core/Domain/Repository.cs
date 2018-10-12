using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SomeBasicFileStoreApp.Core
{
    public class Repository : IRepository
    {
        private readonly IDictionary<long, Customer> _customers = new ConcurrentDictionary<long, Customer>();
        private readonly IDictionary<long, Product> _products = new ConcurrentDictionary<long, Product>();
        private readonly IDictionary<long, Order> _orders = new ConcurrentDictionary<long, Order>();

        public void Save(Customer obj) => _customers[obj.Id] = obj;

        public IEnumerable<Customer> GetCustomers() => _customers.Values;

        public IEnumerable<Order> GetOrders() => _orders.Values;

        public IEnumerable<Product> GetProducts() => _products.Values;

        public void Save(Product obj) => _products[obj.Id] = obj;

        public void Save(Order obj) => _orders[obj.Id] = obj;

        public bool TryGetCustomer(int customerId, out Customer customer) =>
            _customers.TryGetValue(customerId, out customer);

        public bool TryGetProduct(int productId, out Product product) =>
            _products.TryGetValue(productId, out product);

        public bool TryGetOrder(int orderId, out Order order) =>
            _orders.TryGetValue(orderId, out order);
    }
}