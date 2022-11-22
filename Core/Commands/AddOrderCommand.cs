using ProtoBuf;
using System;
using SomeBasicFileStoreApp.Core.Domain;

namespace SomeBasicFileStoreApp.Core.Commands;

[ProtoContract]
public record AddOrderCommand : Command
{
    [ProtoMember(1)] public virtual int Id { get; init; }
    [ProtoMember(2)] public virtual int Version { get; init; }
    [ProtoMember(3)] public virtual int Customer { get; init; }
    [ProtoMember(4)] public virtual DateTime OrderDate { get; init; }

    public override bool Run(IRepository repository)
    {
        if (!repository.TryGetCustomer(Customer, out var customer) ||
            Id > 0 && repository.TryGetOrder(Id, out _)) return false;
        repository.Save(new Order(
            Customer: customer,
            OrderDate: OrderDate,
            Products: Array.Empty<Product>(),
            Version: Version,
            Id: Id <= 0
                ? repository.NextOrderId()
                : Id));
        return true;
    }
}