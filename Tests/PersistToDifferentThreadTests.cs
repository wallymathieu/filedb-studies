using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using SomeBasicFileStoreApp.Core;
using System.Linq;
using System;
using System.Collections.Generic;
namespace SomeBasicFileStoreApp.Tests
{
	
    [TestFixture]
    public class PersistToDifferentThreadTests
    {
        private ObjectContainer _container;
        [Test]
        public void WillSendCommandsToDifferentThread()
        {
            _container = new ObjectContainer();
            _container.Boot();
            var commands = new GetCommands().Get().ToArray();
            var handlers = _container.GetAllHandlers();
            foreach (var command in commands)
            {
                foreach (var handler in handlers.Where(h=>h.CanHandle(command.GetType())))
                {
                    handler.Handle(command);
                }
            }
            _container.Dispose();
            var batches = _container.BatchesPersisted().ToArray();
            Assert.That(batches.Count(), Is.LessThanOrEqualTo(commands.Count()));
            Assert.That(batches.SelectMany(b=>b).Count(), Is.EqualTo(commands.Count()));
        }
    }
}
