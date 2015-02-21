using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using SomeBasicFileStoreApp.Core;
using System.Linq;
using SomeBasicFileStoreApp.Core.Commands;

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

		private IEnumerable<IEnumerable<T>> BatchesOf<T>(IEnumerable<T> xs, int count)
		{
			var enumerator = xs.GetEnumerator();
            while (true)
			{
				var list = new List<T>(count);
				for (int i = 0; i < count && enumerator.MoveNext(); i++)
				{
					list.Add(enumerator.Current);
                }
				if (!list.Any())
				{
					break;
				}
				yield return list;
			}
		}

		[Test]
		public void Read_items_persisted_in_separate_batches()
		{
			var commands = new GetCommands().Get().ToArray();
			_persist = new AppendToFile("CustomerDataTests.db");
			var batches = BatchesOf(commands, 3).ToArray();
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