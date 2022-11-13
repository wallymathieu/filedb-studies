using SomeBasicFileStoreApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SomeBasicFileStoreApp.Tests
{
    class GetCommands
    {
        private int _sequence = 0;

        public GetCommands()
        {
        }

        public IEnumerable<Command> Get()
        {
            var commands = new List<Command>();
            var import = new XmlImport(XDocument.Load(Path.Combine("TestData", "TestData.xml")),
                "http://tempuri.org/Database.xsd");
            var addCustomerCommands = import.Parse<AddCustomerCommand>("Customer",
                onIgnore: (type, property) =>
                    throw new Exception(string.Format("ignoring property {1} on {0}", type,
                        property.PropertyType.Name)));
            foreach (var addCustomerCommand in addCustomerCommands)
            {
                addCustomerCommand.SequenceNumber = ++_sequence;
                commands.Add(addCustomerCommand);
            }

            var addProductCommands = import.Parse<AddProductCommand>("Product",
                onIgnore: (type, property) =>
                    throw new Exception(string.Format("ignoring property {1} on {0}", type,
                        property.PropertyType.Name)));
            foreach (var addProductCommand in addProductCommands)
            {
                addProductCommand.SequenceNumber = ++_sequence;
                commands.Add(addProductCommand);
            }

            var addOrderCommands = import.Parse<AddOrderCommand>("Order",
                onIgnore: (type, property) =>
                    throw new Exception(string.Format("ignoring property {1} on {0}", type,
                        property.PropertyType.Name)));
            foreach (var addOrderCommand in addOrderCommands)
            {
                addOrderCommand.SequenceNumber = ++_sequence;
                commands.Add(addOrderCommand);
            }

            var orderProducts = import.ParseConnections("OrderProduct", "Product", "Order");
            foreach (var (productId, orderId) in orderProducts)
            {
                commands.Add(new AddProductToOrderCommand
                    {ProductId = productId, OrderId = orderId, SequenceNumber = ++_sequence});
            }

            return commands;
        }
    }
}