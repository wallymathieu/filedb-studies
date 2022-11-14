using ProtoBuf;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddProductToOrderCommand : Command
    {
        [ProtoMember(1)]
        public int OrderId { get; set; }
        [ProtoMember(2)]
        public int ProductId { get; set; }

        public override bool Run(IRepository repository)
        {
            if (!repository.TryGetOrder(OrderId, out var order) 
                || !repository.TryGetProduct(ProductId, out var product)) return false;
            var orderNext = order with
            {
                Products = new List<Product>(order.Products) { product }
                    .ToImmutableArray()
            };
            repository.Save(orderNext);
            return true;
        }
    }
}
