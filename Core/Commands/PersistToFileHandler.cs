using System;

namespace SomeBasicFileStoreApp.Core.Commands
{
    public class PersistToFileHandler:ICommandHandler<Command>
    {
        public PersistToFileHandler()
        {
        }

        public void Handle(Command command)
        {
            // send the command to separate thread and persist it
            throw new NotImplementedException();
        }
    }
}

