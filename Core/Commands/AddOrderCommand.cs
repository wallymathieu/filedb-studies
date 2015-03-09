
using ProtoBuf;
using System;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddOrderCommand : Command
    {
        [ProtoMember(1)]
        public virtual int Id { get; private set; }
        [ProtoMember(2)]
        public virtual int Version { get; private set; }

        [ProtoMember(3)]
        public virtual int Customer { get; private set; }

        [ProtoMember(4)]
        public virtual DateTime OrderDate { get; private set; }
       
        public override void Handle(IRepository _repository)
        {
            var command = this;
            _repository.Save(new Order(command.Id, command.Customer, command.OrderDate, new Product[0], command.Version));
        }
    }
}
