using ProtoBuf;
using System.Collections.Generic;
using System.Linq;
using With;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddProductToOrder : Command
    {
        private static IPreparedCopy<Order, IEnumerable<Product>> UpdateProducts =
            Prepare.Copy<Order, IEnumerable<Product>>((o, v) => o.Products == v);
        [ProtoMember(1)]
        public int OrderId { get; set; }
        [ProtoMember(2)]
        public int ProductId { get; set; }

        public override void Handle(IRepository _repository)
        {
            var command = this;
            var order = _repository.GetOrder(command.OrderId);
            _repository.Save(UpdateProducts.Copy(order,
                order.Products.ToList().Tap(o=>o.Add(_repository.GetProduct(command.ProductId)))));
        }
    }
}
