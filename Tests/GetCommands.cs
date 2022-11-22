using SomeBasicFileStoreApp.Core.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SomeBasicFileStoreApp.Tests;

class GetCommands
{
    private int _sequence = 0;

    public IEnumerable<Command> Get()
    {
        var import = new XmlImport(XDocument.Load(Path.Combine("TestData", "TestData.xml")),
            "http://tempuri.org/Database.xsd");
        var addCustomerCommands = import.Parse<AddCustomerCommand>("Customer",
            onIgnore: (type, property) =>
                throw new Exception(string.Format("ignoring property {1} on {0}", type,
                    property.PropertyType.Name)));
        var commands = addCustomerCommands
            .Select(addCustomerCommand => addCustomerCommand with {SequenceNumber = ++_sequence})
            .Cast<Command>().ToList();

        var addProductCommands = import.Parse<AddProductCommand>("Product",
            onIgnore: (type, property) =>
                throw new Exception(string.Format("ignoring property {1} on {0}", type,
                    property.PropertyType.Name)));
        commands.AddRange(addProductCommands
            .Select(addProductCommand => addProductCommand with {SequenceNumber = ++_sequence}));

        var addOrderCommands = import.Parse<AddOrderCommand>("Order",
            onIgnore: (type, property) =>
                throw new Exception(string.Format("ignoring property {1} on {0}", type,
                    property.PropertyType.Name)));
        commands.AddRange(addOrderCommands
            .Select(addOrderCommand => addOrderCommand with {SequenceNumber = ++_sequence}));

        var orderProducts = import.ParseConnections("OrderProduct", "Product", "Order");
        foreach (var (productId, orderId) in orderProducts)
        {
            commands.Add(new AddProductToOrderCommand
                {ProductId = productId, OrderId = orderId, SequenceNumber = ++_sequence});
        }

        return commands;
    }
}