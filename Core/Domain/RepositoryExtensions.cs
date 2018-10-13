using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
    public static class RepositoryExtensions
    {
        public static Customer GetCustomer(this IRepository repository, int customerId)
        {
            if (repository.TryGetCustomer(customerId, out var customer)) return customer;
            throw new KeyNotFoundException();
        }
        public static Product GetProduct(this IRepository repository, int productId)
        {
            if (repository.TryGetProduct(productId, out var product)) return product;
            throw new KeyNotFoundException();
        }
        public static Order GetOrder(this IRepository repository, int orderId)
        {
            if (repository.TryGetOrder(orderId, out var order)) return order;
            throw new KeyNotFoundException();
        }
    }
}