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

        public override bool Run(IRepository repository)
        {
            if (Id > 0 && repository.TryGetProduct(Id, out _)) return false;
            repository.Save(new Product(
                cost: Cost,
                name: Name,
                version: Version,
                id: Id <= 0
                    ? repository.NextProductId()
                    : Id));
            return true;
        }
    }
}