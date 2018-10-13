using System;
using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
    public class Order
    {
        public Order(int id, Customer customer, DateTime orderDate, IEnumerable<Product> products, int version)
        {
            Id = id;
            Customer = customer;
            OrderDate = orderDate;
            Products = products;
            Version = version;
        }
        public int Id { get; }

        public Customer Customer { get; }

        public DateTime OrderDate { get; }

        public IEnumerable<Product> Products { get; }

        public int Version { get; }
    }
}
