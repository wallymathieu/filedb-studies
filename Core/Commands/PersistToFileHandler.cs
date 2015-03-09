using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SomeBasicFileStoreApp.Core.Commands
{
    public class PersistToFileHandler:ICommandHandler<Command>
    {
        private Thread thread;
        private bool stop = false;
        private readonly object _key = new object();
        private readonly ConcurrentQueue<Command> Commands = new ConcurrentQueue<Command>();
        private readonly AppendToFile _appendToFile;
        private readonly int _sleep;

        public PersistToFileHandler(AppendToFile appendToFile, int sleep)
        {
            _appendToFile = appendToFile;
            _sleep = sleep;
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
            while (!stop || !Commands.IsEmpty)
            { 
                lock (_key)
                {
                    var commands = new List<Command>();
                    Command command;
                    while (Commands.TryDequeue(out command))
                    {
                        commands.Add(command);
                    }
                    if (commands.Any())
                    {
                        _appendToFile.Batch(commands);
                    }
                }
                Thread.Sleep(_sleep);
            }
        }

        public void Stop()
        {
            lock (_key)
            {
                stop = true;
            }
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
        }
    }
}

