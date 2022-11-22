using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SomeBasicFileStoreApp.Core.Infrastructure.Threading;

namespace SomeBasicFileStoreApp.Core.Domain;

public class Repository : IRepository
{
    private readonly IDictionary<long, Customer> _customers = new ConcurrentDictionary<long, Customer>();
    private readonly IDictionary<long, Product> _products = new ConcurrentDictionary<long, Product>();
    private readonly IDictionary<long, Order> _orders = new ConcurrentDictionary<long, Order>();
    private readonly ThreadSafeMax<int> _customerId =new(0);
    private readonly ThreadSafeMax<int> _orderId =new(0);
    private readonly ThreadSafeMax<int> _productId =new(0);
    public IEnumerable<Customer> GetCustomers() => _customers.Values;
    public IEnumerable<Order> GetOrders() => _orders.Values;
    public IEnumerable<Product> GetProducts() => _products.Values;
    public int NextCustomerId() => _customerId.Value + 1;
    public int NextOrderId() => _orderId.Value + 1;
    public int NextProductId() => _productId.Value + 1;
    public void Save(Product obj) => 
        _products[_productId.Observe(obj.Id)] = obj;

    public void Save(Order obj) => 
        _orders[_orderId.Observe(obj.Id)] = obj;

    public void Save(Customer obj) => 
        _customers[_customerId.Observe(obj.Id)] = obj;

    public bool TryGetCustomer(int customerId, [MaybeNullWhen(false)] out Customer customer) =>
        _customers.TryGetValue(customerId, out customer);

    public bool TryGetProduct(int productId, [MaybeNullWhen(false)] out Product product) =>
        _products.TryGetValue(productId, out product);

    public bool TryGetOrder(int orderId, [MaybeNullWhen(false)] out Order order) =>
        _orders.TryGetValue(orderId, out order);
}