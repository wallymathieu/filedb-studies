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

        public override bool Handle(IRepository repository)
        {
            var customer = repository.GetCustomer(Id);
            if (customer == null)
            {
                repository.Save(new Customer(Id, Firstname, Lastname, Version));
                return true;
            }
            return false;
        }
    }
}
