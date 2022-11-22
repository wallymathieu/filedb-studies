using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SomeBasicFileStoreApp.Core.Domain;

namespace SomeBasicFileStoreApp.Core
{
    public interface IRepository
    {
        bool TryGetCustomer(int customerId, [MaybeNullWhen(false)]out Customer customer);
        bool TryGetProduct(int productId, [MaybeNullWhen(false)]out Product product);
        bool TryGetOrder(int orderId, [MaybeNullWhen(false)]out Order order);
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