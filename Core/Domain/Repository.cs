using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeBasicFileStoreApp.Core
{
	public class Repository : IRepository
	{
		private IDictionary<long, Customer> _customers = new Dictionary<long, Customer>();
		private IDictionary<long, Product> _products = new Dictionary<long, Product>();
		private IDictionary<long, Order> _orders = new Dictionary<long, Order>();

		public void Save(Customer obj)
		{
			_customers[obj.Id] = obj;
		}
		public void Save(Product obj)
		{
			_products[obj.Id] = obj;
		}
		public void Save(Order obj)
		{
			_orders[obj.Id] = obj;
		}
		public IEnumerable<Customer> QueryOverCustomers()
		{
			return _customers.Values;
		}
		public IEnumerable<Order> QueryOverOrders()
		{
			return _orders.Values;
		}

		public Customer GetCustomer(int v)
		{
			return _customers[v];
		}
		public IEnumerable<Product> QueryOverProducts()
		{
			return _products.Values;
		}

		public Product GetProduct(int v)
		{
			return _products[v];
		}

		public Order GetOrder(int v)
		{
			return _orders[v];
		}

		public Customer GetTheCustomerOrder(int v)
		{
			return _customers[_orders[v].Customer];
		}
	}
}
