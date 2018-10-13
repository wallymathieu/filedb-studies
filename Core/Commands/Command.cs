using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    [ProtoInclude(1, typeof(AddCustomerCommand))]
    [ProtoInclude(2, typeof(AddProductCommand))]
    [ProtoInclude(3, typeof(AddOrderCommand))]
    [ProtoInclude(4, typeof(AddProductToOrderCommand))]
    public abstract class Command
    {
        public long SequenceNumber { get; set; }
        
        /// <summary>
        /// Run command on repository, changing the repository state
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public abstract bool Run(IRepository repository);
    }
}