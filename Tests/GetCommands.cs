using SomeBasicFileStoreApp.Core;
using SomeBasicFileStoreApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SomeBasicFileStoreApp.Tests
{
	class GetCommands
	{
		public GetCommands()
		{
		}
		public IEnumerable<Command> Get()
		{
			var commands = new List<Command>();
			var import = new XmlImport(XDocument.Load(Path.Combine("TestData", "TestData.xml")), "http://tempuri.org/Database.xsd");
			import.Parse(new[] { typeof(Customer), typeof(Order), typeof(Product) },
							(type, obj) =>
							{
								if (obj is Customer)
								{
									commands.Add(new AddCustomerCommand { Object = (Customer)obj });
								}
								if (obj is Product)
								{
									commands.Add(new AddProductCommand { Object = (Product)obj });
								}
								if (obj is Order)
								{
									commands.Add(new AddOrderCommand { Object = (Order)obj });
								}
							}, onIgnore: (type, property) => {
								throw new Exception(string.Format("ignoring property {1} on {0}", type.Name, property.PropertyType.Name));
							});
			import.ParseConnections("OrderProduct", "Product", "Order", (productId, orderId) =>
			{
				commands.Add(new AddProductToOrder { ProductId = productId, OrderId = orderId });
			});


			return commands;
		}
	}
}
