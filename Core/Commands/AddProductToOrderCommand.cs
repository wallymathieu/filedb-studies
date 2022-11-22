using ProtoBuf;
using System.Collections.Generic;
using System.Collections.Immutable;
using SomeBasicFileStoreApp.Core.Domain;

namespace SomeBasicFileStoreApp.Core.Commands;

[ProtoContract]
public record AddProductToOrderCommand : Command
{
    [ProtoMember(1)]
    public int OrderId { get; init; }
    [ProtoMember(2)]
    public int ProductId { get; init; }

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