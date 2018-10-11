using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
    public interface IRepository
    {
        Customer GetCustomer(int customerId);
        Product GetProduct(int productId);
        Order GetOrder(int orderId);
        void Save(Product obj);
        void Save(Order obj);
        void Save(Customer obj);
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Order> GetOrders();
        IEnumerable<Product> GetProducts();
    }
}