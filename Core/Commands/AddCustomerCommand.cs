﻿using ProtoBuf;

namespace SomeBasicFileStoreApp.Core.Commands
{
    [ProtoContract]
    public class AddCustomerCommand : Command
    {
        [ProtoMember(1)]
        public virtual int Id { get; private set; }

        [ProtoMember(2)]
        public virtual int Version { get; private set; }

        [ProtoMember(3)]
        public virtual string Firstname { get; private set; }

        [ProtoMember(4)]
        public virtual string Lastname { get; private set; }

        public override void Handle(IRepository _repository)
        {
            var command = this;
            _repository.Save(new Customer(command.Id, command.Firstname, command.Lastname, command.Version));
        }
    }
}
