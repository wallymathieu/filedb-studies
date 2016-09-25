using SomeBasicFileStoreApp.Core.Domain;
using System;

namespace SomeBasicFileStoreApp.Core.Commands
{
    public class RepositoryCommandHandler
    {
        private readonly IRepository repository;
        public RepositoryCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(Command command)
        {
            Commands.handle(repository, command);
        }
    }
}

