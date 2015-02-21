using System;
using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
	public class Order 
    {
		public Order()
		{
			Products = new List<Product>();
        }
		public virtual int Id { get; set; }

		public virtual int Customer { get; set; }

		public virtual DateTime OrderDate { get; set; }

		public virtual IList<Product> Products { get; set; }

		public virtual int Version { get; set; }
	}
}
