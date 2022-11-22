using ProtoBuf;
using SomeBasicFileStoreApp.Core.Domain;

namespace SomeBasicFileStoreApp.Core.Commands;

[ProtoContract]
public record AddCustomerCommand : Command
{
    [ProtoMember(1)]
    public virtual int Id { get; init; }

    [ProtoMember(2)]
    public virtual int Version { get; init; }

    [ProtoMember(3)]
    public virtual string Firstname { get; init; } = null!;

    [ProtoMember(4)]
    public virtual string Lastname { get; init; } = null!;

    public override bool Run(IRepository repository)
    {
        if (Id > 0 && repository.TryGetCustomer(Id, out _)) return false;
        repository.Save(Customer.Create(
            firstName: Firstname, lastName: Lastname,version: Version, 
            id:Id<=0
                ?repository.NextCustomerId()
                :Id));
        return true;
    }
}