using ProtoBuf;
using System;
using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core
{
	public class Product 
    {
		public virtual int Id { get; set; }

		public virtual float Cost { get; set; }

		public virtual string Name { get; set; }

		public virtual int Version { get; set; }
		public string EventId { get { return "Product_" + Id; } }
	}
}
