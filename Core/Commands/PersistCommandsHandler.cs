using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SomeBasicFileStoreApp.Core.Commands
{
    public class PersistCommandsHandler:ICommandHandler<Command>
    {
        private Thread thread;
        private bool stop = false;
        private readonly object _key = new object();
        private readonly ConcurrentQueue<Command> Commands = new ConcurrentQueue<Command>();
        private readonly IAppendBatch _appendBatch;
        private EventWaitHandle signal;

        public PersistCommandsHandler(IAppendBatch appendBatch)
        {
            _appendBatch = appendBatch;
            signal = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        public void Start()
        {
            if (thread != null)
            {
                throw new Exception();
            }
            thread = new Thread(ThreadStart);
            thread.Start();
        }

        private void ThreadStart()
        {
            while (!stop)
            {
                signal.WaitOne();
                var commands = new List<Command>();
                lock (_key)
                {
                    Command command;
                    while (Commands.TryDequeue(out command))
                    {
                        commands.Add(command);
                    }
                }
                if (commands.Any())
                {
                    _appendBatch.Batch(commands);
                }
            }
        }

        public void Stop()
        {
            stop = true;
            signal.Set();

            if (thread != null)
            {
                thread.Join();
            }
        }

        public void Handle(Command command)
        {
            lock (_key)
            {
                // send the command to separate thread and persist it
                Commands.Enqueue(command);
            }
            signal.Set();
        }
    }
}

