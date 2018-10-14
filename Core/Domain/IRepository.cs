using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
    public interface IRepository
    {
        bool TryGetCustomer(int customerId, out Customer customer);
        bool TryGetProduct(int productId, out Product product);
        bool TryGetOrder(int orderId, out Order order);
        void Save(Product obj);
        void Save(Order obj);
        void Save(Customer obj);
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Order> GetOrders();
        IEnumerable<Product> GetProducts();
        int NextCustomerId();
        int NextOrderId();
        int NextProductId();
    }
}