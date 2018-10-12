using System.Linq;
using SomeBasicFileStoreApp.Core.Commands;
using With.Collections;
using Xunit;

namespace SomeBasicFileStoreApp.Tests
{

    public class PersistToDifferentThreadTests
    {
        private static Command[][] _batches;
        private static Command[] _commandsSent;
        static PersistToDifferentThreadTests()
        {
            var container = new ObjectContainer();
            container.Boot();
            _commandsSent = new GetCommands().Get().ToArray();
            container.PersistAll(_commandsSent);
            container.Dispose();
            _batches = container.BatchesPersisted().ToArray();
        }

        [Fact]
        public void Will_send_all_commands_to_the_different_thread()
        {
            Assert.True(_batches.Length<=_commandsSent.Length, "_batches.Count()<=_commandsSent.Count()");
            Assert.True(_batches.SelectMany(b=>b).Count()== _commandsSent.Count(), 
                "_batches.SelectMany(b=>b).Count()== _commandsSent.Count()");
        }

        [Fact]
        public void Order()
        {
            _batches.SelectMany(b => b).Pairwise(((last, current) =>
            {
                Assert.True(current.SequenceNumber>last.SequenceNumber, "current.SequenceNumber>last.SequenceNumber");
                return "Success";
            })).ToArray();
        }
    }
}
