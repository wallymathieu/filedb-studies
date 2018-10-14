using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddCustomerCommand : Command
    {
        [ProtoMember(1)]
        public virtual int Id { get; set; }

        [ProtoMember(2)]
        public virtual int Version { get; set; }

        [ProtoMember(3)]
        public virtual string Firstname { get; set; }

        [ProtoMember(4)]
        public virtual string Lastname { get; set; }

        public override bool Run(IRepository repository)
        {
            if (Id > 0 && repository.TryGetCustomer(Id, out _)) return false;
            repository.Save(new Customer(
                firstName: Firstname, lastName: Lastname,version: Version, 
                id:Id<=0
                    ?repository.NextCustomerId()
                    :Id));
            return true;
        }
    }
}
