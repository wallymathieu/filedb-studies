using SomeBasicFileStoreApp.Core.Commands;
using System.Collections.Generic;

namespace SomeBasicFileStoreApp.Core.Infrastructure
{
    public interface IAppendBatch
    {
        void Batch(IEnumerable<Command> commands);
    }
}
