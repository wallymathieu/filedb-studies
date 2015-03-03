using System;
using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
	public class Order 
    {
        public Order(int id, int customer, DateTime orderDate, IEnumerable<Product> products, int version)
		{
            Id = id;
            Customer = customer;
            OrderDate = orderDate;
            Products = products;
            Version = version;
        }
		public virtual int Id { get; private set; }

		public virtual int Customer { get; private set; }

		public virtual DateTime OrderDate { get; private set; }

        public virtual IEnumerable<Product> Products { get; private set; }

		public virtual int Version { get; private set; }
	}
}
