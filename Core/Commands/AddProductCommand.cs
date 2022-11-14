using System.Collections.Generic;
using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddProductCommand : Command
    {
        [ProtoMember(1)] public virtual int Id { get; set; }
        [ProtoMember(2)] public virtual int Version { get; set; }
        [ProtoMember(3)] public virtual float Cost { get; set; }
        [ProtoMember(4)] public virtual string Name { get; set; }
        [ProtoMember(5)] public virtual IDictionary<ProductProperty, string> Properties { get; set; }

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
}