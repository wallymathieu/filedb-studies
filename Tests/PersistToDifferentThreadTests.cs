using NUnit.Framework;
using System.Linq;
using SomeBasicFileStoreApp.Core.Commands;
using With.Linq;
using static CoreFsTests.GetCommands;

namespace SomeBasicFileStoreApp.Tests
{

    [TestFixture]
    public class PersistToDifferentThreadTests
    {
        private Command[][] _batches;
        private WithSeqenceNumber[] _commandsSent;
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var container = new ObjectContainer();
            container.Boot();
            _commandsSent = Get().ToArray();
            container.HandleAll(_commandsSent.Select(c=>c.Command));
            container.Dispose();
            _batches = container.BatchesPersisted().ToArray();
        }

        [Test]
        public void Will_send_all_commands_to_the_different_thread()
        {
            Assert.That(_batches.Count(), Is.LessThanOrEqualTo(_commandsSent.Count()));
            Assert.That(_batches.SelectMany(b=>b).Count(), Is.EqualTo(_commandsSent.Count()));
        }

        [Test,Ignore()]
        public void Order()
        {
            /*
            _batches.SelectMany(b => b).Pairwise(((last, current) =>
            {
                Assert.That(current.SequenceNumber,
                    Is.GreaterThan(last.SequenceNumber));
                return "Success";
            })).ToArray();
            */
        }
    }
}
