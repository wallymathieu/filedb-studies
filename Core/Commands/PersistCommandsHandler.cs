using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SomeBasicFileStoreApp.Core.Infrastructure;

namespace SomeBasicFileStoreApp.Core.Commands;

public class PersistCommandsHandler
{
    private Thread? _thread;
    private bool _stop;
    private readonly ConcurrentQueue<Command> _commands = new();
    private readonly IEnumerable<IAppendBatch> _appendBatch;
    private readonly EventWaitHandle _signal;

    public PersistCommandsHandler(IEnumerable<IAppendBatch> appendBatch)
    {
        _appendBatch = appendBatch;
        _signal = new EventWaitHandle(false, EventResetMode.AutoReset);
    }

    public void Start()
    {
        if (_thread != null)
        {
            throw new Exception();
        }
        _thread = new Thread(ThreadStart);
        _thread.Start();
    }

    private void ThreadStart()
    {
        while (!_stop)
        {
            _signal.WaitOne();
            AppendBatch().Wait();
        }
        // While the batch has been running, more commands might have been added
        // and stop might have been called
        AppendBatch().Wait();
    }

    private async Task AppendBatch()
    { 
        var commands = new List<Command>();

        while (_commands.TryDequeue(out var command))
        {
            commands.Add(command);
        }

        if (commands.Any())
        {
            foreach (var appendBatch in _appendBatch)
            {
                await appendBatch.Batch(commands);
            }
        }
    }

    public void Stop()
    {
        _stop = true;
        _signal.Set();

        _thread?.Join();
    }

    public void Append(Command command)
    {
        // send the command to separate thread and persist it
        _commands.Enqueue(command);
        _signal.Set();
    }

    public Task<IEnumerable<Command>>? YieldStored()
    {
        return _appendBatch.FirstOrDefault()?.ReadAll();
    }
}