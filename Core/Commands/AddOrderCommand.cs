using ProtoBuf;
using System;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddOrderCommand : Command
    {
        [ProtoMember(1)] public virtual int Id { get; set; }
        [ProtoMember(2)] public virtual int Version { get; set; }
        [ProtoMember(3)] public virtual int Customer { get; set; }
        [ProtoMember(4)] public virtual DateTime OrderDate { get; set; }

        public override bool Run(IRepository repository)
        {
            if (!repository.TryGetCustomer(Customer, out var customer) ||
                Id > 0 && repository.TryGetOrder(Id, out _)) return false;
            repository.Save(new Order(
                Customer: customer,
                OrderDate: OrderDate,
                Products: new Product[0],
                Version: Version,
                Id: Id <= 0
                    ? repository.NextOrderId()
                    : Id));
            return true;
        }
    }
}