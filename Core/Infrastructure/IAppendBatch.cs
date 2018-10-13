using SomeBasicFileStoreApp.Core.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeBasicFileStoreApp.Core.Infrastructure
{
    public interface IAppendBatch
    {
        Task Batch(IEnumerable<Command> commands);
        Task<IEnumerable<Command>> ReadAll();
    }
}
