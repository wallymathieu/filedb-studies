using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace SomeBasicFileStoreApp.Core.Commands
{
	
    public interface IAppendBatch
    {
        void Batch(IEnumerable<Command> commands);
    }
}
