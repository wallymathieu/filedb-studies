using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core.Commands
{
    public interface IAppendBatch
    {
        void Batch(IEnumerable<Command> commands);
    }
}
