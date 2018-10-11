using ProtoBuf;
using System.Collections.Generic;
using System.Linq;
using With;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddProductToOrderCommand : Command
    {
        private static readonly IPreparedCopy<Order, IEnumerable<Product>> UpdateProducts =
            Prepare.Copy<Order, IEnumerable<Product>>((o, v) => o.Products == v);

        [ProtoMember(1)]
        public int OrderId { get; set; }
        [ProtoMember(2)]
        public int ProductId { get; set; }

        public override bool Handle(IRepository repository)
        {
            var order = repository.GetOrder(OrderId);
            var product = repository.GetProduct(ProductId);
            if (order == null || product == null) return false;
            repository.Save(UpdateProducts.Copy(order,
                order.Products.ToList().Tap(o=> o.Add(product))));
            return true;
        }
    }
}
