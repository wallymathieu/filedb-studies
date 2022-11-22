using System.Collections.Generic;
using ProtoBuf;
using SomeBasicFileStoreApp.Core.Domain;

namespace SomeBasicFileStoreApp.Core.Commands;

[ProtoContract]
public record AddProductCommand : Command
{
    [ProtoMember(1)] public virtual int Id { get; init; }
    [ProtoMember(2)] public virtual int Version { get; init; }
    [ProtoMember(3)] public virtual float Cost { get; init; }
    [ProtoMember(4)] public virtual string Name { get; init; } = null!;
    [ProtoMember(5)] public virtual IDictionary<ProductProperty, string>? Properties { get; init; }

    public override bool Run(IRepository repository)
    {
        if (Id > 0 && repository.TryGetProduct(Id, out _)) return false;
        repository.Save(new Product(
            Cost: Cost,
            Name: Name,
            Version: Version,
            Id: Id <= 0
                ? repository.NextProductId()
                : Id,
            Properties: Properties));
        return true;
    }

}