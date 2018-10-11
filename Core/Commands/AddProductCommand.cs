using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddProductCommand : Command
    {
        [ProtoMember(1)]
        public virtual int Id { get; set; }
        [ProtoMember(2)]
        public virtual int Version { get; set; }
        [ProtoMember(3)]
        public virtual float Cost { get; set; }
        [ProtoMember(4)]
        public virtual string Name { get; set; }
    
        public override bool Handle(IRepository repository)
        {
            if (repository.GetProduct(Id)==null)
            {
                repository.Save(new Product(Id, Cost, Name, Version));
                return true;
            }
            return false;
        }
    }
}
