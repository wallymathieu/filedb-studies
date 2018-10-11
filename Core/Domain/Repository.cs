using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SomeBasicFileStoreApp.Core
{
    public class Repository : IRepository
    {
        private IDictionary<long, Customer> _customers = new ConcurrentDictionary<long, Customer>();
        private IDictionary<long, Product> _products = new ConcurrentDictionary<long, Product>();
        private IDictionary<long, Order> _orders = new ConcurrentDictionary<long, Order>();

        public void Save(Customer obj)
        {
            _customers[obj.Id] = obj;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _customers.Values;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _orders.Values;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _products.Values;
        }

        public void Save(Product obj)
        {
            _products[obj.Id] = obj;
        }
        public void Save(Order obj)
        {
            _orders[obj.Id] = obj;
        }

        public Customer GetCustomer(int customerId)
        {
            return _customers[customerId];
        }

        public Product GetProduct(int productId)
        {
            return _products[productId];
        }

        public Order GetOrder(int orderId)
        {
            return _orders[orderId];
        }
    }
}
