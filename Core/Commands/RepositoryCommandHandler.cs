using System;

namespace SomeBasicFileStoreApp.Core.Commands
{
    public class RepositoryCommandHandler:ICommandHandler<Command>
    {
        private readonly IRepository repository;
        public RepositoryCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(Command command)
        {
            command.Handle(repository);
        }
    }
}

