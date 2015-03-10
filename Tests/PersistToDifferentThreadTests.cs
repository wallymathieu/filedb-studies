using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicFileStoreApp.Core;
using System.Linq;
using System;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core.Commands;


namespace SomeBasicFileStoreApp.Tests
{
	
    [TestFixture]
    public class PersistToDifferentThreadTests
    {
        private Command[][] _batches;
        private Command[] _commandsSent;
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var container = new ObjectContainer();
            container.Boot();
            _commandsSent = new GetCommands().Get().ToArray();
            var handlers = container.GetAllHandlers();
            foreach (var command in _commandsSent)
            {
                foreach (var handler in handlers.Where(h=>h.CanHandle(command.GetType())))
                {
                    handler.Handle(command);
                }
            }
            container.Dispose();
            _batches = container.BatchesPersisted().ToArray();
        }

        [Test]
        public void Will_send_all_commands_to_the_different_thread()
        {
            Assert.That(_batches.Count(), Is.LessThanOrEqualTo(_commandsSent.Count()));
            Assert.That(_batches.SelectMany(b=>b).Count(), Is.EqualTo(_commandsSent.Count()));
        }

        [Test]
        public void Order()
        {
            var enumerator = _batches.SelectMany(b => b).GetEnumerator();
            enumerator.MoveNext();
            var last = enumerator.Current;
            for (; enumerator.MoveNext(); )
            {
                Assert.That(enumerator.Current.SequenceNumber, Is.GreaterThan(last.SequenceNumber));
                last = enumerator.Current;
            }
            //Console.WriteLine(string.Join(", ",
            //    _batches.Select(b => "["+string.Join(", ", b.Select(a => a.SequenceNumber)) +"]" )));
        }
    }
}
