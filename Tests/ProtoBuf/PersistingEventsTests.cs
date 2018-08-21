using System;
using Xunit;
using System.IO;
using System.Linq;
using With;
using SomeBasicFileStoreApp.Core.Infrastructure.ProtoBuf;
using System.Collections.Generic;
using With.Collections;

namespace SomeBasicFileStoreApp.Tests.ProtoBuf
{
    public class PersistingEventsTests:IDisposable
    {
        private List<string> dbs = new List<string>();
        public void Dispose()
        {
            foreach (var db in dbs)
            {
                File.WriteAllText(db, string.Empty);
            }
        }

        [Fact]
        public void Read_items_persisted_in_separate_batches()
        {
            var commands = new GetCommands().Get().ToArray();
            var _persist = new AppendToFile("Proto_CustomerDataTests_1.db".Tap(db => dbs.Add(db)));
            var batches = commands.BatchesOf(3).ToArray();
            // in order for the test to be valid
            Assert.True(batches.Length>2, "batches.Length>2");
            foreach (var batch in batches)
            {
                _persist.Batch(batch);
            }
            Assert.Equal(_persist.ReadAll().Count(), commands.Length);
        }

        [Fact]
        public void Read_items_persisted_in_single_batch()
        {
            var commands = new GetCommands().Get().ToArray();
            var _persist = new AppendToFile("Proto_CustomerDataTests_2.db".Tap(db => dbs.Add(db)));
            _persist.Batch(commands);
            Assert.Equal(_persist.ReadAll().Count(), commands.Length);
        }
    }
}