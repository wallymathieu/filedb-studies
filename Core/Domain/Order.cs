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
        public virtual int Id { get; }

        public virtual Customer Customer { get; }

        public virtual DateTime OrderDate { get; }

        public virtual IEnumerable<Product> Products { get; }

        public virtual int Version { get; }
    }
}
