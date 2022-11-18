using System;
using System.IO;
using System.Linq;
using With;
using SomeBasicFileStoreApp.Core.Infrastructure.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WallyMathieu.Collections;
using Xunit;

namespace SomeBasicFileStoreApp.Tests.Json
{
    public class PersistingEventsTests:IDisposable
    {
        private readonly List<string> _dbs = new();
        public void Dispose()
        {
            foreach (var db in _dbs)
            {
                File.WriteAllText(db, string.Empty);
            }
        }

        [Fact]
        public async Task Read_items_persisted_in_separate_batches()
        {
            var commands = new GetCommands().Get().ToArray();
            var persist = new AppendToFile("Json_CustomerDataTests_1.db".Tap(db => _dbs.Add(db)));
            var batches = commands.BatchesOf(3).ToArray();
            // in order for the test to be valid
            Assert.True(batches.Length>2, "batches.Length>2");
            foreach (var batch in batches)
            {
                await persist.Batch(batch);
            }

            var all = await persist.ReadAll();
            Assert.Equal(all.Count(), commands.Length);
        }

        [Fact]
        public async Task Read_items_persisted_in_single_batch()
        {
            var commands = new GetCommands().Get().ToArray();
            var persist = new AppendToFile("Json_CustomerDataTests_2.db".Tap(db => _dbs.Add(db)));
            await persist.Batch(commands);
            var all = await persist.ReadAll();
            Assert.Equal(all.Count(), commands.Length);
        }

        [Fact]
        public async Task Read_items()
        {
            var commands = new GetCommands().Get().ToArray();
            var persist = new AppendToFile("Json_CustomerDataTests_3.db".Tap(db => _dbs.Add(db)));
            await persist.Batch(commands);
            var all = await persist.ReadAll();
            Assert.Equal(all.Select(c => c.GetType()).ToArray(), commands.Select(c => c.GetType()).ToArray());
        }
    }
}