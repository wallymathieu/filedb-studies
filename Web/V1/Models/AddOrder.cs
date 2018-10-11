using System;
using SomeBasicFileStoreApp.Core.Commands;

namespace Web.V1.Models
{
    public class AddOrder
    {
        public Command ToCommand(DateTime now)
        {
            return new AddOrderCommand()
            {
                Id = Id,
                Customer=Customer,
                OrderDate = now
            };
        }

        public int Customer { get; set; }

        public int Id { get; set; }
    }
}