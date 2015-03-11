using NUnit.Framework;
using System.IO;
using System.Linq;
using SomeBasicFileStoreApp.Core.Commands;
using With;
namespace SomeBasicFileStoreApp.Tests
{
    [TestFixture]
	public class PersistingEventsTests
	{
		private AppendToFile _persist;

		[SetUp]
		public void TestFixtureSetup()
		{
			File.WriteAllText("CustomerDataTests.db", string.Empty); 
		}

		[Test]
		public void Read_items_persisted_in_separate_batches()
		{
			var commands = new GetCommands().Get().ToArray();
			_persist = new AppendToFile("CustomerDataTests.db");
            var batches = commands.BatchesOf(3).ToArray();
			// in order for the test to be valid
			Assert.That(batches.Length, Is.GreaterThan(2));
			foreach (var batch in batches)
			{
				_persist.Batch(batch);
			}
			Assert.That(_persist.ReadAll().Count(), Is.EqualTo(commands.Length));
        }

		[Test]
		public void Read_items_persisted_in_single_batch()
		{
			var commands = new GetCommands().Get().ToArray();
			_persist = new AppendToFile("CustomerDataTests.db");
            _persist.Batch(commands);
			Assert.That(_persist.ReadAll().Count(), Is.EqualTo(commands.Length));
		}
	}
}