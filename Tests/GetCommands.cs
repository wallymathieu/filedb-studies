using SomeBasicFileStoreApp.Core;
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
			XmlImport.Parse(XDocument.Load(Path.Combine("TestData", "TestData.xml")), new[] { typeof(Customer), typeof(Order), typeof(Product) },
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
							}, "http://tempuri.org/Database.xsd");
			return commands;
		}
	}
}
