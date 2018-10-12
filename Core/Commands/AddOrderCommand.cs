using ProtoBuf;
using System;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddOrderCommand : Command
    {
        [ProtoMember(1)]
        public virtual int Id { get; set; }
        [ProtoMember(2)]
        public virtual int Version { get; set; }
        [ProtoMember(3)]
        public virtual int Customer { get; set; }
        [ProtoMember(4)]
        public virtual DateTime OrderDate { get; set; }
       
        public override bool Handle(IRepository repository)
        {
            if (repository.TryGetCustomer(Customer, out var customer)
                && !repository.TryGetOrder(Id, out _))
            {
                repository.Save(new Order(Id, customer, OrderDate, new Product[0], Version));
                return true;
            }
            return false;
        }
    }
}
