using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    [ProtoInclude(1, typeof(AddCustomerCommand))]
    [ProtoInclude(2, typeof(AddProductCommand))]
    [ProtoInclude(3, typeof(AddOrderCommand))]
    [ProtoInclude(4, typeof(AddProductToOrder))]
    public abstract class Command
    {
        public long SequenceNumber { get; set; }
        public abstract void Handle(IRepository repository);
    }
}