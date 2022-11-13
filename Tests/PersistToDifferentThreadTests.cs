using System.Linq;
using SomeBasicFileStoreApp.Core.Commands;
using With.Collections;
using Xunit;

namespace SomeBasicFileStoreApp.Tests
{

    public class PersistToDifferentThreadTests
    {
        private static readonly Command[][] Batches;
        private static readonly Command[] CommandsSent;
        static PersistToDifferentThreadTests()
        {
            var container = new ObjectContainer();
            container.Boot();
            CommandsSent = new GetCommands().Get().ToArray();
            container.PersistAll(CommandsSent);
            container.Dispose();
            Batches = container.BatchesPersisted().ToArray();
        }

        [Fact]
        public void Will_send_all_commands_to_the_different_thread()
        {
            Assert.True(Batches.Length<=CommandsSent.Length, "_batches.Count()<=_commandsSent.Count()");
            Assert.True(Batches.SelectMany(b=>b).Count()== CommandsSent.Count(), 
                "_batches.SelectMany(b=>b).Count()== _commandsSent.Count()");
        }

        [Fact]
        public void Order()
        {
            Batches.SelectMany(b => b).Pairwise(((last, current) =>
            {
                Assert.True(current.SequenceNumber>last.SequenceNumber, "current.SequenceNumber>last.SequenceNumber");
                return "Success";
            })).ToArray();
        }
    }
}
