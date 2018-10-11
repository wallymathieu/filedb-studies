using System;
using System.IO;
using System.Linq;
using With;
using SomeBasicFileStoreApp.Core.Infrastructure.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using With.Collections;
using Xunit;

namespace SomeBasicFileStoreApp.Tests.Json
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
        public async Task Read_items_persisted_in_separate_batches()
        {
            var commands = new GetCommands().Get().ToArray();
            var _persist = new AppendToFile("Json_CustomerDataTests_1.db".Tap(db => dbs.Add(db)));
            var batches = commands.BatchesOf(3).ToArray();
            // in order for the test to be valid
            Assert.True(batches.Length>2, "batches.Length>2");
            foreach (var batch in batches)
            {
                await _persist.Batch(batch);
            }

            var all = await _persist.ReadAll();
            Assert.Equal(all.Count(), commands.Length);
        }

        [Fact]
        public async Task Read_items_persisted_in_single_batch()
        {
            var commands = new GetCommands().Get().ToArray();
            var _persist = new AppendToFile("Json_CustomerDataTests_2.db".Tap(db => dbs.Add(db)));
            await _persist.Batch(commands);
            var all = await _persist.ReadAll();
            Assert.Equal(all.Count(), commands.Length);
        }

        [Fact]
        public async Task Read_items()
        {
            var commands = new GetCommands().Get().ToArray();
            var _persist = new AppendToFile("Json_CustomerDataTests_3.db".Tap(db => dbs.Add(db)));
            await _persist.Batch(commands);
            var all = await _persist.ReadAll();
            Assert.Equal(all.Select(c => c.GetType()).ToArray(), commands.Select(c => c.GetType()).ToArray());
        }
    }
}