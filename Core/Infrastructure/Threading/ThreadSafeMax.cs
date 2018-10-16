using System;
using System.Linq;

namespace SomeBasicFileStoreApp.Core.Infrastructure.Threading
{
    /// <summary>
    /// small helper class in order to observe values and store the maximum value
    /// </summary>
    class ThreadSafeMax<T> where T : IComparable
    {
        private readonly object _lockObject = new object();
        /// <summary>
        /// Create an instance of ThreadSafeMax 
        /// </summary>
        /// <param name="value"></param>
        public ThreadSafeMax(T value)
        {
            Value = value;
        }
        public T Value { get; private set; }

        /// <summary>
        /// Observe value, potentially recording new value in this instance
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public T Observe(T value)
        {
            lock (_lockObject)
                Value = new []{value, Value}.Max();
            return value;
        }
    }
}