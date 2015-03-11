using SomeBasicFileStoreApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SomeBasicFileStoreApp.Tests
{
    class GetCommands
	{
        private int sequence = 0;
		public GetCommands()
		{
		}
		public IEnumerable<Command> Get()
		{
			var commands = new List<Command>();
			var import = new XmlImport(XDocument.Load(Path.Combine("TestData", "TestData.xml")), "http://tempuri.org/Database.xsd");
			import.Parse<AddCustomerCommand>("Customer",
				(obj) =>
				{
                    obj.SequenceNumber = ++sequence;
					commands.Add(obj);
				}, onIgnore: (type, property) => {
					throw new Exception(string.Format("ignoring property {1} on {0}", type, property.PropertyType.Name));
				});
			import.Parse<AddOrderCommand>("Order", 
				(obj) =>
				{
                    obj.SequenceNumber = ++sequence;
					commands.Add(obj);
				}, onIgnore: (type, property) => {
					throw new Exception(string.Format("ignoring property {1} on {0}", type, property.PropertyType.Name));
				});

			import.Parse<AddProductCommand>("Product", 
				(obj) =>
				{
                    obj.SequenceNumber = ++sequence;
					commands.Add(obj);
				}, onIgnore: (type, property) => {
					throw new Exception(string.Format("ignoring property {1} on {0}", type, property.PropertyType.Name));
				});

			import.ParseConnections("OrderProduct", "Product", "Order", (productId, orderId) =>
			{
                var obj = new AddProductToOrder { ProductId = productId, OrderId = orderId };
                obj.SequenceNumber = ++sequence;
				commands.Add(obj);
			});

			return commands;
		}
	}
}
